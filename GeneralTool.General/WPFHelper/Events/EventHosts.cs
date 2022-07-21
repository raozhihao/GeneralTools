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

        private static DependencyObject dependencyObject;
        private static void EventCommandsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue == null || e.OldValue != null)
                return;

            if (e.NewValue is EventHostCollection eve)
            {
                dependencyObject = d;
                // AddEventHost(d, eve);
                eve.Changed += Eve_Changed;
            }
        }

        private static void Eve_Changed(object sender, EventArgs e)
        {
            var eve = sender as EventHost;

            if (eve.Command == null)
                return;


            if (eve.Command is IEventCommand cmd)
            {
                cmd.SetObject(dependencyObject);
                cmd.EventName = eve.EventName;
                cmd.SetParameter(eve.CommandParameter);
                eve.RegisterEvent(dependencyObject);
            }


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
            //if (element == null)
            //{
            //    throw new ArgumentNullException("element");
            //}

            //var hosts = (ObservableCollection<EventHost>)element.GetValue(EventCommandsProerty);
            //if (hosts == null)
            //{
            //    hosts = new ObservableCollection<EventHost>();
            //    hosts.SetValue(EventCommandsProerty, hosts);
            //}

            var hosts = element.GetValue(EventCommandsProerty) as EventHostCollection;
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
            }
            element.SetValue(EventCommandsProerty, hosts);
        }
    }
}
