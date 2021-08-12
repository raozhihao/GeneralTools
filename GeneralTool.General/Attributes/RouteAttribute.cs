using GeneralTool.General.NetHelper;
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
        /// <param name="method"></param>
        public RouteAttribute(string url, string explanation = "", HttpMethod method = HttpMethod.GET)
        {
            Url = url;
            Explanation = explanation;
            this.Method = method;
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

        /// <summary>
        /// 请示的Http方法
        /// </summary>
        public HttpMethod Method { get; set; }

        #endregion Public 属性
    }
}