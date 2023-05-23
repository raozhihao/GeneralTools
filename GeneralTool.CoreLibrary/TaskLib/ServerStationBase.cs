using System;
using System.Collections.Generic;
using System.Reflection;

using GeneralTool.CoreLibrary.Attributes;
using GeneralTool.CoreLibrary.Enums;
using GeneralTool.CoreLibrary.Extensions;
using GeneralTool.CoreLibrary.Interfaces;
using GeneralTool.CoreLibrary.Logs;
using GeneralTool.CoreLibrary.Models;
using GeneralTool.CoreLibrary.WPFHelper;

namespace GeneralTool.CoreLibrary.TaskLib
{
    /// <summary>
    /// 站点服务基类
    /// </summary>
    public abstract class ServerStationBase : IServerStation
    {
        #region Public 构造函数

        /// <summary>
        /// </summary>
        /// <param name="log">
        /// </param>
        public ServerStationBase(ILog log)
        {
            if (log == null)
                log = new ConsoleLogInfo();
            this.Log = log;
        }

        #endregion Public 构造函数

        #region Public 属性

        /// <summary>
        /// 获取或设置日志
        /// </summary>
        public ILog Log { get; set; }

        #endregion Public 属性

        #region Protected 属性

        /// <summary>
        /// 当前请求的参数转换器集合
        /// </summary>
        public Dictionary<string, ParamterConvertItem> ParamterConverters { get; set; } = new Dictionary<string, ParamterConvertItem>();

        /// <summary>
        /// 当前请求的路由集合
        /// </summary>
        public Dictionary<string, RequestAddressItem> RequestRoute { get; set; } = new Dictionary<string, RequestAddressItem>();


        #endregion Protected 属性

        #region Public 方法

        /// <summary>
        /// 添加路由
        /// </summary>
        /// <param name="url">
        /// </param>
        /// <param name="target">
        /// </param>
        /// <param name="m">
        /// </param>
        /// <param name="httpMethod"></param>
        /// <returns>
        /// </returns>
        public virtual bool AddRoute(string url, object target, MethodInfo m, HttpMethod httpMethod)
        {
            bool flag = this.RequestRoute.ContainsKey(url);
            bool result;
            if (flag)
            {
                this.Log.Error($"类型: {target} 方法: {url} 已存在,不添加");
                result = false;
            }
            else
            {
                this.RequestRoute.Add(url, new RequestAddressItem
                {
                    Url = url,
                    MethodInfo = m,
                    Target = target,
                    HttpMethod = httpMethod
                });
                result = true;
            }
            return result;
        }

