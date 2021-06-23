using System.Windows;

namespace GeneralTool.General.WPFHelper.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class DialogResultDependencyProperty
    {
        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty DialogResultProperty =
            DependencyProperty.RegisterAttached("DialogResult", typeof(bool?), typeof(DialogResultDependencyProperty), new PropertyMetadata(null, OnDialogResultChanged));

        private static void OnDialogResultChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Window window = d as Window;
            if (window == null)
                return;
            if (e.NewValue != null)
            {
                window.DialogResult = (bool?)e.NewValue;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dp"></param>
        /// <returns></returns>
        public static bool? GetDialogResult(DependencyObject dp)
        {
            return (bool?)dp.GetValue(DialogResultProperty);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dp"></param>
        /// <param name="value"></param>
        public static void SetDialogResult(DependencyObject dp, bool? value)
        {
            dp.SetValue(DialogResultProperty, value);
        }
    }
}
