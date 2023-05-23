using System;

using GeneralTool.CoreLibrary.Interfaces;

namespace GeneralTool.CoreLibrary.Attributes
{
    /// <summary>
    /// UI编辑器特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class UIEditorAttribute : Attribute
    {
        #region Public 构造函数

        /// <summary>
        /// </summary>
        /// <param name="convert">
        /// UI编辑器类型
        /// </param>
        public UIEditorAttribute(Type convert)
        {
            this.Convert = convert;
        }

        #endregion Public 构造函数

        #region Public 属性

        /// <summary>
        /// UI编辑器类型
        /// </summary>
        public Type Convert { get; set; }

        #endregion Public 属性

        #region Public 方法

        /// <summary>
        /// 获取UI编辑器
        /// </summary>
        /// <returns>
        /// </returns>
        public IUIEditorConvert GetConvert()
        {
            return Activator.CreateInstance(this.Convert) as IUIEditorConvert;
        }

        #endregion Public 方法
    }
}