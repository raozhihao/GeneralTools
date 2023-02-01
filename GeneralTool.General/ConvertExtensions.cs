using System;

namespace GeneralTool.General
{
    /// <summary>
    /// 转换
    /// </summary>
    public static class ConvertExtensions
    {
        /// <summary>
        /// 判断值是否能够转换为指定的类型
        /// </summary>
        /// <param name="value">要判断的值</param>
        /// <param name="targetType">要判断的类型</param>
        /// <returns></returns>
        public static bool CanConvert(this object value, Type targetType)
        {
            if (value == null || targetType == null)
                return false;

            var valueType = value.GetType();
            if (valueType == targetType)
                return true;

            if (valueType == typeof(object))
                return true;

            if (valueType.IsAssignableFrom(targetType))
                return true;

            return true;
        }
    }
}
