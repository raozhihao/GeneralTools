using System;

namespace GeneralTool.General.Models
{
    /// <summary>
    /// 与Http服务器进行通信的请求类
    /// </summary>
    [Serializable]
    public class RequestCommand
    {
        /// <summary>
        /// 要传递的参数对象集合
        /// </summary>
        public object[] Parameters { get; set; }

        /// <summary>
        /// 要传递的方法名称
        /// </summary>
        public string MethodName { get; set; }

        /// <summary>
        /// 要使用的接口类名称
        /// </summary>
        public string ClassName { get; set; }


        /// <summary>
        /// 客户端是否已发出下线命令
        /// </summary>
        public bool Disconnect { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public RequestCommand() : this("", "", new object[] { })
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="className"></param>
        /// <param name="methodName"></param>
        /// <param name="paramters"></param>
        public RequestCommand(string className, string methodName, object[] paramters)
        {
            ClassName = className;
            MethodName = methodName;
            Parameters = paramters;
        }
    }
}
