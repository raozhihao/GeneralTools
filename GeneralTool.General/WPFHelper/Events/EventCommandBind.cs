using System;
using System.Windows;

namespace GeneralTool.General.WPFHelper.Events
{
    /// <summary>
    /// 单项事件绑定
    /// </summary>
    public class EventCommandBind : DependencyObject
    {

        /// <summary>
        /// 事件,这是依赖属性
        /// </summary>
        public static readonly DependencyProperty EventCommandProerty = DependencyProperty.RegisterAttached("Command", typeof(IEventCommand), typeof(EventCommandBind), new PropertyMetadata(null, new PropertyChangedCallback(CommandChanged)));

        private static void CommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is UIElement control)
            {
                if (e.NewValue is IEventCommand cmd)
                {
                    cmd.SetObject(control);

                    var eventName = GetEventName(d);
                    if (string.IsNullOrWhiteSpace(eventName))
                    {
                        throw new ArgumentNullException($"依赖属性 EventName 必须要设置");
                    }
                    cmd.EventName = eventName;
                    RegisterEvent(control, eventName, cmd);
                }
            }
        }

        private static void RegisterEvent(UIElement d, string eventName, IEventCommand cmd)
        {
            var action = cmd.ActionEventHandler;
            var @event = d.GetType().GetEvent(eventName);
            if (@event == null)
                throw new ArgumentNullException($"事件名称 {eventName} 不存在元素 {d} 中,请检查");

            var handler = Delegate.CreateDelegate(@event.EventHandlerType, action.Target, action.Method);
            @event.AddEventHandler(d, handler);

        }


        /// <summary>
        /// 设置Command
        /// </summary>
        /// <param name="dp"></param>
        /// <param name="value"></param>
        public static void SetCommand(DependencyObject dp, object value)
        {
            dp.SetValue(EventCommandProerty, value);
        }


        /// <summary>
        /// 获取Command
        /// </summary>
        /// <param name="dp"></param>
        /// <returns></returns>
        public static object GetCommand(DependencyObject dp)
        {
            return dp.GetValue(EventCommandProerty);
        }

        /// <summary>
        /// 事件名称,这是依赖属性
        /// </summary>
        public static readonly DependencyProperty EventNameProperty = DependencyProperty.Register("EventName", typeof(string), typeof(EventCommandBind));

        /// <summary>
        /// 设置事件名称
        /// </summary>
        /// <param name="dp"></param>
        /// <param name="value"></param>
        public static void SetEventName(DependencyObject dp, string value)
        {
            dp.SetValue(EventNameProperty, value);
        }

        /// <summary>
        /// 获取事件名称
        /// </summary>
        /// <param name="dp"></param>
        /// <returns></returns>
        public static string GetEventName(DependencyObject dp)
        {
            return (string)dp.GetValue(EventNameProperty);
        }
    }
}
