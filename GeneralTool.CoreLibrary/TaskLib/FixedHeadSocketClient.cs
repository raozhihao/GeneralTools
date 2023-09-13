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
using GeneralTool.CoreLibrary.SocketLib.Interfaces;
using GeneralTool.CoreLibrary.SocketLib.Models;
using GeneralTool.CoreLibrary.SocketLib.Packages;
using GeneralTool.CoreLibrary.WPFHelper;

namespace GeneralTool.CoreLibrary.TaskLib
{
    //FixedHeadSocketClient

    public class FixedHeadSocketClient : TaskSocketClient<FixedHeadRecevieState>
    {
        public FixedHeadSocketClient(ILog log, IJsonConvert jsonConvert) : base(log, jsonConvert, () => new FixedHeadPackage())
        {
        }
    }

    /// <summary>
    /// 固定包头客户端
    /// </summary>
    public class TaskSocketClient<T> : IDisposable where T : ReceiveState, new()
    {
        /// <summary>
        /// 
        /// 
        /// </summary>
        public ILog Log { get; set; }

        /// <summary>
        /// 超时时间
        /// </summary>
        public int ReadTimeOut { get; set; } = 180000;

        private readonly StringConverter converter = new StringConverter();

        /// <summary>
        /// 
        /// </summary>
        public SocketClient<T> Client { get; private set; }

        private readonly AutoResetEvent autoReset = new AutoResetEvent(false);

        private readonly IJsonConvert jsonConvert;

        private string result;
        private bool isCancel;
        private bool error;
        private bool isDisconnect;
        private bool disposedValue;

        /// <summary>
        /// 
        /// </summary>
        public TaskSocketClient(ILog log, IJsonConvert jsonConvert, Func<IPackage<T>> package)
        {
            if (log == null) log = new ConsoleLogInfo();
            Log = log;

            if (jsonConvert == null) jsonConvert = new BaseJsonCovert();
            this.jsonConvert = jsonConvert;

            Client = new SocketClient<T>(log)
            {
                PackageFunc = package
            };

            Client.ReceiveEvent += Client_ReceiveEvent;
            Client.DisconnectEvent += Client_DisconnectEvent;
            Client.ErrorEvent += Client_ErrorEvent;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        public void Startup(string ip, int port)
        {
            Client.Startup(IPAddress.Parse(ip), port);
        }

        private void Client_ErrorEvent(object sender, SocketErrorArg e)
        {
            error = true;
            result = e.Exception?.GetInnerExceptionMessage();
            _ = autoReset.Set();
        }

        private void Client_DisconnectEvent(object sender, SocketErrorArg e)
        {
            isDisconnect = true;
            result = "已断开连接";
            _ = autoReset.Set();
        }

        private void Client_ReceiveEvent(object sender, ReceiveArg e)
        {
            result = Encoding.UTF8.GetString(e.PackBuffer.ToArray());
            _ = autoReset.Set();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TOut"></typeparam>
        /// <param name="url"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public TOut SendResultObject<TOut>(string url, Dictionary<string, string> parameters, CancellationToken token) => (TOut)SendResultObject(url, parameters, token);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public object SendResultObject(string url, Dictionary<string, string> parameters, CancellationToken token)
        {
            ServerRequest request = new ServerRequest()
            {
                Url = url,
                Parameters = parameters
            };
            return SendResultObject(request, token);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public ServerResponse Send(string url, Dictionary<string, string> parameters, CancellationToken token)
        {
            ServerRequest request = new ServerRequest()
            {
                Url = url,
                Parameters = parameters
            };
            return Send(request, token);
        }

        /// <summary>
        /// 发送并返回值
        /// </summary>
        public TOut SendResultObject<TOut>(ServerRequest request, CancellationToken token) => (TOut)SendResultObject(request, token);

        /// <summary>
        /// 发送并返回值
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public object SendResultObject(ServerRequest request, CancellationToken token)
        {
            ServerResponse reponse = Send(request, token);
            return !reponse.RequestSuccess
                ? throw new Exception(reponse.ErroMsg)
                : converter.ConvertSimpleType(reponse.ResultString, Type.GetType(reponse.ReturnTypeString));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ServerResponse Send(ServerRequest request, CancellationToken token)
        {

            string jsonRequest = jsonConvert.SerializeObject(request);
            _ = Client.Send(jsonRequest);
            bool re = false;

            if (ReadTimeOut <= 0)
            {
                re = autoReset.WaitOne();
            }
            else
            {
                DateTime time = DateTime.Now;
                TimeSpan timeOutSpan = TimeSpan.FromMilliseconds(ReadTimeOut);
                do
                {
                    if (DateTime.Now - time >= timeOutSpan)
                        break;

                    re = autoReset.WaitOne(10);
                    if (re)
                    {
                        break;
                    }


                } while (!token.IsCancellationRequested);
            }


            if (token.IsCancellationRequested)
                throw new Exception("已取消任务");

            if (!re)
                throw new Exception("连接已超时");
            if (isCancel)
            {
                isCancel = false;
                throw new TaskCanceledException();
            }

            if (isDisconnect)
            {
                isDisconnect = false;
                throw new Exception("连接被断开");
            }

            if (error)
            {
                error = false;
                throw new Exception($"出现错误:[{result}]");
            }

            return jsonConvert.DeserializeObject<ServerResponse>(result);

        }

        /// <summary>
        /// 取消任务
        /// </summary>
        public void Cancel()
        {
            isCancel = true;
            _ = autoReset.Set();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: 释放托管状态(托管对象)
                    if (Client != null)
                    {
                        Client.ReceiveEvent -= Client_ReceiveEvent;
                        Client.DisconnectEvent -= Client_DisconnectEvent;
                        Client.ErrorEvent -= Client_ErrorEvent;
                        Client.Close();
                        Client = null;
                    }
                }

                // TODO: 释放未托管的资源(未托管的对象)并重写终结器
                // TODO: 将大型字段设置为 null
                disposedValue = true;
            }
        }

        // // TODO: 仅当“Dispose(bool disposing)”拥有用于释放未托管资源的代码时才替代终结器
        // ~FixedHeadSocketClient()
        // {
        //     // 不要更改此代码。请将清理代码放入“Dispose(bool disposing)”方法中
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // 不要更改此代码。请将清理代码放入“Dispose(bool disposing)”方法中
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
