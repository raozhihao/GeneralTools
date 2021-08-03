using System;

namespace GeneralTool.General.ValueTypeExtensions
{
    /// <summary>
    /// 枚举相关扩展类
    /// </summary>
    public static class EnumExtensions
    {
        #region Public 方法

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