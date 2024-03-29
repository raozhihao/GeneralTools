﻿using System;

using GeneralTool.General.Enums;
using GeneralTool.General.Extensions;
using GeneralTool.General.Interfaces;
using GeneralTool.General.Logs;
using GeneralTool.General.Models;

namespace GeneralTool.General.TaskLib
{
    /// <summary>
    /// 
    /// </summary>
    public class ServerStation : ServerStationBase
    {
        #region Private 字段

        private readonly IJsonConvert _jsonCovert;

        private readonly ILog log;

        #endregion Private 字段

        #region Public 构造函数
        /// <summary>
        /// 
        /// </summary>
        /// <param name="jsonConvert"></param>
        /// <param name="log"></param>
        public ServerStation(IJsonConvert jsonConvert = null, ILog log = null) : base(log)
        {
            if (log == null)
                log = new ConsoleLogInfo();
            if (jsonConvert == null)
                jsonConvert = new BaseJsonCovert();

            this._jsonCovert = jsonConvert;
            this.log = log;
            this.SocketServer = new SocketServer(log);
            this.SocketServer.RecDataEvent += this.SocketServer_RecDataEvent;
        }

        #endregion Public 构造函数

        #region Public 属性
        /// <summary>
        /// 
        /// </summary>
        public SocketServer SocketServer { get; set; }

        #endregion Public 属性

        #region Public 方法

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override bool Close()
        {
            this.SocketServer.Close();
            this.SocketServer.Dispose();
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        public override bool Start(string ip, int port)
        {
            this.SocketServer.IsDelMsgAsyn = false;
            this.SocketServer.IsCheckLink = true;
            this.SocketServer.IsAutoSize = true;
            this.SocketServer.IsDelMsgAsyn = true;
            this.SocketServer.IsReciverForAll = true;
            this.SocketServer.InitSocket(ip, port);
            this.SocketServer.Connect();
            return true;
        }

        #endregion Public 方法

        #region Private 方法

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        private void SocketServer_RecDataEvent(RecDataObject obj)
        {
            ServerResponse serverResponse = new ServerResponse
            {
                StateCode = RequestStateCode.OK
            };
            try
            {
                obj.DelEndString();
                ServerRequest serverRequest = null;
                try
                {
                    serverRequest = _jsonCovert.DeserializeObject<ServerRequest>(obj.StringDatas);
                    log.Debug($"获取到客户端调用:{serverRequest.Url}");
                }
                catch (Exception ex)
                {
                    this.log.Fail($"反序列化失败:{ex}");
                    serverResponse.StateCode = RequestStateCode.UrlError;
                    serverResponse.RequestSuccess = false;
                    serverResponse.ErroMsg = ex.GetInnerExceptionMessage();
                    return;
                }
                serverResponse = this.GetServerResponse(serverRequest, this._jsonCovert);
            }
            catch (Exception ex5)
            {
                log.Fail($"客户端调用服务方法发生未知错误:{ex5.Message}");
                serverResponse.StateCode = RequestStateCode.UnknowError;
                serverResponse.RequestSuccess = false;
                serverResponse.ErroMsg = ex5.GetInnerExceptionMessage();
            }
            finally
            {
                this.SocketServer.Send(_jsonCovert.SerializeObject(serverResponse), obj.Socket);
            }
        }

        #endregion Private 方法
    }
}