        /// <summary>
        /// 获取返回信息
        /// </summary>
        /// <param name="serverRequest">请示类型</param>
        /// <param name="jsonConvert"></param>
        /// <returns></returns>
        public ServerResponse GetServerResponse(ServerRequest serverRequest, IJsonConvert jsonConvert)
        {

            ServerResponse serverResponse = new ServerResponse
            {
                StateCode = RequestStateCode.OK
            };
            try
            {
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
                        serverResponse.ReturnTypeString = requestAddressItem.MethodInfo.ReturnType.AssemblyQualifiedName;
                        try
                        {
                            var converter = new StringConverter();
                            ParameterInfo[] parameters = requestAddressItem.MethodInfo.GetParameters();
                            object[] array = new object[parameters.Length];
                            foreach (ParameterInfo parameterInfo in parameters)
                            {
                                if (serverRequest.Parameters.TryGetValue(parameterInfo.Name, out var value))
                                {
                                    //如果有,则转换
                                    var wa = parameterInfo.GetCustomAttribute<WaterMarkAttribute>();
                                    if (wa != null && wa.IsJson)
                                    {
                                        array[parameterInfo.Position] = jsonConvert.DeserializeObject(value, parameterInfo.ParameterType);
                                    }
                                    else
                                        array[parameterInfo.Position] = converter.ConvertSimpleType(value, parameterInfo.ParameterType);
                                }
                                else
                                {
                                    array[parameterInfo.Position] = converter.ConvertSimpleType(parameterInfo.DefaultValue, parameterInfo.ParameterType);
                                }
                            }
                            try
                            {
                                serverResponse.Result = requestAddressItem.MethodInfo.Invoke(requestAddressItem.Target, array);
                                try
                                {
                                    serverResponse.ResultString = converter.Convert(serverResponse.Result, null, null, null) + "";
                                }
                                catch (Exception ex)
                                {
                                    serverResponse.ResultString = ex.GetInnerExceptionMessage();
                                }
                                if (method != null)
                                {
                                    serverResponse.ErroMsg = string.Concat(method.Invoke(requestAddressItem.Target, null));
                                }
                            }
                            catch (Exception ex2)
                            {
                                this.Log.Fail($"客户端调用服务方法执行发生错误:{ex2}");
                                serverResponse.StateCode = RequestStateCode.ServerOptionError;
                                serverResponse.RequestSuccess = false;
                                serverResponse.ErroMsg = ex2.GetInnerExceptionMessage();
                            }
                        }
                        catch (Exception ex3)
                        {
                            this.Log.Fail($"客户端调用服务方法参数转换发生错误:{ex3}");
                            serverResponse.StateCode = RequestStateCode.ParamterTypeError;
                            serverResponse.RequestSuccess = false;
                            serverResponse.ErroMsg = ex3.GetInnerExceptionMessage();
                        }
                    }
                }
                catch (Exception ex4)
                {
                    this.Log.Fail($"客户端调用服务方法发生错误:{ex4}");
                    serverResponse.StateCode = RequestStateCode.RequestMsgError;
                    serverResponse.RequestSuccess = false;
                    serverResponse.ErroMsg = ex4.GetInnerExceptionMessage();
                }
            }
            catch (Exception ex5)
            {
                this.Log.Fail($"客户端调用服务方法发生未知错误:{ex5}");
                serverResponse.StateCode = RequestStateCode.UnknowError;
                serverResponse.RequestSuccess = false;
                serverResponse.ErroMsg = ex5.GetInnerExceptionMessage();
            }
            return serverResponse;
        }

        /// <summary>
        /// 获取返回信息
        /// </summary>
        /// <param name="serverRequest">请示类型</param>
        /// <param name="jsonConvert">Json转换器</param>
        /// <returns></returns>
        public string GetReponseString(ServerRequest serverRequest, IJsonConvert jsonConvert = null)
        {
            if (jsonConvert == null)
                jsonConvert = new BaseJsonCovert();
            var response = this.GetServerResponse(serverRequest, jsonConvert);

            RequestAddressItem item = this.RequestRoute[serverRequest.Url];
            var attr = item.MethodInfo.GetCustomAttribute<RouteAttribute>();
            if (attr != null)
            {
                if (attr.ReReponse)
                {
                    if (response.RequestSuccess)
                        return response.Result + "";
                    else
                    {
                        if (!string.IsNullOrWhiteSpace(attr.ReReponseErroFomartString))
                        {
                            return attr.ReReponseErroFomartString.Replace("{0}", response.ErroMsg).Replace("\r\n", "");
                        }
                    }
                }
            }

            return jsonConvert.SerializeObject(response);
        }

        /// <summary>
        /// 添加转换器
        /// </summary>
        /// <typeparam name="T">
        /// </typeparam>
        /// <param name="converter">
        /// </param>
        /// <returns>
        /// </returns>
        public virtual bool AddTypeConverter<T>(Func<string, object> converter)
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

        /// <summary>
        /// 关闭服务
        /// </summary>
        /// <returns>
        /// </returns>
        public abstract bool Close();

        /// <summary>
        /// 移除路由
        /// </summary>
        /// <param name="url">
        /// </param>
        /// <returns>
        /// </returns>
        public bool RemoveRoute(string url)
        {
            bool flag = !this.RequestRoute.ContainsKey(url);
            return !flag && this.RequestRoute.Remove(url);
        }

        /// <summary>
        /// 开启服务
        /// </summary>
        /// <param name="ip">
        /// </param>
        /// <param name="port">
        /// </param>
        /// <returns>
        /// </returns>
        public abstract bool Start(string ip, int port);

        #endregion Public 方法
    }

    /// <summary>
    /// Http请求类型枚举
    /// </summary>
    public enum HttpMethod
    {
        /// <summary>
        /// GET
        /// </summary>
        GET,

        /// <summary>
        /// POST
        /// </summary>
        POST
    }
}