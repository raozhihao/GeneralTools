using System.Windows;

namespace GeneralTool.General.WPFHelper.Extensions
{
    /// <summary>
    /// </summary>
    public static class DialogResultDependencyProperty
    {
        #region Public 字段

        /// <summary>
        /// </summary>
        public static readonly DependencyProperty DialogResultProperty =
            DependencyProperty.RegisterAttached("DialogResult", typeof(bool?), typeof(DialogResultDependencyProperty), new PropertyMetadata(null, OnDialogResultChanged));

        #endregion Public 字段

        #region Public 方法

        /// <summary>
        /// </summary>
        /// <param name="dp">
        /// </param>
        /// <returns>
        /// </returns>
        public static bool? GetDialogResult(DependencyObject dp)
        {
            return (bool?)dp.GetValue(DialogResultProperty);
        }

        /// <summary>
        /// </summary>
        /// <param name="dp">
        /// </param>
        /// <param name="value">
        /// </param>
        public static void SetDialogResult(DependencyObject dp, bool? value)
        {
            dp.SetValue(DialogResultProperty, value);
        }

        #endregion Public 方法

        #region Private 方法

        private static void OnDialogResultChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is Window window))
                return;
            if (e.NewValue != null)
            {
                window.DialogResult = (bool?)e.NewValue;
            }
        }

        #endregion Private 方法
    }
}