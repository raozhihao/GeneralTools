using System;
using System.Collections.Generic;

namespace GeneralTool.General.Models
{
    /// <summary>
    /// 服务请求对象
    /// </summary>
    [Serializable]
    public class ServerRequest
    {
        /// <summary>
        /// 路由地址
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 获取值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetValue(string key)
        {
            return this.Paramters[key];
        }

        /// <summary>
        /// 获取值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T GetValue<T>(string key)
        {
            return (T)((object)Convert.ChangeType(this.Paramters[key], typeof(T)));
        }

        /// <summary>
        /// 参数列表
        /// </summary>
        public Dictionary<string, string> Paramters = new Dictionary<string, string>();
    }
}
