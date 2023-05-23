using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using GeneralTool.CoreLibrary.Extensions;
using GeneralTool.CoreLibrary.Interfaces;
using GeneralTool.CoreLibrary.Logs;
using GeneralTool.CoreLibrary.Models;
using GeneralTool.CoreLibrary.SocketLib;
using GeneralTool.CoreLibrary.SocketLib.Models;
using GeneralTool.CoreLibrary.WPFHelper;

namespace GeneralTool.CoreLibrary.TaskLib
{
    /// <summary>
    /// 固定包头客户端
    /// </summary>
    public class FixedHeadSocketClient : IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        public ILog Log { get; set; }

        private readonly StringConverter converter = new StringConverter();

        /// <summary>
        /// 
        /// </summary>
        public SocketClient<FixedHeadRecevieState> Client { get; private set; }

        private readonly AutoResetEvent autoReset = new AutoResetEvent(false);

        private readonly IJsonConvert jsonConvert;

        private string result;
        private bool isCancel;
        private bool error;
        private bool isDisconnect;
        /// <summary>
        /// 
        /// </summary>
        public FixedHeadSocketClient(ILog log = null, IJsonConvert jsonConvert = null)
        {
            if (log == null) log = new ConsoleLogInfo();
            this.Log = log;

            if (jsonConvert == null) jsonConvert = new BaseJsonCovert();
            this.jsonConvert = jsonConvert;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        public void Startup(string ip, int port)
        {
            this.Client = SimpleClientBuilder.CreateFixedCommandSubPack(this.Log);
            this.Client.ReceiveEvent += Client_ReceiveEvent;
            this.Client.DisconnectEvent += Client_DisconnectEvent;
            this.Client.ErrorEvent += Client_ErrorEvent;
            this.Client.Startup(IPAddress.Parse(ip), port);
        }

        private void Client_ErrorEvent(object sender, SocketErrorArg e)
        {
            this.error = true;
            result = e.Exception?.GetInnerExceptionMessage();
            this.autoReset.Set();
        }

        private void Client_DisconnectEvent(object sender, SocketErrorArg e)
        {
            this.isDisconnect = true;
            result = "已断开连接";
            this.autoReset.Set();
        }

        private void Client_ReceiveEvent(object sender, ReceiveArg e)
        {
            this.result = Encoding.UTF8.GetString(e.PackBuffer.ToArray());
            this.autoReset.Set();
        }



        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public T SendResultObject<T>(string url, Dictionary<string, string> parameters) => (T)this.SendResultObject(url, parameters);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public object SendResultObject(string url, Dictionary<string, string> parameters)
        {
            var request = new ServerRequest()
            {
                Url = url,
                Parameters = parameters
            };
            return this.SendResultObject(request);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public ServerResponse Send(string url, Dictionary<string, string> parameters)
        {
            var request = new ServerRequest()
            {
                Url = url,
                Parameters = parameters
            };
            return this.Send(request);
        }

        /// <summary>
        /// 发送并返回值
        /// </summary>
        public T SendResultObject<T>(ServerRequest request) => (T)this.SendResultObject(request);

        /// <summary>
        /// 发送并返回值
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public object SendResultObject(ServerRequest request)
        {
            var reponse = this.Send(request);
            if (!reponse.RequestSuccess)
                throw new Exception(reponse.ErroMsg);


            return this.converter.ConvertSimpleType(reponse.ResultString, Type.GetType(reponse.ReturnTypeString));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ServerResponse Send(ServerRequest request)
        {
            var jsonRequest = this.jsonConvert.SerializeObject(request);
            this.Client.Send(jsonRequest);
            this.autoReset.WaitOne();
            if (this.isCancel)
            {
                this.isCancel = false;
                throw new TaskCanceledException();
            }


            if (this.isDisconnect)
            {
                this.isDisconnect = false;
                throw new Exception("连接被断开");
            }

            if (this.error)
            {
                this.error = false;
                throw new Exception($"出现错误:[{this.result}]");
            }

            return this.jsonConvert.DeserializeObject<ServerResponse>(this.result);
        }

        /// <summary>
        /// 取消任务
        /// </summary>
        public void Cancel()
        {
            this.isCancel = true;
            this.autoReset.Set();
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            if (this.Client != null)
            {
                this.Client.ReceiveEvent -= Client_ReceiveEvent;
                this.Client.DisconnectEvent -= Client_DisconnectEvent;
                this.Client.ErrorEvent -= Client_ErrorEvent;
                this.Client.Close();
            }
        }
    }
}
