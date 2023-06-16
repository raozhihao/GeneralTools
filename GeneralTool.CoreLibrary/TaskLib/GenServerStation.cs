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
        private readonly SocketServer<T> server;
        /// <summary>
        /// 返回消息,该事件需要设置 <see cref="ExecuteType"/> 为Buffer
        /// </summary>
        public event EventHandler<ReceiveArg> ReceiveEvent;
        /// <summary>
        /// 返回消息,该事件需要设置 <see cref="ExecuteType"/> RequestInfo
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
            if (ExecuteType == GenExecuteType.Buffer)
            {
                Log.Debug("直接将buffer交由开发人员处理");
                ReceiveEvent?.Invoke(sender, e);
            }
            else
            {
                ServerResponse serverResponse = new ServerResponse
                {
                    StateCode = RequestStateCode.OK
                };

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
                    serverResponse.StateCode = RequestStateCode.UrlError;
                    serverResponse.RequestSuccess = false;
                    serverResponse.ErroMsg = ex.GetInnerExceptionMessage();
                    _ = server.Send(JsonConvert.SerializeObject(serverResponse), e.Client);
                    return;
                }

                if (RequestInfoEvent != null && ExecuteType == GenExecuteType.RequestInfo)
                {
                    //给定必要信息后由调用人员自行处理
                    try
                    {
                        RequestAddressItem route = RequestRoute[serverRequest.Url];
                        Log.Debug($"由开发人员开始执行方法:{serverRequest.Url}");
                        RequestInfoEvent?.Invoke(sender, new GenRequestRoute() { AddressItem = route, SendToClinet = SendToClient, Client = e.Client, ServerRequest = serverRequest });
                    }
                    catch (Exception ex)
                    {
                        Log?.Fail($"客户端调用服务方法发生未知错误:{ex}");
                        serverResponse.StateCode = RequestStateCode.UnknowError;
                        serverResponse.RequestSuccess = false;
                        serverResponse.ErroMsg = ex.GetInnerExceptionMessage();
                        _ = server.Send(JsonConvert.SerializeObject(serverResponse), e.Client);
                    }
                    return;
                }
                else if (ExecuteType == GenExecuteType.Auto)
                {
                    //由本类自行处理
                    try
                    {
                        Log.Debug($"由底层开始执行方法:{serverRequest.Url}");
                        serverResponse = GetServerResponse(serverRequest, JsonConvert);
                    }
                    catch (Exception ex6)
                    {
                        Log?.Fail($"客户端调用服务方法发生未知错误:{ex6}");
                        serverResponse.StateCode = RequestStateCode.UnknowError;
                        serverResponse.RequestSuccess = false;
                        serverResponse.ErroMsg = ex6.GetInnerExceptionMessage();
                    }
                    finally
                    {
                        Log.Debug($"底层执行方法:{serverRequest.Url} 完成,准备发送给客户端");
                        string result = "";
                        try
                        {
                            result = JsonConvert.SerializeObject(serverResponse);
                        }
                        catch (Exception ex)
                        {
                            result = ex.GetInnerExceptionMessage();
                            serverResponse = new ServerResponse()
                            {
                                RequestSuccess = false,
                                ErroMsg = "方法调用成功,但返回时出错:" + result
                            };

                            result = JsonConvert.SerializeObject(serverResponse);
                        }
                        _ = server.Send(result, e.Client);
                    }
                }

            }

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
            try
            {
                server.Startup(IPAddress.Parse(ip), port);
                return true;
            }
            catch (Exception ex)
            {
                Log?.Fail($"Server startup error : {ex}");
                return false;
            }

        }
    }
}
