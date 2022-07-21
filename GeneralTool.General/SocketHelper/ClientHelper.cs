﻿using GeneralTool.General.ExceptionHelper;
using GeneralTool.General.Models;
using System;
using System.Collections.Generic;

namespace GeneralTool.General.SocketHelper
{
    /// <summary>
    /// 客户端帮助类
    /// </summary>
    public class ClientHelper
    {
        private readonly ClientSocketBase clientSocket;
        private readonly SerializeHelpers serialize;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        public ClientHelper(string host = "127.0.0.1", int port = 55155)
        {
            serialize = new SerializeHelpers();
            clientSocket = new ClientSocketBase(host, port);
        }

        /// <summary>
        /// 开启连接
        /// </summary>
        public void Start()
        {
            this.clientSocket.Start();
        }


        /// <summary>
        /// 发送命令并获取返回命令
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public ResponseCommand SendCommand(RequestCommand cmd)
        {
            if (!clientSocket.IsConnected)
            {
                Start();
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

            ResponseCommand response = new ResponseCommand();
            List<byte> buffer = new List<byte>();
            try
            {
                buffer = clientSocket.Send(bytes);
            }
            catch (Exception ex)
            {
                response.Messages = ex.Message;
                response.Success = false;
            }

            try
            {
                response = serialize.Desrialize<ResponseCommand>(bytes);
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Messages = "反序列化出错:" + ex.Message;
                response.Success = false;
            }


            return response;
        }



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
    }
}
