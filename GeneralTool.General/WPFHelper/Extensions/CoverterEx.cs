using System.Windows.Data;

namespace GeneralTool.General.WPFHelper.Extensions
{
    /// <summary>
    /// </summary>
    public class CoverterEx
    {
        #region Public 属性

        /// <summary>
        /// Object to String
        /// </summary>
        public IValueConverter ObjectToStringConverter => ValueConverter.Create<object, string>(ConvertMethod, CallBack);

        /// <summary>
        /// Object to TypeString
        /// </summary>
        public IValueConverter ObjectToTypeStringConverter => ValueConverter.Create<object, string>(TypeConvertMethod, TypeCallBack);

        #endregion Public 属性

        #region Private 方法

        private object CallBack(ValueConverterArgs<string> arg)
        {
            return arg.Value;
        }

        private string ConvertMethod(ValueConverterArgs<object> s)
        {
            return s.Value?.ToString() ?? s.Value + "";
        }

        private object TypeCallBack(ValueConverterArgs<string> arg)
        {
            return arg.TargetType.FullName;
        }

        private string TypeConvertMethod(ValueConverterArgs<object> s)
        {
            return s.Value + "";
        }

        #endregion Private 方法
    }
}