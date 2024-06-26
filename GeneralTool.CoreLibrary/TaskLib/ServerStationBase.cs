﻿using System;
using System.Collections.Generic;
using System.Linq;
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
            Log = log;
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
        public Dictionary<TaskKey, RequestAddressItem> RequestRoute { get; set; } = new Dictionary<TaskKey, RequestAddressItem>();


        /// <summary>
        /// 请求消息事件，此为第二事件
        /// </summary>
        public event Func<ServerRequest, ServerResponse> RequestEvent;

        #endregion Protected 属性

        #region Public 方法

        protected virtual ServerResponse OnRequestEvent(ServerRequest request)
        {
            if (this.RequestEvent == null) return null;
            return this.RequestEvent(request);
        }

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
            TaskKey taskKey = new TaskKey()
            {
                Name = url,
                FullName = m + ""
            };
            bool flag = RequestRoute.ContainsKey(taskKey);
            bool result;
            if (flag)
            {
                Log.Error($"类型: {target} 方法: {url} 已存在,不添加");
                result = false;
            }
            else
            {
                RequestRoute.Add(taskKey, new RequestAddressItem
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
                    RequestAddressItem requestAddressItem = GetRequestItem(serverRequest);


                    if (string.IsNullOrWhiteSpace(requestAddressItem.Url))
                    {
                        serverResponse.StateCode = RequestStateCode.UrlError;
                        serverResponse.RequestSuccess = false;
                        serverResponse.ErroMsg = "未找到对应的接口";
                    }
                    else
                    {
                        MethodInfo method = requestAddressItem.Target.GetType().GetMethod("GetServerErroMsg");
                        serverResponse.ReturnTypeString = requestAddressItem.MethodInfo.ReturnType.AssemblyQualifiedName;
                        try
                        {
                            StringConverter converter = new StringConverter();
                            ParameterInfo[] parameters = requestAddressItem.MethodInfo.GetParameters();
                            object[] array = new object[parameters.Length];
                            foreach (ParameterInfo parameterInfo in parameters)
                            {
                                if (serverRequest.Parameters.TryGetValue(parameterInfo.Name, out string value))
                                {
                                    //如果有,则转换
                                    WaterMarkAttribute wa = parameterInfo.GetCustomAttribute<WaterMarkAttribute>();
                                    array[parameterInfo.Position] = wa != null && wa.IsJson
                                        ? jsonConvert.DeserializeObject(value, parameterInfo.ParameterType)
                                        : converter.ConvertSimpleType(value, parameterInfo.ParameterType);
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
                                    if (serverResponse.Result != null)
                                    {
                                       var type= serverResponse.Result.GetType();
                                        if (type == typeof(string)|| type.IsValueType||type.IsAbstract||type.IsInterface)
                                            serverResponse.ResultString = serverResponse.Result + "";
                                        else 
                                            serverResponse.ResultString = jsonConvert.SerializeObject(serverResponse.Result);

                                    }
                                }
                                catch (Exception)
                                {
                                    serverResponse.ResultString = "";
                                }
                                if (method != null)
                                {
                                    serverResponse.ErroMsg = string.Concat(method.Invoke(requestAddressItem.Target, null));
                                }
                            }
                            catch (Exception ex2)
                            {
                                Log.Fail($"客户端调用服务方法执行发生错误:{ex2}");
                                serverResponse.StateCode = RequestStateCode.ServerOptionError;
                                serverResponse.RequestSuccess = false;
                                serverResponse.ErroMsg = ex2.GetInnerExceptionMessage();
                            }
                        }
                        catch (Exception ex3)
                        {
                            Log.Fail($"客户端调用服务方法参数转换发生错误:{ex3}");
                            serverResponse.StateCode = RequestStateCode.ParamterTypeError;
                            serverResponse.RequestSuccess = false;
                            serverResponse.ErroMsg = ex3.GetInnerExceptionMessage();
                        }
                    }
                }
                catch (Exception ex4)
                {
                    Log.Fail($"客户端调用服务方法发生错误:{ex4}");
                    serverResponse.StateCode = RequestStateCode.RequestMsgError;
                    serverResponse.RequestSuccess = false;
                    serverResponse.ErroMsg = ex4.GetInnerExceptionMessage();
                }
            }
            catch (Exception ex5)
            {
                Log.Fail($"客户端调用服务方法发生未知错误:{ex5}");
                serverResponse.StateCode = RequestStateCode.UnknowError;
                serverResponse.RequestSuccess = false;
                serverResponse.ErroMsg = ex5.GetInnerExceptionMessage();
            }
            return serverResponse;
        }

        public RequestAddressItem GetRequestItem(ServerRequest serverRequest)
        {
            // 查找对应的接口
            foreach (KeyValuePair<TaskKey, RequestAddressItem> item in RequestRoute)
            {
                if (item.Key.Name != serverRequest.Url)
                    continue;

                //查看名称是否一致
                RequestAddressItem value = item.Value;
                ParameterInfo[] mps = value.MethodInfo.GetParameters();
                string mpsNames = string.Join(",", mps.Select(p => p.Name).ToArray());
                string requestNames = string.Join(",", serverRequest.Parameters.Keys);
                if (mpsNames == requestNames)
                {
                    return value;
                }
            }
            return default;
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
            ServerResponse response = GetServerResponse(serverRequest, jsonConvert);

            return GetReponseString(serverRequest,response,jsonConvert);
        }

        public string GetReponseString(ServerRequest serverRequest, ServerResponse response, IJsonConvert jsonConvert )
        {
            RequestAddressItem item = GetRequestItem(serverRequest);//RequestRoute[serverRequest.Url];
            if (!string.IsNullOrWhiteSpace(item.Url))
            {
                RouteAttribute attr = item.MethodInfo.GetCustomAttribute<RouteAttribute>();
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
            bool flag = ParamterConverters.ContainsKey(typeFromHandle.FullName);
            bool result;
            if (flag)
            {
                result = false;
            }
            else
            {
                ParamterConverters.Add(typeFromHandle.FullName, new ParamterConvertItem
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
            List<TaskKey> removes = new List<TaskKey>();
            foreach (KeyValuePair<TaskKey, RequestAddressItem> item in RequestRoute)
            {
                string surl = item.Key.Name;
                if (surl == url)
                {
                    removes.Add(item.Key);
                }
            }
            foreach (TaskKey item in removes)
            {
                _ = RequestRoute.Remove(item);
            }
            return true;
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