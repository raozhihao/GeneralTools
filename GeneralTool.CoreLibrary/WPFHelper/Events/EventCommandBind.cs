using System;
using System.Windows;

namespace GeneralTool.CoreLibrary.WPFHelper.Events
{
    /// <summary>
    /// 单项事件绑定
    /// </summary>
    public class EventCommandBind : DependencyObject
    {
        #region Public 字段

        /// <summary>
        /// 事件,这是依赖属性
        /// </summary>
        public static readonly DependencyProperty EventCommandProerty = DependencyProperty.RegisterAttached("Command", typeof(IEventCommand), typeof(EventCommandBind), new PropertyMetadata(null, new PropertyChangedCallback(CommandChanged)));

        /// <summary>
        /// 事件名称,这是依赖属性
        /// </summary>
        public static readonly DependencyProperty EventNameProperty = DependencyProperty.Register("EventName", typeof(string), typeof(EventCommandBind));

        #endregion Public 字段

        #region Public 方法

        /// <summary>
        /// 获取Command
        /// </summary>
        /// <param name="dp">
        /// </param>
        /// <returns>
        /// </returns>
        public static object GetCommand(DependencyObject dp)
        {
            return dp.GetValue(EventCommandProerty);
        }

        /// <summary>
        /// 获取事件名称
        /// </summary>
        /// <param name="dp">
        /// </param>
        /// <returns>
        /// </returns>
        public static string GetEventName(DependencyObject dp)
        {
            return (string)dp.GetValue(EventNameProperty);
        }

        /// <summary>
        /// 设置Command
        /// </summary>
        /// <param name="dp">
        /// </param>
        /// <param name="value">
        /// </param>
        public static void SetCommand(DependencyObject dp, object value)
        {
            dp.SetValue(EventCommandProerty, value);
        }

        /// <summary>
        /// 设置事件名称
        /// </summary>
        /// <param name="dp">
        /// </param>
        /// <param name="value">
        /// </param>
        public static void SetEventName(DependencyObject dp, string value)
        {
            dp.SetValue(EventNameProperty, value);
        }

        #endregion Public 方法

        #region Private 方法

        private static void CommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is UIElement control)
            {
                if (e.NewValue is IEventCommand cmd)
                {
                    cmd.SetObject(control);

                    string eventName = GetEventName(d);
                    if (string.IsNullOrWhiteSpace(eventName))
                    {
                        throw new ArgumentNullException($"依赖属性 EventName 必须要设置");
                    }
                    cmd.EventName = eventName;
                    RegisterEvent(control, eventName, cmd);
                }
            }
        }

        private static RoutedEvent GetRouteEvent(UIElement b, string eventName)
        {
            eventName += "Event";

            System.Reflection.FieldInfo property = typeof(UIElement).GetField(eventName);
            return property != null ? (RoutedEvent)property.GetValue(b) : null;
        }

        private static void RegisterEvent(UIElement d, string eventName, IEventCommand cmd)
        {
            EventHandler action = cmd.ActionEventHandler;
            System.Reflection.EventInfo @event = d.GetType().GetEvent(eventName) ?? throw new ArgumentNullException($"事件名称 {eventName} 不存在元素 {d} 中,请检查");
            Delegate handler = Delegate.CreateDelegate(@event.EventHandlerType, action.Target, action.Method);

            RoutedEvent routeEvent = GetRouteEvent(d, eventName);
            if (routeEvent != null)
                d.AddHandler(routeEvent, handler, true);
            else
                @event.AddEventHandler(d, handler);
        }

        #endregion Private 方法
    }
}