using System;
using System.Windows;
using System.Windows.Controls;

namespace GeneralTool.CoreLibrary.WPFHelper.Extensions
{
    /// <summary>
    /// </summary>
    public class ScrollViewerExtensions
    {
        #region Public 字段

        /// <summary>
        /// </summary>
        public static readonly DependencyProperty AlwaysScrollToEndProperty = DependencyProperty.RegisterAttached("AlwaysScrollToEnd", typeof(bool), typeof(ScrollViewerExtensions), new PropertyMetadata(false, new PropertyChangedCallback(ScrollViewerExtensions.AlwaysScrollToEndChanged)));

        #endregion Public 字段

        #region Private 字段

        private static bool _autoScroll;

        #endregion Private 字段

        #region Public 方法

        /// <summary>
        /// </summary>
        /// <param name="scroll">
        /// </param>
        /// <returns>
        /// </returns>
        public static bool GetAlwaysScrollToEnd(ScrollViewer scroll)
        {
            bool flag = scroll == null;
            return flag ? throw new ArgumentNullException("scroll") : (bool)scroll.GetValue(ScrollViewerExtensions.AlwaysScrollToEndProperty);
        }

        /// <summary>
        /// </summary>
        /// <param name="scroll">
        /// </param>
        /// <param name="alwaysScrollToEnd">
        /// </param>
        public static void SetAlwaysScrollToEnd(ScrollViewer scroll, bool alwaysScrollToEnd)
        {
            bool flag = scroll == null;
            if (flag)
            {
                throw new ArgumentNullException("scroll");
            }
            scroll.SetValue(ScrollViewerExtensions.AlwaysScrollToEndProperty, alwaysScrollToEnd);
        }

        #endregion Public 方法

        #region Private 方法

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

        #endregion Private 方法
    }
}