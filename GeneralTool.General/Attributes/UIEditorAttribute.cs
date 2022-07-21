using GeneralTool.General.Interfaces;
using System;

namespace GeneralTool.General.Attributes
{
    /// <summary>
    /// UI编辑器特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class UIEditorAttribute : Attribute
    {
        /// <summary>
        /// UI编辑器类型
        /// </summary>
        public Type Convert { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="convert">UI编辑器类型</param>
        public UIEditorAttribute(Type convert)
        {
            this.Convert = convert;
        }

        /// <summary>
        /// 获取UI编辑器
        /// </summary>
        /// <returns></returns>
        public IUIEditorConvert GetConvert()
        {
            return Activator.CreateInstance(this.Convert) as IUIEditorConvert;
        }
    }
}
