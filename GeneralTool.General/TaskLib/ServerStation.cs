using GeneralTool.General.Enums;
using GeneralTool.General.Interfaces;
using GeneralTool.General.Models;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace GeneralTool.General.TaskLib
{
    internal class ServerStation
    {
        private readonly IJsonConvert jsonCovert;
        private Dictionary<string, RequestAddressItem> RequestRoute { get; set; } = new Dictionary<string, RequestAddressItem>();


        private Dictionary<string, ParamterConvertItem> ParamterConverters { get; set; } = new Dictionary<string, ParamterConvertItem>();


        public SocketServer SocketServer { get; set; } = new SocketServer();


        private readonly ILog log;
        public ServerStation(IJsonConvert jsonConvert = null, ILog log = null)
        {
            if (jsonConvert == null)
                this.jsonCovert = new BaseJsonCovert();
            this.log = log;
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
                    serverRequest = jsonCovert.DeserializeObject<ServerRequest>(obj.StringDatas);
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
                                    array[parameterInfo.Position] = Convert.ChangeType(value, parameterInfo.ParameterType);
                                }
                                else if (this.ParamterConverters.ContainsKey(parameterInfo.ParameterType.FullName))
                                {
                                    array[parameterInfo.Position] = this.ParamterConverters[parameterInfo.ParameterType.FullName].Converter(value);
                                }
                                else
                                {
                                    array[parameterInfo.Position] = jsonCovert.DeserializeObject(value, parameterInfo.ParameterType);
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
                this.SocketServer.Send(jsonCovert.SerializeObject(serverResponse), obj.Socket);
            }
        }

        // Token: 0x0600000D RID: 13 RVA: 0x00002604 File Offset: 0x00000804
        public bool AddRoute(string url, object target, MethodInfo m)
        {
            bool flag = this.RequestRoute.ContainsKey(url);
            bool result;
            if (flag)
            {
                this.log.Error($"类型: {target} 方法: {url} 已存在,不添加");
                result = false;
            }
            else
            {

                this.RequestRoute.Add(url, new RequestAddressItem
                {
                    Url = url,
                    MethodInfo = m,
                    Target = target
                });
                result = true;
            }
            return result;
        }

        // Token: 0x0600000E RID: 14 RVA: 0x00002668 File Offset: 0x00000868
        public bool AddTypeConverter<T>(Func<string, object> converter)
        {
            Type typeFromHandle = typeof(T);
            bool flag = this.ParamterConverters.ContainsKey(typeFromHandle.FullName);
            bool result;
            if (flag)
            {
                result = false;
            }
            else
            {
                this.ParamterConverters.Add(typeFromHandle.FullName, new ParamterConvertItem
                {
                    Type = typeFromHandle,
                    Converter = converter
                });
                result = true;
            }
            return result;
        }

        public bool RemoveRoute(string url)
        {
            bool flag = !this.RequestRoute.ContainsKey(url);
            return !flag && this.RequestRoute.Remove(url);
        }

        // Token: 0x06000010 RID: 16 RVA: 0x00002708 File Offset: 0x00000908
        public bool Start(string ip, int port)
        {
            this.SocketServer.IsDelMsgAsyn = false;
            this.SocketServer.IsCheckLink = true;
            this.SocketServer.IsAutoSize = true;
            this.SocketServer.IsDelMsgAsyn = true;
            this.SocketServer.IsReciverForAll = true;
            this.SocketServer.InitSocket(ip, port);
            this.SocketServer.Start();
            return true;
        }

        // Token: 0x06000011 RID: 17 RVA: 0x00002778 File Offset: 0x00000978
        public bool Close()
        {
            this.SocketServer.Close();
            this.SocketServer.Dispose();
            return true;
        }
    }
}
