using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

using GeneralTool.CoreLibrary.Attributes;
using GeneralTool.CoreLibrary.Extensions;

namespace GeneralTool.CoreLibrary.Ioc
{
    /// <summary>
    /// 简单Ioc服务
    /// </summary>
    public class SimpleIocSerivce
    {
        private SimpleIocSerivce() { }

        private static readonly Lazy<SimpleIocSerivce> serivce;
        /// <summary>
        /// 服务实例
        /// </summary>
        public static SimpleIocSerivce SimpleIocSerivceInstance { get; }

        /// <summary>
        /// 获取当前所有的已注入对象,请先执行Start
        /// </summary>
        public IEnumerable<object> Instaces
        {
            get
            {
                return from i in typeDics select i.Value;
            }
        }

        static SimpleIocSerivce()
        {
            serivce = new Lazy<SimpleIocSerivce>(() => new SimpleIocSerivce());
            SimpleIocSerivceInstance = serivce.Value;
        }

        private readonly Dictionary<Type, DefinedClass> typeDics = new Dictionary<Type, DefinedClass>();

        private readonly Dictionary<Type, DefinedClass> interfaceDics = new Dictionary<Type, DefinedClass>();

        /// <summary>
        /// 开始
        /// </summary>
        public void Start()
        {
            //赋值
            foreach (KeyValuePair<Type, DefinedClass> item in typeDics)
            {
                DefinedClass defindClass = item.Value;
                this.SetValue(defindClass, item.Key);
            }

            //调用构造函数
            foreach (KeyValuePair<Type, DefinedClass> item in typeDics)
            {
                DefinedClass defindClass = item.Value;
                InjectTypeAttribute attrs = defindClass.TType.GetCustomAttribute<InjectTypeAttribute>();
                //进行构造函数调用
                if (!item.Value.IsUninitializedObject)
                    continue;//非实例对象不用调用

                Type type = defindClass.Instance.GetType();
                //没有构造函数的,则查找第一个构造函数
                ConstructorInfo[] constructors = type.GetConstructors();
                if (constructors.Length == 0)
                    throw new Exception($"类型 : {type} 没有公共的构造函数");

                //直接获取第一个
                ConstructorInfo constructor = constructors[0];
                ParameterInfo[] parameters = constructor.GetParameters();

                object[] datas = new object[parameters.Length];
                int index = 0;
                foreach (ParameterInfo pro in parameters)
                {
                    Type parameterType = pro.ParameterType;
                    //查看当前属性的类型是否已注册
                    if (typeDics.ContainsKey(parameterType))
                        datas[index] = typeDics[parameterType].Instance;
                    else if (parameterType.IsInterface)
                    {
                        //如果是接口,则找到与其一致的
                        if (interfaceDics.ContainsKey(parameterType))
                            datas[index] = interfaceDics[parameterType].Instance;  //存在,则进行赋值
                        else
                            datas[index] = default;
                    }
                    else
                        datas[index] = default;
                    index++;
                }
                _ = constructor.Invoke(defindClass.Instance, datas);
            }

            //调用函数
            foreach (KeyValuePair<Type, DefinedClass> item in typeDics)
            {
                if (item.Value.Instance is IInitInterface t)
                    t.Init();
                _ = (item.Value.InitMethod?.Invoke(item.Value.Instance, null));
            }
        }

