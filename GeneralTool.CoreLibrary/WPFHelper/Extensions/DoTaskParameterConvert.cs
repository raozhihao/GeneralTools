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
                StringBuilder builder = new StringBuilder();
                _ = builder.Append("{\"Url\":\"" + task.Url + "\",\"Paramters\":");

                System.Collections.ObjectModel.ObservableCollection<ParameterItem> list = task.Paramters;
                if (list.Count == 0)
                    _ = builder.Append("null}");
                else
                {
                    System.Collections.Generic.IEnumerable<string> listStr = list.Select(p =>
                    {
                        return string.Format("\"{0}\":\"{1}\"", p.ParameterName, p.Value);
                    });
                    _ = builder.Append("{" + string.Join(",", listStr) + "}}");
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