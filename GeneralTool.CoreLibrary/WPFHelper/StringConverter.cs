using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Data;

using GeneralTool.CoreLibrary.Extensions;

namespace GeneralTool.CoreLibrary.WPFHelper
{
    /// <summary>
    /// string字符串值与指定类型的转换器
    /// </summary>
    public class StringConverter : IValueConverter
    {
        /// <inheritdoc/>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //将指定的值转为字符串类型
            string str = value + "";

            if (string.IsNullOrWhiteSpace(str))
                return str;

            Type type = value.GetType();

            TypeConverter converter = TypeDescriptor.GetConverter(type);
            bool flag = converter.CanConvertFrom(value.GetType());
            if (!flag)
            {
                converter = TypeDescriptor.GetConverter(value.GetType());
            }
            if (!flag && !converter.CanConvertTo(type))
            {
                //直接使用Json
                return value.SerializeToJsonString();
            }
            return str;
        }

        /// <inheritdoc/>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //将指定类型的字符串值转为指定类型的值
            string str = value + "";
            return string.IsNullOrEmpty(str) ? null : targetType == null ? str : ConvertSimpleType(str, targetType);
        }

        /// <summary>
        /// 将传入的类型转换为指定的类型
        /// </summary>
        /// <param name="value"></param>
        /// <param name="destinationType"></param>
        /// <returns></returns>
        public object ConvertSimpleType(string value, Type destinationType)
        {
            if (typeof(string) == destinationType)
                return value;

            object returnValue;
            if ((value == null) || destinationType.IsInstanceOfType(value))
            {
                return value;
            }
            string str = value;
            if ((str != null) && (str.Length == 0))
            {
                return null;
            }

            if (destinationType.IsValueType)
            {
                object obj = Activator.CreateInstance(destinationType);
                if (obj is IConvertible)
                {
                    if (destinationType.IsEnum && int.TryParse(value, out var e))
                    {
                        return Enum.Parse(destinationType, value);
                    }
                    return System.Convert.ChangeType(value, destinationType);
                }
            }

            TypeConverter converter = TypeDescriptor.GetConverter(destinationType);
            bool flag = converter.CanConvertFrom(destinationType);
            if (!flag)
            {
                return str.DeserializeJsonToObject(destinationType);
            }

            try
            {
                returnValue = flag ? converter.ConvertFrom(null, null, value) : converter.ConvertTo(null, null, value, destinationType);
            }
            catch (Exception e)
            {
                throw new InvalidOperationException("类型转换出错：" + value.ToString() + "==>" + destinationType, e);
            }
            return returnValue;
        }

        /// <summary>
        /// 将字符串类型转为指定的数据类型
        /// </summary>
        /// <param name="value"></param>
        /// <param name="destinationType"></param>
        /// <returns></returns>
        public object ConvertSimpleType(object value, Type destinationType)
        {
            object returnValue;
            if ((value == null) || destinationType.IsInstanceOfType(value))
            {
                return value;
            }
            string str = value as string;
            if ((str != null) && (str.Length == 0))
            {
                return null;
            }

            if (value is IConvertible)
            {
                return System.Convert.ChangeType(value, destinationType);
            }
            TypeConverter converter = TypeDescriptor.GetConverter(destinationType);
            bool flag = converter.CanConvertFrom(value.GetType());
            if (!flag)
            {
                converter = TypeDescriptor.GetConverter(value.GetType());
            }
            if (!flag && !converter.CanConvertTo(destinationType))
            {
                return str == null
                    ? destinationType.IsValueType ? Activator.CreateInstance(destinationType) : null
                    : str.DeserializeJsonToObject(destinationType);
            }
            try
            {
                returnValue = flag ? converter.ConvertFrom(null, null, value) : converter.ConvertTo(null, null, value, destinationType);
            }
            catch (Exception e)
            {
                throw new InvalidOperationException("类型转换出错：" + value.ToString() + "==>" + destinationType, e);
            }
            return returnValue;
        }

    }
}
