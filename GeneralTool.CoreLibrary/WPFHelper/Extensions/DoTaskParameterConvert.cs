using System;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;

using GeneralTool.CoreLibrary.Models;

namespace GeneralTool.CoreLibrary.WPFHelper.Extensions
{
    /// <summary>
    /// </summary>
    [ValueConversion(typeof(string), typeof(DoTaskParameterItem))]
    public class DoTaskParameterConvert : IValueConverter
    {
        #region Public 方法

        ///<inheritdoc/>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DoTaskParameterItem task)
            {
                var builder = new StringBuilder();
                builder.Append("{\"Url\":\"" + task.Url + "\",\"Paramters\":");

                var list = task.Paramters;
                if (list.Count == 0)
                    builder.Append("null}");
                else
                {
                    var listStr = list.Select(p =>
                    {
                        return string.Format("\"{0}\":\"{1}\"", p.ParameterName, p.Value);
                    });
                    builder.Append("{" + string.Join(",", listStr) + "}}");
                }
                return builder.ToString();
            }
            return value;
        }

        ///<inheritdoc/>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }

        #endregion Public 方法
    }
}