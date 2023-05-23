using System;
using System.Windows;
using System.Windows.Media;

namespace GeneralTool.CoreLibrary.WPFHelper.Events
{
    /// <summary>
    /// 事件宿主集合
    /// </summary>
    public class EventHosts : DependencyObject
    {
        #region Public 字段

        /// <summary>
        /// 事件集合,这是依赖属性
        /// </summary>
        public static readonly DependencyProperty EventCommandsProerty;

        #endregion Public 字段

        #region Private 字段

        private static DependencyObject dependencyObject;

        #endregion Private 字段

        #region Public 构造函数

        static EventHosts()
        {
            EventCommandsProerty = DependencyProperty.RegisterAttached("EventCommands", typeof(EventHostCollection), typeof(EventHosts), new FrameworkPropertyMetadata(EventCommandsChanged));
        }

        #endregion Public 构造函数

        #region Public 方法

        /// <summary>
        /// 获取事件集合
        /// </summary>
        /// <param name="element">
        /// 元素
        /// </param>
        /// <returns>
        /// </returns>
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
        /// <param name="element">
        /// 元素
        /// </param>
        /// <param name="hosts">
        /// 事件集合
        /// </param>
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

        #endregion Public 方法

        #region Private 方法


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

        #endregion Private 方法
    }
}