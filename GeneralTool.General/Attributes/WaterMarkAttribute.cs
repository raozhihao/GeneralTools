using System;

namespace GeneralTool.General.Attributes
{
    /// <summary>
    /// 水印提示特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter)]
    public class WaterMarkAttribute : Attribute
    {
        #region Public 构造函数

        /// <summary>
        /// </summary>
        /// <param name="waterMark">
        /// </param>
        public WaterMarkAttribute(string waterMark)
        {
            WaterMark = waterMark;
        }

        #endregion Public 构造函数

        #region Public 属性

        /// <summary>
        /// 水印
        /// </summary>
        public string WaterMark
        {
            get;
            set;
        }

        #endregion Public 属性
    }
}