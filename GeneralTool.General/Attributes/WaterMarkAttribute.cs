using System;

namespace GeneralTool.General.Attributes
{
    /// <summary>
    /// 水印提示特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter)]
    public class WaterMarkAttribute : Attribute
    {
        /// <summary>
        /// 水印
        /// </summary>
        public string WaterMark
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="waterMark"></param>
        public WaterMarkAttribute(string waterMark)
        {
            WaterMark = waterMark;
        }
    }
}
