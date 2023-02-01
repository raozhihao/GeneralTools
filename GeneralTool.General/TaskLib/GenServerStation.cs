using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

using GeneralTool.General.Enums;
using GeneralTool.General.ExceptionHelper;
using GeneralTool.General.Interfaces;
using GeneralTool.General.Logs;
using GeneralTool.General.Models;
using GeneralTool.General.SocketLib;
using GeneralTool.General.SocketLib.Interfaces;
using GeneralTool.General.SocketLib.Models;
using GeneralTool.General.SocketLib.Packages;

namespace GeneralTool.General.TaskLib
{
    /// <summary>
    /// 固定包头长度服务类
    /// </summary>
    public class GenServerStation<T> : ServerStationBase where T : ReceiveState, new()
    {
        private readonly SocketServer<T> server;
        /// <summary>
        /// 返回消息
        /// </summary>
        public event EventHandler<ReceiveArg> ReceiveEvent;
        /// <summary>
        /// 返回消息
        /// </summary>
        public event EventHandler<GenRequestRoute> RequestInfoEvent;

        /// <summary>
        /// 处理类型
        /// </summary>
        public GenExecuteType ExecuteType { get; set; }
        /// <summary>
        /// 序列化类
        /// </summary>
        public IJsonConvert JsonConvert { get; set; }

        /// <inheritdoc/>
        public GenServerStation(IJsonConvert jsonConvert, ILog log, Func<IPackage<T>> package) : base(log)
        {
            if (log == null)
                log = new ConsoleLogInfo();
            if (jsonConvert == null)
                jsonConvert = new BaseJsonCovert();

            this.Log = log;
            this.JsonConvert = jsonConvert;
            this.server = new SocketServer<T>(log);
            if (package == null)
            {
                package = new Func<IPackage<T>>(() => new NoPackage<T>());
            }
            this.server.Package = package;
            this.server.ClientConnctedEvent += Server_ClientConnctedEvent;
            this.server.DisconnectEvent += Server_DisconnectEvent;
            this.server.ErrorEvent += Server_ErrorEvent;
            this.server.ReceiveEvent += Server_ReceiveEvent;
        }

        private void Server_ReceiveEvent(object sender, ReceiveArg e)
        {
            if (this.ExecuteType == GenExecuteType.Buffer)
            {
                this.Log.Debug("直接将buffer交由开发人员处理");
                this.ReceiveEvent?.Invoke(sender, e);
            }
            else
            {
                ServerResponse serverResponse = new ServerResponse
                {
                    StateCode = RequestStateCode.OK
                };

                var receiveMsg = Encoding.UTF8.GetString(e.PackBuffer.ToArray());
                ServerRequest serverRequest = null;
                try
                {
                    serverRequest = this.JsonConvert.DeserializeObject<ServerRequest>(receiveMsg);
                    this.Log?.Debug($"获取到客户端调用:{serverRequest.Url}");
                }
                catch (Exception ex)
                {
                    this.Log?.Fail($"客户端无法反序列化:{ex},传入为:{receiveMsg}");
                    serverResponse.StateCode = RequestStateCode.UrlError;
                    serverResponse.RequestSuccess = false;
                    serverResponse.ErroMsg = ex.GetInnerExceptionMessage();
                    this.server.Send(this.JsonConvert.SerializeObject(serverResponse), e.Client);
                    return;
                }

                if (this.RequestInfoEvent != null && this.ExecuteType == GenExecuteType.RequestInfo)
                {
                    //给定必要信息后由调用人员自行处理
                    try
                    {
                        var route = this.RequestRoute[serverRequest.Url];
                        this.Log.Debug($"由开发人员开始执行方法:{serverRequest.Url}");
                        this.RequestInfoEvent?.Invoke(sender, new GenRequestRoute() { AddressItem = route, SendToClinet = this.SendToClient, Client = e.Client, ServerRequest = serverRequest });
                    }
                    catch (Exception ex)
                    {
                        this.Log?.Fail($"客户端调用服务方法发生未知错误:{ex}");
                        serverResponse.StateCode = RequestStateCode.UnknowError;
                        serverResponse.RequestSuccess = false;
                        serverResponse.ErroMsg = ex.GetInnerExceptionMessage();
                        this.server.Send(this.JsonConvert.SerializeObject(serverResponse), e.Client);
                    }
                    return;
                }
                else if (this.ExecuteType == GenExecuteType.Auto)
                {
                    //由本类自行处理
                    try
                    {
                        this.Log.Debug($"由底层开始执行方法:{serverRequest.Url}");
                        serverResponse = this.GetServerResponse(serverRequest, this.JsonConvert);
                    }
                    catch (Exception ex6)
                    {
                        this.Log?.Fail($"客户端调用服务方法发生未知错误:{ex6}");
                        serverResponse.StateCode = RequestStateCode.UnknowError;
                        serverResponse.RequestSuccess = false;
                        serverResponse.ErroMsg = ex6.GetInnerExceptionMessage();
                    }
                    finally
                    {
                        this.Log.Debug($"底层执行方法:{serverRequest.Url} 完成,准备发送给客户端");
                        string result = "";
                        try
                        {
                            result = this.JsonConvert.SerializeObject(serverResponse);
                        }
                        catch (Exception ex)
                        {
                            result = ex.GetInnerExceptionMessage();
                            serverResponse = new ServerResponse()
                            {
                                RequestSuccess = false,
                                ErroMsg = "方法调用成功,但返回时出错:" + result
                            };

                            result = this.JsonConvert.SerializeObject(serverResponse);
                        }
                        this.server.Send(result, e.Client);
                    }
                }

            }

        }

        private void SendToClient(byte[] buffer, Socket client)
        {
            this.server.Send(buffer, client);
        }

        private void Server_ErrorEvent(object sender, SocketErrorArg e)
        {
            try
            {
                this.Log?.Fail($"{e.Client.RemoteEndPoint} Error : {e.Exception}");
            }
            catch
            {

            }
        }

        private void Server_DisconnectEvent(object sender, SocketErrorArg e)
        {
            try
            {
                this.Log?.Debug($"{e.Client.RemoteEndPoint} Disconnect");
            }
            catch
            {

            }
        }

        private void Server_ClientConnctedEvent(object sender, SocketArg e)
        {
            try
            {
                this.Log?.Debug($"{e.Client.RemoteEndPoint} Connect");
            }
            catch
            {

            }
        }


        /// <inheritdoc/>
        public override bool Close()
        {
            try
            {
                this.server?.Close();
            }
            catch
            {

            }
            return true;
        }

        /// <inheritdoc/>
        public override bool Start(string ip, int port)
        {
            try
            {
                this.server.Startup(IPAddress.Parse(ip), port);
                return true;
            }
            catch (Exception ex)
            {
                this.Log?.Fail($"Server startup error : {ex}");
                return false;
            }

        }
    }
}
