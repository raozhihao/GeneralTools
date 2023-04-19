using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;

using GeneralTool.General.ExceptionHelper;
using GeneralTool.General.Extensions;
using GeneralTool.General.Models;
using GeneralTool.General.SocketLib;
using GeneralTool.General.SocketLib.Models;

namespace GeneralTool.General.SocketHelper
{
    /// <summary>
    /// 客户端帮助类
    /// </summary>
    public class ClientHelper
    {
        #region Private 字段

        private readonly SocketClient<FixedHeadRecevieState> clientSocket;
        private readonly SerializeHelpers serialize;
        private readonly string host;
        private readonly int port;
        private readonly AutoResetEvent autoReset = new AutoResetEvent(false);
        private readonly ResponseCommand response = new ResponseCommand();

        #endregion Private 字段

        #region Public 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="host">
        /// </param>
        /// <param name="port">
        /// </param>
        public ClientHelper(string host = "127.0.0.1", int port = 55155)
        {
            serialize = new SerializeHelpers();
            clientSocket = SimpleClientBuilder.CreateFixedCommandSubPack(null);
            clientSocket.DisconnectEvent += ClientSocket_DisconnectEvent;
            clientSocket.ErrorEvent += ClientSocket_ErrorEvent;
            clientSocket.ReceiveEvent += ClientSocket_ReceiveEvent;
            this.host = host;
            this.port = port;
        }

        private void ClientSocket_ReceiveEvent(object sender, ReceiveArg e)
        {
            var buffer = e.PackBuffer.ToArray();
            this.response.Success = true;
            this.response.ResultObject = this.serialize.Desrialize<ResponseCommand>(buffer);
            this.autoReset.Set();
        }

        private void ClientSocket_ErrorEvent(object sender, SocketErrorArg e)
        {
            this.response.Success = false;
            this.response.ResultObject = null;
            this.response.Messages = e.Exception.GetInnerExceptionMessage();
            this.autoReset.Set();
        }

        private void ClientSocket_DisconnectEvent(object sender, SocketErrorArg e)
        {
            this.response.Success = false;
            this.response.ResultObject = null;
            this.response.Messages = e.Exception?.GetInnerExceptionMessage();
            this.autoReset.Set();
        }

        #endregion Public 构造函数

        #region Public 方法

        /// <summary>
        /// 关闭连接
        /// </summary>
        public void Close()
        {
            System.Diagnostics.Trace.WriteLine("调用客户端Close");
            if (clientSocket == null)
            {
                return;
            }
            clientSocket.Close();
        }

        /// <summary>
        /// 发送命令并获取返回命令
        /// </summary>
        /// <param name="cmd">
        /// </param>
        /// <returns>
        /// </returns>
        public ResponseCommand SendCommand(RequestCommand cmd)
        {
            if (!clientSocket.IsConnected)
            {
                try
                {
                    Start();
                }
                catch (Exception ex)
                {
                    return new ResponseCommand(false, ex.GetInnerExceptionMessage(), null);
                }
            }

            byte[] bytes;
            try
            {
                bytes = serialize.Serialize(cmd);
            }
            catch (Exception ex)
            {
                throw new SerializeException("序列化出错", ex);
            }


            List<byte> buffer = new List<byte>();
            try
            {
                var re = clientSocket.Send(bytes, clientSocket.Socket);
                if (!re)
                {
                    response.Messages = "发送远程数据不成功";
                    response.Success = false;
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.Messages = ex.GetInnerExceptionMessage();
                response.Success = false;
            }

            this.autoReset.WaitOne();
            bytes = null;


            return response;
        }

        /// <summary>
        /// 开启连接
        /// </summary>
        public void Start()
        {
            this.clientSocket.Startup(IPAddress.Parse(this.host), this.port);
            this.autoReset.Reset();
        }

        #endregion Public 方法
    }
}