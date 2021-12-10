using System;
using System.IO;
using System.Net;
using System.Text;

using GeneralTool.General.Models;

namespace GeneralTool.General.TaskLib
{
    /// <summary>
    /// 请示事件参数
    /// </summary>
    public class RequestInfo : EventArgs
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="serverRequest"></param>
        /// <param name="response"></param>
        /// <param name="requestRoute"></param>
        public RequestInfo(ServerRequest serverRequest, HttpListenerResponse response, RequestAddressItem requestRoute)
        {
            ServerRequest = serverRequest;
            Response = response;
            RequestRoute = requestRoute;
        }
        /// <summary>
        /// 
        /// </summary>
        public RequestInfo()
        {

        }
        /// <summary>
        /// 请求获取到的数据
        /// </summary>
        public ServerRequest ServerRequest { get; set; }

        /// <summary>
        /// 返回对象
        /// </summary>
        public HttpListenerResponse Response { get; set; }

        /// <summary>
        /// 当前请求的路由集合
        /// </summary>
        public RequestAddressItem RequestRoute { get; set; }

        /// <summary>
        /// 写入响应内容
        /// </summary>
        /// <param name="responseString"></param>
        public void WriteResponse(string responseString)
        {
            byte[] buff = Encoding.UTF8.GetBytes(responseString);

            // 输出回应内容
            try
            {
                //必须设置响应中正文的字节数，否则在关闭输出流时将会出错
                this.Response.ContentLength64 = buff.Length;
                Stream output = this.Response.OutputStream;

                output.Write(buff, 0, buff.Length);
                output.Close();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                this.Response.Close();
            }
        }
    }

    /// <summary>
    /// Context
    /// </summary>
    public class ContextRequest : EventArgs
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public ContextRequest(HttpListenerContext context)
        {
            Context = context;
        }

        /// <summary>
        /// 请示和响应对象的访问类
        /// </summary>
        public HttpListenerContext Context { get; }
    }
}
