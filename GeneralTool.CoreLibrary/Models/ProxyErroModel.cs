using System;

namespace GeneralTool.CoreLibrary.Models
{
    /// <summary>
    /// 错误消息
    /// </summary>
    [Serializable]
    public class ProxyErroModel
    {
        #region Public 构造函数

        /// <summary>
        /// </summary>
        /// <param name="requestCommand">
        /// </param>
        /// <param name="responseCommand">
        /// </param>
        public ProxyErroModel(RequestCommand requestCommand, ResponseCommand responseCommand)
        {
            RequestCommand = requestCommand;
            ResponseCommand = responseCommand;
        }

        #endregion Public 构造函数

        #region Public 属性

        /// <summary>
        /// 请求命令(包含请求的详细信息)
        /// </summary>
        public RequestCommand RequestCommand { get; set; }

        /// <summary>
        /// 回应命令(包含返回响应的详细错误信息)
        /// </summary>
        public ResponseCommand ResponseCommand { get; set; }

        #endregion Public 属性
    }
}