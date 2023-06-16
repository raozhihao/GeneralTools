using System;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;

namespace GeneralTool.CoreLibrary.WPFHelper
{
    /// <summary>
    /// Window帮助类
    /// </summary>
    public static class WindowHelper
    {
        /// <summary>
        /// 获取控件句柄
        /// </summary>
        /// <param name="dependencyObject"></param>
        /// <returns></returns>
        public static IntPtr GetHandleByDependencyObject(this DependencyObject dependencyObject) => dependencyObject.GetHwndSource().Handle;

        /// <summary>
        /// 获取控件win32相关信息
        /// </summary>
        /// <param name="dependencyObject"></param>
        /// <returns></returns>
        public static HwndSource GetHwndSource(this DependencyObject dependencyObject) => (HwndSource)PresentationSource.FromDependencyObject(dependencyObject);

        /// <summary>
        /// 获取控件win32相关信息
        /// </summary>
        /// <param name="visual"></param>
        /// <returns></returns>
        public static HwndSource GetHwndSource(this Visual visual) => (HwndSource)PresentationSource.FromVisual(visual);

    }
}
