using System;
using System.Windows;
using System.Windows.Controls;

namespace GeneralTool.General.WPFHelper.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public class ScrollViewerExtensions
    {
        private static void AlwaysScrollToEndChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            ScrollViewer scrollViewer = sender as ScrollViewer;
            bool flag = scrollViewer != null;
            if (flag)
            {
                bool flag2 = e.NewValue != null && (bool)e.NewValue;
                bool flag3 = flag2;
                if (flag3)
                {
                    scrollViewer.ScrollToEnd();
                    scrollViewer.ScrollChanged += ScrollViewerExtensions.ScrollChanged;
                }
                else
                {
                    scrollViewer.ScrollChanged -= ScrollViewerExtensions.ScrollChanged;
                }
                return;
            }
            throw new InvalidOperationException("附加的Always Scroll To End属性只能应用于Scroll Viewer实例.");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="scroll"></param>
        /// <returns></returns>
        public static bool GetAlwaysScrollToEnd(ScrollViewer scroll)
        {
            bool flag = scroll == null;
            if (flag)
            {
                throw new ArgumentNullException("scroll");
            }
            return (bool)scroll.GetValue(ScrollViewerExtensions.AlwaysScrollToEndProperty);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="scroll"></param>
        /// <param name="alwaysScrollToEnd"></param>
        public static void SetAlwaysScrollToEnd(ScrollViewer scroll, bool alwaysScrollToEnd)
        {
            bool flag = scroll == null;
            if (flag)
            {
                throw new ArgumentNullException("scroll");
            }
            scroll.SetValue(ScrollViewerExtensions.AlwaysScrollToEndProperty, alwaysScrollToEnd);
        }


        private static void ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            ScrollViewer scrollViewer = sender as ScrollViewer;
            bool flag = scrollViewer == null;
            if (flag)
            {
                throw new InvalidOperationException("附加的Always Scroll To End属性只能应用于Scroll Viewer实例.");
            }
            bool flag2 = e.ExtentHeightChange == 0.0;
            if (flag2)
            {
                ScrollViewerExtensions._autoScroll = (scrollViewer.VerticalOffset == scrollViewer.ScrollableHeight);
            }
            bool flag3 = ScrollViewerExtensions._autoScroll && e.ExtentHeightChange != 0.0;
            if (flag3)
            {
                scrollViewer.ScrollToVerticalOffset(scrollViewer.ExtentHeight);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty AlwaysScrollToEndProperty = DependencyProperty.RegisterAttached("AlwaysScrollToEnd", typeof(bool), typeof(ScrollViewerExtensions), new PropertyMetadata(false, new PropertyChangedCallback(ScrollViewerExtensions.AlwaysScrollToEndChanged)));


        private static bool _autoScroll;
    }
}
