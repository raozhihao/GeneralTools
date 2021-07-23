using GeneralTool.General.Enums;
using GeneralTool.General.Interfaces;
using GeneralTool.General.Logs;
using GeneralTool.General.Models;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace GeneralTool.General.TaskLib
{
    internal class ServerStation : ServerStationBase
    {
        private readonly IJsonConvert _jsonCovert;
      

        public SocketServer SocketServer { get; set; }


        private readonly ILog log;
        public ServerStation(IJsonConvert jsonConvert = null, ILog log = null):base(log)
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


        // Token: 0x0600000C RID: 12 RVA: 0x00002278 File Offset: 0x00000478
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
                    this.log.Fail($"客户端调用参数不正确:{ex.Message}");
                    serverResponse.StateCode = RequestStateCode.UrlError;
                    serverResponse.RequestSuccess = false;
                    serverResponse.ErroMsg = ex.Message;
                    return;
                }
                try
                {
                    serverResponse.RequestUrl = serverRequest.Url;
                    if (!this.RequestRoute.ContainsKey(serverRequest.Url))
                    {
                        serverResponse.StateCode = RequestStateCode.UrlError;
                        serverResponse.RequestSuccess = false;
                        serverResponse.ErroMsg = "未找到对应的接口";
                    }
                    else
                    {
                        RequestAddressItem requestAddressItem = this.RequestRoute[serverRequest.Url];
                        MethodInfo method = requestAddressItem.Target.GetType().GetMethod("GetServerErroMsg");
                        try
                        {
                            ParameterInfo[] parameters = requestAddressItem.MethodInfo.GetParameters();
                            object[] array = new object[parameters.Length];
                            foreach (ParameterInfo parameterInfo in parameters)
                            {
                                string value = serverRequest.GetValue(parameterInfo.Name);
                                if (parameterInfo.ParameterType.IsValueType || parameterInfo.ParameterType.FullName.Equals(typeof(string).FullName))
                                {
                                    if (parameterInfo.ParameterType.IsEnum)
                                        array[parameterInfo.Position] = Enum.Parse(parameterInfo.ParameterType, value);
                                    else
                                        array[parameterInfo.Position] = Convert.ChangeType(value, parameterInfo.ParameterType);
                                }
                                else if (this.ParamterConverters.ContainsKey(parameterInfo.ParameterType.FullName))
                                {
                                    array[parameterInfo.Position] = this.ParamterConverters[parameterInfo.ParameterType.FullName].Converter(value);
                                }
                                else
                                {
                                    array[parameterInfo.Position] = _jsonCovert.DeserializeObject(value, parameterInfo.ParameterType);
                                }
                            }
                            try
                            {
                                serverResponse.Result = requestAddressItem.MethodInfo.Invoke(requestAddressItem.Target, array);
                                if (method != null)
                                {
                                    serverResponse.ErroMsg = string.Concat(method.Invoke(requestAddressItem.Target, null));
                                }
                            }
                            catch (Exception ex2)
                            {
                                log.Fail($"客户端调用服务方法执行发生错误:{ex2.Message}");
                                serverResponse.StateCode = RequestStateCode.ServerOptionError;
                                serverResponse.RequestSuccess = false;
                                serverResponse.ErroMsg = ex2.Message;
                            }
                        }
                        catch (Exception ex3)
                        {
                            log.Fail($"客户端调用服务方法参数转换发生错误:{ex3.Message}");
                            serverResponse.StateCode = RequestStateCode.ParamterTypeError;
                            serverResponse.RequestSuccess = false;
                            serverResponse.ErroMsg = ex3.Message;
                        }
                    }
                }
                catch (Exception ex4)
                {
                    log.Fail($"客户端调用服务方法发生错误:{ex4.Message}");
                    serverResponse.StateCode = RequestStateCode.RequestMsgError;
                    serverResponse.RequestSuccess = false;
                    serverResponse.ErroMsg = ex4.Message;
                }
            }
            catch (Exception ex5)
            {
                log.Fail($"客户端调用服务方法发生未知错误:{ex5.Message}");
                serverResponse.StateCode = RequestStateCode.UnknowError;
                serverResponse.RequestSuccess = false;
                serverResponse.ErroMsg = ex5.Message;
            }
            finally
            {
                this.SocketServer.Send(_jsonCovert.SerializeObject(serverResponse), obj.Socket);
            }
        }

        // Token: 0x0600000D RID: 13 RVA: 0x00002604 File Offset: 0x00000804
       
        // Token: 0x06000010 RID: 16 RVA: 0x00002708 File Offset: 0x00000908
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

        // Token: 0x06000011 RID: 17 RVA: 0x00002778 File Offset: 0x00000978
        public override bool Close()
        {
            this.SocketServer.Close();
            this.SocketServer.Dispose();
            return true;
        }
    }
}
