using System;
using System.Windows;
using System.Windows.Media;

namespace GeneralTool.General.WPFHelper.Events
{
    /// <summary>
    /// 事件宿主集合
    /// </summary>
    public class EventHosts : DependencyObject
    {
        /// <summary>
        /// 事件集合,这是依赖属性
        /// </summary>
        public static readonly DependencyProperty EventCommandsProerty;

        static EventHosts()
        {
            EventCommandsProerty = DependencyProperty.RegisterAttached("EventCommands", typeof(EventHostCollection), typeof(EventHosts), new FrameworkPropertyMetadata(EventCommandsChanged));
        }
        private static void EventCommandsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue == null)
                return;

            if (!(e.NewValue is EventHostCollection eve))
                return;

            AddEventHost(d, eve);

        }

        private static void AddEventHost(DependencyObject d, EventHostCollection eves)
        {
            foreach (var eve in eves)
            {
                if (eve.Command == null)
                    return;


                if (eve.Command is IEventCommand cmd)
                {
                    cmd.SetObject(d);
                    cmd.EventName = eve.EventName;
                    cmd.SetParameter(eve.CommandParameter);
                    eve.RegisterEvent(d);
                }

            }
        }

        /// <summary>
        /// 获取事件集合
        /// </summary>
        /// <param name="element">元素</param>
        /// <returns></returns>
        public static EventHostCollection GetEventCommands(Visual element)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            var hosts = (EventHostCollection)element.GetValue(EventCommandsProerty);
            if (hosts == null)
            {
                hosts = new EventHostCollection();
                hosts.SetValue(EventCommandsProerty, hosts);
            }

            return hosts;
        }


        /// <summary>
        /// 设置事件集合
        /// </summary>
        /// <param name="element">元素</param>
        /// <param name="hosts">事件集合</param>
        public static void SetEventCommands(Visual element, EventHostCollection hosts)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            if (hosts == null)
            {
                hosts = new EventHostCollection();
                hosts.SetValue(EventCommandsProerty, hosts);
            }

        }
    }
}
