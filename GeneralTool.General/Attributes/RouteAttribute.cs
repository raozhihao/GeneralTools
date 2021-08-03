using System;

namespace GeneralTool.General.Attributes
{
    /// <summary>
    /// 路由特性
    /// </summary>
    public class RouteAttribute : Attribute
    {
        #region Public 构造函数

        /// <summary>
        /// </summary>
        /// <param name="url">
        /// </param>
        /// <param name="explanation">
        /// </param>
        public RouteAttribute(string url, string explanation = "")
        {
            Url = url;
            Explanation = explanation;
        }

        #endregion Public 构造函数

        #region Public 属性

        /// <summary>
        /// 提示信息
        /// </summary>
        public string Explanation
        {
            get;
            set;
        }

        /// <summary>
        /// 路由地址
        /// </summary>
        public string Url
        {
            get;
            set;
        }

        #endregion Public 属性
    }
}