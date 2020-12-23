using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneralTool.General.Models
{
    /// <summary>
    /// 错误消息
    /// </summary>
    public class ProxyErroModel
    {
        /// <summary>
        /// 请求命令(包含请求的详细信息)
        /// </summary>
        public RequestCommand RequestCommand { get; set; }

        /// <summary>
        /// 回应命令(包含返回响应的详细错误信息)
        /// </summary>
        public ResponseCommand ResponseCommand { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="requestCommand"></param>
        /// <param name="responseCommand"></param>
        public ProxyErroModel(RequestCommand requestCommand, ResponseCommand responseCommand)
        {
            this.RequestCommand = requestCommand;
            this.ResponseCommand = responseCommand;
        }
    }
}
