using System;

namespace GeneralTool.General.Enums
{
    /// <summary>
    /// 枚举扩展
    /// </summary>
    public static class EnumExtension
    {
        /// <summary>
        /// 获取枚举上的特性内的属性值
        /// </summary>
        /// <typeparam name="OutType">特性上属性值的返回类型</typeparam>
        /// <typeparam name="EnumAttributeType">枚举上特性类型</typeparam>
        /// <param name="enum">枚举类型</param>
        /// <param name="property">要获取的特性中的属性名称</param>
        /// <returns>返回该属性的值</returns>
        public static OutType GetEnumCustomAttributeInfo<OutType, EnumAttributeType>(this Enum @enum, string property) where EnumAttributeType : Attribute
        {
            var en = @enum.GetEnumCustomAttribute<EnumAttributeType>();
            var p = en.GetType().GetProperty(property);
            if (p == null)
                return default;

            return (OutType)p.GetValue(en);
        }

        /// <summary>
        /// 获取枚举上的指定的特性
        /// </summary>
        /// <typeparam name="T">要获取的特性类型</typeparam>
        /// <param name="enum">要获取的枚举</param>
        /// <returns>返回获取到的特性</returns>
        public static T GetEnumCustomAttribute<T>(this Enum @enum) where T : Attribute
        {
            var type = @enum.GetType();
            var field = type.GetField(@enum.ToString());
            if (field == null)
                return default;

            var datas = field.GetCustomAttributes(typeof(T), false);
            if (datas.Length == 0)
                return default;

            var en = (T)datas[0];
            return en;
        }

    }
}
