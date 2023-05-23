using System;

namespace GeneralTool.CoreLibrary.Extensions
{
    /// <summary>
    /// 枚举相关扩展类
    /// </summary>
    public static class EnumExtensions
    {
        #region Public 方法

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
            if (en == null)
                return default;
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

        /// <summary>
        /// 将基础数值或string类型转为指定的枚举类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="code">传入的值必须是基础数值类型,例如Int32</param>
        /// <returns></returns>
        public static T ToEnum<T>(this object code) where T : struct
        {
            if (code is string codeStr)
            {
                Enum.TryParse<T>(codeStr, true, out var result);
                return result;
            }
            return (T)Enum.ToObject(typeof(T), code);
        }

        /// <summary>
        /// 获取枚举指定特性类型的说明
        /// </summary>
        /// <param name="enum">
        /// </param>
        /// <param name="attrType">
        /// 要查找的自定义特性类型
        /// </param>
        /// <param name="attrName">
        /// 要查找的自定义特性中的属性名称(区分大小写)
        /// </param>
        /// <returns>
        /// </returns>
        public static string GetDescription(this Enum @enum, Type attrType, string attrName)
        {
            return @enum.GetInnerDescription(attrType, attrName) + "";
        }

        /// <summary>
        /// 获取枚举指定特性类型的说明
        /// </summary>
        /// <param name="enum">
        /// </param>
        /// <param name="attrType">
        /// 要查找的自定义特性类型
        /// </param>
        /// <param name="attrName">
        /// 要查找的自定义特性中的属性名称(区分大小写)
        /// </param>
        /// <returns>
        /// </returns>
        public static T GetDescription<T>(this Enum @enum, Type attrType, string attrName)
        {
            return (T)@enum.GetInnerDescription(attrType, attrName);
        }

        /// <summary>
        /// 获取枚举指定特性类型的说明
        /// </summary>
        /// <param name="enum">
        /// </param>
        /// <param name="attrType">
        /// 要查找的自定义特性类型
        /// </param>
        /// <param name="attrIndex">
        /// 要查找的自定义特性中的属性下标
        /// </param>
        /// <returns>
        /// </returns>
        public static string GetDescription(this Enum @enum, Type attrType, int attrIndex = 0)
        {
            return @enum.GetInnerDescription(attrType, attrIndex) + "";
        }

        /// <summary>
        /// 获取枚举指定特性类型的说明
        /// </summary>
        /// <param name="enum">
        /// </param>
        /// <param name="attrType">
        /// 要查找的自定义特性类型
        /// </param>
        /// <param name="attrIndex">
        /// 要查找的自定义特性中的属性下标
        /// </param>
        /// <returns>
        /// </returns>
        public static T GetDescription<T>(this Enum @enum, Type attrType, int attrIndex = 0)
        {
            return (T)@enum.GetInnerDescription(attrType, attrIndex);
        }

        /// <summary>
        /// 获取枚举指定特性类型的说明
        /// </summary>
        /// <param name="enum">
        /// </param>
        /// <param name="attrType">
        /// 要查找的自定义特性类型
        /// </param>
        /// <param name="attrName">
        /// 要查找的自定义特性中的属性名称(区分大小写)
        /// </param>
        /// <returns>
        /// </returns>
        public static object GetInnerDescription(this Enum @enum, Type attrType, string attrName)
        {
            if (attrType == null)
            {
                return null;
            }
            Type type = @enum.GetType();

            System.Reflection.FieldInfo field = type.GetField(@enum.ToString());

            object[] objTypes = field.GetCustomAttributes(attrType, false);
            if (objTypes.Length == 0)
            {
                return null;
            }

            object obj = objTypes[0];
            System.Reflection.PropertyInfo attrPro = obj.GetType().GetProperty(attrName);
            if (attrPro == null)
            {
                return null;
            }
            object result = attrPro.GetValue(obj);

            return result;
        }

        /// <summary>
        /// 获取枚举指定特性类型的说明
        /// </summary>
        /// <param name="enum">
        /// </param>
        /// <param name="attrType">
        /// 要查找的自定义特性类型
        /// </param>
        /// <param name="attrIndex">
        /// 要查找的自定义特性中的属性下标
        /// </param>
        /// <returns>
        /// </returns>
        public static object GetInnerDescription(this Enum @enum, Type attrType, int attrIndex = 0)
        {
            if (attrType == null)
            {
                return null;
            }
            Type type = @enum.GetType();

            System.Reflection.FieldInfo field = type.GetField(@enum.ToString());

            object[] objTypes = field.GetCustomAttributes(attrType, false);
            if (objTypes.Length == 0)
            {
                return null;
            }

            object obj = objTypes[0];
            System.Reflection.PropertyInfo[] attrLen = obj.GetType().GetProperties();
            if (attrLen.Length <= attrIndex)
            {
                return null;
            }

            System.Reflection.PropertyInfo attrPro = attrLen[attrIndex];
            if (attrPro == null)
            {
                return null;
            }
            object result = attrPro.GetValue(obj);
            return result;
        }

        #endregion Public 方法
    }
}