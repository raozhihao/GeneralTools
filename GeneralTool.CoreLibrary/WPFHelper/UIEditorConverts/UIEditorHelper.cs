using System;
using System.Windows;

namespace GeneralTool.CoreLibrary.WPFHelper.UIEditorConverts
{
    /// <summary>
    /// 帮助类
    /// </summary>
    public class UIEditorHelper
    {
        #region Public 方法

        /// <summary>
        /// 获取成员对应属性对象
        /// </summary>
        /// <typeparam name="T">
        /// </typeparam>
        /// <param name="memberInfo">
        /// </param>
        /// <returns>
        /// </returns>
        public static T GetCusomAttr<T>(System.Reflection.MemberInfo memberInfo) where T : Attribute
        {
            object[] attrs = memberInfo.GetCustomAttributes(typeof(T), false);
            return attrs.Length == 1 ? attrs[0] as T : null;
        }

        /// <summary>
        /// 获取对象是否只读
        /// </summary>
        /// <param name="propertyInfo">
        /// </param>
        /// <returns>
        /// </returns>
        public static bool GetReadOnly(System.Reflection.PropertyInfo propertyInfo)
        {
            System.ComponentModel.ReadOnlyAttribute attr = GetCusomAttr<System.ComponentModel.ReadOnlyAttribute>(propertyInfo);
            if (attr != null)
            {
                if (!attr.IsReadOnly)//可读状态
                {
                    //非只读,则查看是否能设置值
                    if (propertyInfo.SetMethod == null)
                        return true;//只读
                    return false;
                }
                return true;
            }

            return false;
        }

        /// <summary>
        /// 获取对象是否应该显示
        /// </summary>
        /// <param name="propertyInfo">
        /// </param>
        /// <returns>
        /// </returns>
        public static Visibility GetVisibility(System.Reflection.PropertyInfo propertyInfo)
        {
            System.ComponentModel.BrowsableAttribute visibleAttr = GetCusomAttr<System.ComponentModel.BrowsableAttribute>(propertyInfo);
            return visibleAttr != null ? visibleAttr.Browsable ? Visibility.Visible : Visibility.Collapsed : Visibility.Visible;
        }

        #endregion Public 方法
    }
}