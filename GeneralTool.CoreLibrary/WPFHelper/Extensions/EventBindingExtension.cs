using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace GeneralTool.CoreLibrary.WPFHelper.Extensions
{
    /// <summary>
    /// 事件绑定标记扩展类
    /// </summary>
    public class EventBindingExtension : MarkupExtension
    {
        #region Private 字段

        private readonly Dictionary<Type, Delegate> _dummyHandlers = new Dictionary<Type, Delegate>();

        private EventInfo _eventInfo;

        /// <summary>
        /// 绑定的方法
        /// </summary>
        private MethodInfo bindingMethodInfo;

        /// <summary>
        /// 控件绑定的DataContext上对应的方法
        /// </summary>
        private MethodInfo dataContextMethodInfo;

        /// <summary>
        /// 初始路由委托
        /// </summary>
        private Delegate initRouteDelegate;

        private bool isEvent;

        private DependencyObject targetObj;

        #endregion Private 字段

        #region Public 构造函数

        /// <summary>
        /// </summary>
        public EventBindingExtension()
        {
        }

        /// <summary>
        /// </summary>
        /// <param name="eventHandlerName">
        /// </param>
        public EventBindingExtension(string eventHandlerName) : base()
        {
            EventHandlerName = eventHandlerName;
        }

        #endregion Public 构造函数

        #region Public 属性

        /// <summary>
        /// </summary>
        [ConstructorArgument(nameof(EventHandlerName))]
        public string EventHandlerName { get; set; }

        /// <summary>
        /// 要绑定的事件标识路由事件特征
        /// </summary>
        public RoutedEvent RoutedEvent { get; set; }

        #endregion Public 属性

        #region Public 方法

        /// <summary>
        /// 执行方法
        /// </summary>
        /// <param name="sender">
        /// </param>
        /// <param name="e">
        /// </param>
        public void MethodEvent(object sender, object e)
        {
            if (!(sender is  FrameworkElement element))
                return;
            if (dataContextMethodInfo != null && element.DataContext != null)
            {
                ParameterInfo[] p = dataContextMethodInfo.GetParameters();
                int plen = p.Length;
                List<object> list = new List<object>();
                if (plen == 0)
                {
                }
                else if (plen == 1)
                {
                    if (p[0].ParameterType == sender.GetType())
                    {
                        list.Add(sender);
                    }
                    else
                    {
                        list.Add(e);
                    }
                }
                else
                {
                    list.Add(sender);
                    list.Add(e);
                }

                _ = dataContextMethodInfo.Invoke(element.DataContext, list.ToArray());
            }
        }

        ///<inheritdoc/>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (string.IsNullOrEmpty(EventHandlerName))
                throw new ArgumentException("The EventHandlerName property is not set", "EventHandlerName");

            IProvideValueTarget target = (IProvideValueTarget)serviceProvider.GetService(typeof(IProvideValueTarget));

            targetObj = target.TargetObject as DependencyObject;
            if (targetObj == null)
                throw new InvalidOperationException("The target object must be a DependencyObject");

            if (target.TargetProperty is EventInfo @event)
            {
                isEvent = true;
                _eventInfo = @event;
                return CreateEventHandler(targetObj);
            }
            else if (target.TargetProperty is MethodInfo m)
            {
                isEvent = false;

                bindingMethodInfo = m;
                //查看是否为路由事件
                Type eventType = null;
                ParameterInfo[] parameters = m.GetParameters();
                if (parameters.Length == 2)
                {
                    ParameterInfo p1 = parameters[0];

                    ParameterInfo p2 = parameters[1];

                    _ = p1.ParameterType.IsAssignableFrom(typeof(DependencyObject)) && p2.ParameterType.IsSubclassOf(typeof(System.MulticastDelegate));
                    {
                        //为路由事件
                        MethodInfo method = p2.ParameterType.GetMethod("Invoke");
                        bindingMethodInfo = method ?? throw new Exception("不支持的路由事件");
                        eventType = p2.ParameterType;
                    }
                }

                //如果用户绑定的路由标识
                if (RoutedEvent != null)
                {
                    //创建一个初始的路由委托返回,方法类型必须要有返回
                    initRouteDelegate = CreateHandler(targetObj, bindingMethodInfo, eventType);
                    return initRouteDelegate;
                }
                Delegate @delegate = CreateHandler(targetObj, m, eventType);

                return @delegate;
            }
            return null;
        }

        #endregion Public 方法

        #region Private 方法

        private Delegate CreateDummyHandler(Type eventHandlerType)
        {
            Type[] parameterTypes = GetParameterTypes(eventHandlerType);
            Type returnType = eventHandlerType.GetMethod("Invoke").ReturnType;
            DynamicMethod dm = new DynamicMethod(eventHandlerType.Name, returnType, parameterTypes, targetObj.GetType(), true);
            ILGenerator il = dm.GetILGenerator();
            if (returnType != typeof(void))
            {
                if (returnType.IsValueType)
                {
                    LocalBuilder local = il.DeclareLocal(returnType);
                    il.Emit(OpCodes.Ldloca_S, local);
                    il.Emit(OpCodes.Initobj, returnType);
                    il.Emit(OpCodes.Ldloc_0);
                }
                else
                {
                    il.Emit(OpCodes.Ldnull);
                }
            }

            il.Emit(OpCodes.Ret);
            Delegate de = dm.CreateDelegate(eventHandlerType);
            return de;
        }

        private Delegate CreateEventHandler(DependencyObject targetObj)
        {
            object dataContext = GetDataContext(targetObj);
            if (dataContext == null)
            {
                SubscribeToDataContextChanged(targetObj);
                return GetDummyHandler(_eventInfo.EventHandlerType);
            }

            Delegate handler = GetHandler(dataContext, _eventInfo, EventHandlerName);
            if (handler == null)
            {
                Trace.TraceError(
                    "EventBinding: no suitable method named '{0}' found in type '{1}' to handle event '{2'}",
                    EventHandlerName,
                    dataContext.GetType(),
                    _eventInfo);
                return GetDummyHandler(_eventInfo.EventHandlerType);
            }

            return handler;
        }

        private Delegate CreateHandler(DependencyObject targetObj, MethodInfo m, Type eventType = null)
        {
            object dataContext = GetDataContext(targetObj);
            if (dataContext == null)
            {
                SubscribeToDataContextChanged(targetObj);
            }
            else
            {
                dataContextMethodInfo = dataContext.GetType().GetMethod(EventHandlerName);
            }
            if (eventType == null)
            {
                return LinqToDelegate(m);
            }
            else
            {
                MethodInfo method = GetType().GetMethod(nameof(MethodEvent));
                Delegate eventHandler = Delegate.CreateDelegate(eventType, this, method);

                return eventHandler;
            }
        }

        private object GetDataContext(DependencyObject target)
        {

            object context = target.GetValue(FrameworkElement.DataContextProperty)
                   ?? target.GetValue(FrameworkContentElement.DataContextProperty);
            if (context is ObjectDataProvider c)
                context = c.ObjectInstance;
            return context;
        }

        private Delegate GetDummyHandler(Type eventHandlerType)
        {
            if (!_dummyHandlers.TryGetValue(eventHandlerType, out Delegate handler))
            {
                handler = CreateDummyHandler(eventHandlerType);
                _dummyHandlers[eventHandlerType] = handler;
            }
            return handler;
        }

        private Delegate GetHandler(object dataContext, EventInfo eventInfo, string eventHandlerName)
        {
            Type dcType = dataContext.GetType();

            MethodInfo method = dcType.GetMethod(
                eventHandlerName,
                GetParameterTypes(eventInfo.EventHandlerType));
            return method != null
                ? method.IsStatic
                    ? Delegate.CreateDelegate(eventInfo.EventHandlerType, method)
                    : Delegate.CreateDelegate(eventInfo.EventHandlerType, dataContext, method)
                : null;
        }

        private Type[] GetParameterTypes(Type delegateType)
        {
            MethodInfo invokeMethod = delegateType.GetMethod("Invoke");
            return invokeMethod.GetParameters().Select(p => p.ParameterType).ToArray();
        }

        private Delegate LinqToDelegate(MethodInfo m)
        {
            //获取EventType
            Type returnType = m.ReturnType;
            ParameterInfo[] ps = m.GetParameters();
            List<System.Linq.Expressions.ParameterExpression> pes = new List<System.Linq.Expressions.ParameterExpression>();
            foreach (ParameterInfo item in ps)
            {
                pes.Add(System.Linq.Expressions.Expression.Parameter(item.ParameterType));
            }

            System.Linq.Expressions.DefaultExpression body = System.Linq.Expressions.Expression.Default(returnType);

            System.Linq.Expressions.LambdaExpression lab = System.Linq.Expressions.Expression.Lambda(body, "lab", false, pes);
            Delegate labDe = lab.Compile();
            Delegate de = m.CreateDelegate(labDe.GetType());
            //var de = this.GetDummyHandler(labDe.GetType());
            return de;
        }

        private void SubscribeToDataContextChanged(DependencyObject targetObj)
        {
            DependencyPropertyDescriptor
                .FromProperty(FrameworkElement.DataContextProperty, targetObj.GetType())
                .AddValueChanged(targetObj, TargetObject_DataContextChanged);
        }

        private void TargetObject_DataContextChanged(object sender, EventArgs e)
        {
            if (!(sender is  DependencyObject targetObj))
                return;

            UnsubscribeFromDataContextChanged(targetObj);
            object dataContext = GetDataContext(targetObj);
            if (dataContext == null)
                return;

            //在其DataContext中查找有没有绑定的方法
            dataContextMethodInfo = dataContext.GetType().GetMethod(EventHandlerName);
            if (dataContextMethodInfo == null)
                return;

            if (!isEvent)
            {
                FrameworkElement element = sender as FrameworkElement;
                if (initRouteDelegate != null)
                {
                    //绑定的是EventHandler,则先去除已经默认的绑定
                    element.RemoveHandler(RoutedEvent, initRouteDelegate);
                    //将委托注册到路由中
                    element.AddHandler(RoutedEvent, initRouteDelegate, true);
                }
            }
            else
            {
                Delegate handler = GetHandler(dataContext, _eventInfo, EventHandlerName);
                if (handler != null)
                {
                    _eventInfo.AddEventHandler(targetObj, handler);
                }
            }
        }

        private void UnsubscribeFromDataContextChanged(DependencyObject targetObj)
        {
            DependencyPropertyDescriptor
                .FromProperty(FrameworkElement.DataContextProperty, targetObj.GetType())
                .RemoveValueChanged(targetObj, TargetObject_DataContextChanged);
        }

        #endregion Private 方法
    }
}