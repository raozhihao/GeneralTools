using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

using GeneralTool.CoreLibrary.Enums;
using GeneralTool.CoreLibrary.Extensions;
using GeneralTool.CoreLibrary.Interfaces;
using GeneralTool.CoreLibrary.Logs;
using GeneralTool.CoreLibrary.Models;
using GeneralTool.CoreLibrary.SocketLib;
using GeneralTool.CoreLibrary.SocketLib.Interfaces;
using GeneralTool.CoreLibrary.SocketLib.Models;
using GeneralTool.CoreLibrary.SocketLib.Packages;

namespace GeneralTool.CoreLibrary.TaskLib
{
    /// <summary>
    /// 固定包头长度服务类
    /// </summary>
    public class GenServerStation<T> : ServerStationBase where T : ReceiveState, new()
    {
        public SocketServer<T> server { get; private set; }
        /// <summary>
        /// 消息得到返回时的引用发事件，此为第一事件
        /// </summary>
        public event EventHandler<ReceiveArg> BufferReceiveEvent;
        /// <summary>
        /// 消息得到的事件，此为第三事件
        /// </summary>
        public event EventHandler<GenRequestRoute> RequestInfoEvent;

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

            Log = log;
            JsonConvert = jsonConvert;
            server = new SocketServer<T>(log);
            if (package == null)
            {
                package = new Func<IPackage<T>>(() => new NoPackage<T>());
            }
            server.PackageFunc = package;
            server.ClientConnctedEvent += Server_ClientConnctedEvent;
            server.DisconnectEvent += Server_DisconnectEvent;
            server.ErrorEvent += Server_ErrorEvent;
            server.ReceiveEvent += Server_ReceiveEvent;
        }

        private void Server_ReceiveEvent(object sender, ReceiveArg e)
        {
            var reponse = new ServerResponse()
            {
                RequestSuccess = true,
                StateCode = RequestStateCode.OK
            };
            try
            {
                BufferReceiveEvent?.Invoke(sender, e);
            }
            catch (Exception ex)
            {
                reponse.RequestSuccess = false;
                reponse.ErroMsg = ex.GetInnerExceptionMessage();
                this.SendReponse(reponse, e.Client);
                return;
            }
            if (e.Handled)
            {
                Log.Debug("直接将buffer交由开发人员处理");
                return;
            }



            string receiveMsg = Encoding.UTF8.GetString(e.PackBuffer.ToArray());
            ServerRequest serverRequest = null;
            try
            {
                serverRequest = JsonConvert.DeserializeObject<ServerRequest>(receiveMsg);
                Log?.Debug($"获取到客户端调用:{serverRequest.Url}");
            }
            catch (Exception ex)
            {
                Log?.Fail($"客户端无法反序列化:{ex},传入为:{receiveMsg}");
                reponse.StateCode = RequestStateCode.UrlError;
                reponse.RequestSuccess = false;
                reponse.ErroMsg = ex.GetInnerExceptionMessage();
                this.SendReponse(reponse, e.Client);
                return;
            }


            try
            {
                reponse = this.OnRequestEvent(serverRequest);
            }
            catch (Exception ex)
            {
                reponse.RequestSuccess = false;
                reponse.ErroMsg = ex.GetInnerExceptionMessage();
                this.SendReponse(reponse, e.Client);
                return;
            }

            if (reponse == null)
            {
                if (RequestInfoEvent != null)
                {
                    //给定必要信息后由调用人员自行处理
                    try
                    {
                        var route = GetRequestItem(serverRequest); //RequestRoute[serverRequest.Url];
                        Log.Debug($"由开发人员开始执行方法:{serverRequest.Url}");
                        RequestInfoEvent?.Invoke(sender, new GenRequestRoute() { AddressItem = route, SendToClinet = SendToClient, Client = e.Client, ServerRequest = serverRequest });
                    }
                    catch (Exception ex)
                    {
                        Log?.Fail($"客户端调用服务方法发生未知错误:{ex}");
                        reponse.StateCode = RequestStateCode.UnknowError;
                        reponse.RequestSuccess = false;
                        reponse.ErroMsg = ex.GetInnerExceptionMessage();
                        this.SendReponse(reponse, e.Client);
                    }
                    return;
                }
                else
                {
                    //由本类自行处理
                    try
                    {
                        Log.Debug($"由底层开始执行方法:{serverRequest.Url}");
                        reponse = GetServerResponse(serverRequest, JsonConvert);
                    }
                    catch (Exception ex6)
                    {
                        Log?.Fail($"客户端调用服务方法发生未知错误:{ex6}");
                        reponse.StateCode = RequestStateCode.UnknowError;
                        reponse.RequestSuccess = false;
                        reponse.ErroMsg = ex6.GetInnerExceptionMessage();
                        this.SendReponse(reponse, e.Client);
                        return;
                    }
                    Log.Debug($"底层执行方法:{serverRequest.Url} 完成,准备发送给客户端");
                    this.SendReponse(reponse, e.Client);
                }
            }
            else
            {
                this.SendReponse(reponse, e.Client);
            }
        }

        private void SendReponse(ServerResponse reponse, Socket client)
        {
            string result;
            try
            {
                result = JsonConvert.SerializeObject(reponse);
            }
            catch (Exception ex2)
            {
                result = ex2.GetInnerExceptionMessage();
                reponse.RequestSuccess = false;
                reponse.ErroMsg = "方法调用成功,但在序列化时出错:" + result;

                result = JsonConvert.SerializeObject(reponse);
            }
            _ = server.Send(result, client);
        }

        private void SendToClient(byte[] buffer, Socket client)
        {
            _ = server.Send(buffer, client);
        }

        private void Server_ErrorEvent(object sender, SocketErrorArg e)
        {
            try
            {
                Log?.Fail($"{e.Client.RemoteEndPoint} Error : {e.Exception}");
            }
            catch
            {

            }
        }

        private void Server_DisconnectEvent(object sender, SocketErrorArg e)
        {
            try
            {
                Log?.Debug($"{e.Client.RemoteEndPoint} Disconnect");
            }
            catch
            {

            }
        }

        private void Server_ClientConnctedEvent(object sender, SocketArg e)
        {
            try
            {
                Log?.Debug($"{e.Client.RemoteEndPoint} Connect");
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
                server?.Close();
            }
            catch
            {

            }
            try
            {
                server.Dispose();
            }
            catch (Exception)
            {

            }
            return true;
        }

        /// <inheritdoc/>
        public override bool Start(string ip, int port)
        {
            server.Startup(IPAddress.Parse(ip), port);
            return true;

        }
    }
}
