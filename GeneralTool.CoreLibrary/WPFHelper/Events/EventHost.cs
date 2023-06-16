using System;
using System.Windows;
using System.Windows.Media;

namespace GeneralTool.CoreLibrary.WPFHelper.Events
{
    /// <summary>
    /// 事件宿主
    /// </summary>
    public class EventHost : Freezable
    {
        #region Public 字段

        /// <summary>
        /// 事件自定义参数
        /// </summary>
        public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register(nameof(CommandParameter), typeof(object), typeof(EventHost), new PropertyMetadata(new PropertyChangedCallback(CallBackMethod)));

        /// <summary>
        /// 命令绑定
        /// </summary>
        public static readonly DependencyProperty CommandProerty = DependencyProperty.Register(nameof(Command), typeof(IEventCommand), typeof(EventHost), new PropertyMetadata(CallBackMethod));

        #endregion Public 字段

        #region Public 属性

        /// <summary>
        /// 获取或设置事件的命令程序
        /// </summary>
        public IEventCommand Command
        {
            get { return GetValue(CommandProerty) as IEventCommand; }
            set { SetValue(CommandProerty, value); }
        }

        /// <summary>
        /// 获取或设置事件的自定义参数
        /// </summary>
        public object CommandParameter
        {
            get { return GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }

        /// <summary>
        /// 设置或获取事件名称
        /// </summary>
        public string EventName { get; set; }

        #endregion Public 属性

        #region Public 方法

        /// <summary>
        /// 注册事件
        /// </summary>
        /// <param name="d">
        /// </param>
        public void RegisterEvent(DependencyObject d)
        {
            IEventCommand cmd = Command;
            EventHandler action = cmd.ActionEventHandler;
            System.Reflection.EventInfo @event = d.GetType().GetEvent(EventName);

            Delegate handler = Delegate.CreateDelegate(@event.EventHandlerType, action.Target, action.Method);
            @event.AddEventHandler(d, handler);
        }

        #endregion Public 方法

        #region Protected 方法
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override Freezable CreateInstanceCore()
        {
            Type type = typeof(Visual);
            return (Freezable)Activator.CreateInstance(type);
        }

        #endregion Protected 方法

        #region Private 方法

        private static void CallBackMethod(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
        }

        #endregion Private 方法
    }
}