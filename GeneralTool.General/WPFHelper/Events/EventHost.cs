using System;
using System.Windows;
using System.Windows.Media;

namespace GeneralTool.General.WPFHelper.Events
{
    /// <summary>
    /// 事件宿主
    /// </summary>
    public class EventHost : Freezable
    {
        /// <summary>
        /// 设置或获取事件名称
        /// </summary>
        public String EventName { get; set; }

        /// <summary>
        /// 获取或设置事件的命令程序
        /// </summary>
        public IEventCommand Command
        {
            get { return GetValue(CommandProerty) as IEventCommand; }
            set { SetValue(CommandProerty, value); }
        }

        /// <summary>
        /// 命令绑定
        /// </summary>
        public static readonly DependencyProperty CommandProerty = DependencyProperty.Register(nameof(Command), typeof(IEventCommand), typeof(EventHost), new PropertyMetadata(CallBackMethod));


        /// <summary>
        /// 获取或设置事件的自定义参数
        /// </summary>
        public object CommandParameter
        {
            get { return GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }

        /// <summary>
        /// 事件自定义参数
        /// </summary>
        public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register(nameof(CommandParameter), typeof(object), typeof(EventHost), new PropertyMetadata(new PropertyChangedCallback(CallBackMethod)));

        private static void CallBackMethod(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }

        /// <summary>
        /// 创建对象
        /// </summary>
        /// <returns></returns>

        protected override Freezable CreateInstanceCore()
        {
            Type type = typeof(Visual);
            return (Freezable)Activator.CreateInstance(type);
        }

        /// <summary>
        /// 注册事件
        /// </summary>
        /// <param name="d"></param>
        public void RegisterEvent(DependencyObject d)
        {
            var cmd = this.Command as IEventCommand;
            var action = cmd.ActionEventHandler;
            var @event = d.GetType().GetEvent(this.EventName);

            var handler = Delegate.CreateDelegate(@event.EventHandlerType, action.Target, action.Method);
            @event.AddEventHandler(d, handler);

        }
    }
}
