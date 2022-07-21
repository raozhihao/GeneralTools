using System.Windows.Data;

namespace GeneralTool.General.WPFHelper.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public  class CoverterEx
    {

        /// <summary>
        /// Object to String
        /// </summary>
        public  IValueConverter ObjectToStringConverter => ValueConverter.Create<object, string>(ConvertMethod, CallBack);



        private  string ConvertMethod(ValueConverterArgs<object> s)
        {
            return s.Value?.ToString() ?? s.Value + "";
        }

        private  object CallBack(ValueConverterArgs<string> arg)
        {
            return arg.Value;
        }

        /// <summary>
        /// Object to TypeString
        /// </summary>
        public  IValueConverter ObjectToTypeStringConverter => ValueConverter.Create<object, string>(TypeConvertMethod, TypeCallBack);



        private string TypeConvertMethod(ValueConverterArgs<object> s)
        {
            return s.Value + "";
        }

        private object TypeCallBack(ValueConverterArgs<string> arg)
        {
            return arg.TargetType.FullName;
        }
    }
}
