using System;

namespace GeneralTool.General.Attributes
{
    /// <summary>
    /// 路由特性
    /// </summary>
    public class RouteAttribute : Attribute
    {
        /// <summary>
        /// 路由地址
        /// </summary>
        public string Url
        {
            get;
            set;
        }

        /// <summary>
        /// 提示信息
        /// </summary>
        public string Explanation
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="explanation"></param>
        public RouteAttribute(string url, string explanation = "")
        {
            Url = url;
            Explanation = explanation;
        }
    }
}
