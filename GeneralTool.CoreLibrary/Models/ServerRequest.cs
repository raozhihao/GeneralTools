using System;
using System.Collections.Generic;

namespace GeneralTool.CoreLibrary.Models
{
    /// <summary>
    /// 服务请求对象
    /// </summary>
    [Serializable]
    public class ServerRequest
    {
        #region Public 字段

        /// <summary>
        /// 参数列表
        /// </summary>
        public Dictionary<string, string> Parameters { get; set; } =
        new Dictionary<string, string>();

        #endregion Public 字段

        #region Public 属性

        /// <summary>
        /// 路由地址
        /// </summary>
        public string Url { get; set; }

        #endregion Public 属性

        #region Public 方法

        /// <summary>
        /// 获取值
        /// </summary>
        /// <param name="key">
        /// </param>
        /// <returns>
        /// </returns>
        public string GetValue(string key)
        {
            return this.Parameters[key];
        }

        /// <summary>
        /// 获取值
        /// </summary>
        /// <typeparam name="T">
        /// </typeparam>
        /// <param name="key">
        /// </param>
        /// <returns>
        /// </returns>
        public T GetValue<T>(string key)
        {
            return (T)((object)Convert.ChangeType(this.Parameters[key], typeof(T)));
        }

        #endregion Public 方法
    }
}