        private void SetValue(DefinedClass defindClass, Type key)
        {
            //获取所有属性
            PropertyInfo[] pros = key.GetProperties();
            foreach (PropertyInfo pro in pros)
            {
                Type propertyType = pro.PropertyType;
                //查看当前属性的类型是否已注册
                if (typeDics.ContainsKey(propertyType))
                {
                    //存在,则进行赋值
                    if (pro.SetMethod != null)
                    {
                        object obj = typeDics[propertyType].Instance;
                        pro.SetValue(defindClass.Instance, obj);
                    }
                }
                else if (propertyType.IsInterface)
                {
                    //如果是接口,则找到与其一致的
                    if (interfaceDics.ContainsKey(propertyType))
                    {
                        //存在,则进行赋值
                        if (pro.SetMethod != null)
                        {
                            object obj = interfaceDics[propertyType].Instance;
                            pro.SetValue(defindClass.Instance, obj);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 注册指定程序集中的所有类型
        /// </summary>
        /// <param name="assembly">程序集</param>
        /// <param name="checkInjectTypeAttr">是否检查类型上有 InjectType 特性</param>
        public void Inject(Assembly assembly, bool checkInjectTypeAttr = false) => Inject(checkInjectTypeAttr, assembly.ExportedTypes.ToArray());


        /// <summary>
        /// 注册指定的类型集合
        /// </summary>
        /// <param name="types">类型集合</param>
        /// <param name="checkInjectTypeAttr">是否检查类型上有 InjectType 特性</param>
        public void Inject(bool checkInjectTypeAttr, params Type[] types)
        {
            if (types == null)
                return;

            foreach (Type type in types)
            {
                //查看是否有Route标记
                if (checkInjectTypeAttr)
                {
                    InjectTypeAttribute attr = type.GetCustomAttribute<InjectTypeAttribute>();
                    if (attr == null)
                        continue;
                }

                try
                {
                    Inject(type);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Trace.WriteLine($"在创建类 : {type} 时出错 - {ex.GetInnerExceptionMessage()}");
                }
            }
        }

        /// <summary>
        /// 注册指定实例集合
        /// </summary>
        /// <param name="Instances">实例集合</param>
        public void Inject(params object[] Instances)
        {
            if (Instances == null) return;
            foreach (object obj in Instances)
            {
                Inject(obj, null);
            }
        }

        /// <summary>
        /// 注册类型
        /// </summary>
        /// <typeparam name="T">实例类型</typeparam>
        public void Inject<T>() where T : class
        {
            Type type = typeof(T);
            Inject(type);
        }

        /// <summary>
        /// 注册类型
        /// </summary>
        /// <param name="type">实例类型</param>
        /// <param name="methodName"></param>
        public void Inject(Type type, string methodName = null)
        {
            object obj = FormatterServices.GetUninitializedObject(type);
            Inject(obj, true, methodName);
        }

        /// <summary>
        /// 注册类型
        /// </summary>
        /// <param name="obj">实例</param>
        /// <param name="methodName">需要调用的方法名称</param>
        public void Inject(object obj, string methodName = null) => Inject(obj, false, methodName);

        /// <summary>
        /// 注册类型
        /// </summary>
        /// <param name="obj">实例</param>
        /// <param name="isUninitializedObject"></param>
        /// <param name="methodName">需要调用的方法名称</param>
        private void Inject(object obj, bool isUninitializedObject = false, string methodName = null) => Inject(null, obj, isUninitializedObject, methodName);

        /// <summary>
        /// 注册类型
        /// </summary>
        /// <typeparam name="TInterface">接口类型</typeparam>
        /// <param name="instance">实例</param>
        /// <param name="methodName">需要调用的方法名称</param>
        public void Inject<TInterface>(object instance, string methodName = null) where TInterface : class
        {
            Type intefaceType = typeof(TInterface);
            Inject(intefaceType, instance, false, methodName);
        }

        /// <summary>
        /// 注册类型
        /// </summary>
        /// <param name="intefaceType">接口类型</param>
        /// <param name="instance">实例</param>
        /// <param name="isUninitializedObject"></param>
        /// <param name="methodName">需要调用的方法名称</param>
        private void Inject(Type intefaceType, object instance, bool isUninitializedObject, string methodName = null)
        {
            Type type = instance.GetType();
            if (intefaceType != null)
            {
                //判断是否为接口
                if (!intefaceType.IsInterface)
                {
                    throw new Exception($"类型 {intefaceType} 不是一个接口");
                }
                if (type.IsAssignableFrom(intefaceType))
                {
                    throw new Exception($"类型 {type} 不实现接口 {intefaceType}");
                }
            }

            MethodInfo method = null;
            if (!string.IsNullOrWhiteSpace(methodName))
                method = type.GetMethod(methodName);
            var defined = new DefinedClass() { Instance = instance, InterfaceType = intefaceType, InitMethod = method, TType = type, IsUninitializedObject = isUninitializedObject };
            if (!typeDics.ContainsKey(type))
                typeDics.Add(type, defined);
            else
                typeDics[type] = defined;

            if (intefaceType != null)
            {
                if (!interfaceDics.ContainsKey(intefaceType))
                    interfaceDics.Add(intefaceType, defined);
                else
                    interfaceDics[intefaceType] = defined;
            }
        }

        public void RuntimeInject(object instance)
        {
            Type type = instance.GetType();
            var defined = new DefinedClass() { Instance = instance, InterfaceType = null, InitMethod = null, TType = type, IsUninitializedObject = false };
            if (!typeDics.ContainsKey(type))
                typeDics.Add(type, defined);
            else
                typeDics[type] = defined;

            //进行实例化
            this.SetValue(defined, type);
        }

        /// <summary>
        /// 注册类型
        /// </summary>
        /// <param name="intefaceType">接口类型</param>
        /// <param name="type">接口对应实例类型(需要无参构造函数)</param>
        /// <param name="methodName">调用方法</param>
        public void Inject(Type intefaceType, Type type, string methodName = null)
        {
            object instance = FormatterServices.GetUninitializedObject(type);
            Inject(intefaceType, instance, true, methodName);

        }

        /// <summary>
        /// 注册类型
        /// </summary>
        /// <typeparam name="TInterface">接口类型</typeparam>
        /// <typeparam name="TType">实例类型</typeparam>
        /// <param name="methodName">实例化成功后调用的方法名称</param>
        public void Inject<TInterface, TType>(string methodName = null) where TType : class => Inject(typeof(TInterface), typeof(TType), methodName);

        /// <summary>
        /// 注册类型并确定实例化后调用的方法名称
        /// </summary>
        /// <typeparam name="T">实例类型</typeparam>
        /// <param name="methodName">实例化成功后调用的方法名称</param>
        public void Inject<T>(string methodName = null) where T : class
        {
            Type type = typeof(T);
            Inject(type, methodName);
        }

        /// <summary>
        /// 获取类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T Resolve<T>() where T : class => Resolve(typeof(T)) as T;

        /// <summary>
        /// 获取类型
        /// </summary>
        /// <param name="type">要获取的类型</param>
        /// <returns></returns>
        public object Resolve(Type type)
        {
            var instance = typeDics.ContainsKey(type) ? typeDics[type].Instance : null;
            instance = instance == null ? (interfaceDics.ContainsKey(type) ? interfaceDics[type].Instance : null) : instance;

            foreach (var item in typeDics)
            {
                if (type.IsAssignableFrom(item.Key))
                {
                    return typeDics[item.Key].Instance;
                }
            }

            foreach (var item in interfaceDics)
            {
                if (type.IsAssignableFrom(item.Key))
                {
                    return interfaceDics[item.Key].Instance;
                }
            }
            return null;
        }
    }
}
