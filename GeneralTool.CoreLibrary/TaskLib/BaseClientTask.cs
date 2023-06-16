using System;
using System.Collections.Generic;

using GeneralTool.CoreLibrary.Interfaces;
using GeneralTool.CoreLibrary.Logs;
using GeneralTool.CoreLibrary.Models;
using GeneralTool.CoreLibrary.WPFHelper;

namespace GeneralTool.CoreLibrary.TaskLib
{
    /// <summary>
    /// 为Dto任务类进行处理
    /// </summary>
    public class BaseClientTask
    {
        /// <summary>
        /// 
        /// </summary>
        public ILog Log { get; set; }

        private readonly IJsonConvert jsonConvert;
        private readonly StringConverter converter = new StringConverter();

        private readonly string url;
        private readonly string ip;
        private readonly int port;

        /// <summary>
        /// 
        /// </summary>
        public BaseClientTask(string ip, int port, string url, ILog log = null, IJsonConvert jsonConvert = null)
        {
            if (log == null) log = new ConsoleLogInfo();
            Log = log;

            if (jsonConvert == null) jsonConvert = new BaseJsonCovert();
            this.jsonConvert = jsonConvert;

            if (string.IsNullOrWhiteSpace(url))
                throw new ArgumentNullException(url);

            this.url = url;

            if (string.IsNullOrWhiteSpace(ip))
                throw new ArgumentNullException(ip);

            this.ip = ip;
            this.port = port;
        }

        /// <summary>
        /// 
        /// </summary>
        public object InvokeResult(string methodName, params object[] datas)
        {
            ServerRequest request = Parse(methodName, datas);

            return InvokeRequest(request);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public object InvokeRequest(ServerRequest request)
        {
            using (FixedHeadSocketClient client = new FixedHeadSocketClient(Log, jsonConvert))
            {
                client.Startup(ip, port);
                return client.SendResultObject(request.Url, request.Parameters);
            }
        }

        private ServerRequest Parse(string methodName, object[] datas)
        {
            //获取方法
            System.Reflection.ParameterInfo[] parameters = new System.Diagnostics.StackFrame(2).GetMethod().GetParameters();
            if (parameters.Length != datas.Length)
                throw new Exception("传递的参数个数与顺序应与方法一致");

            Dictionary<string, string> dic = new Dictionary<string, string>();
            for (int i = 0; i < parameters.Length; i++)
            {
                System.Reflection.ParameterInfo parameterInfo = parameters[i];

                object value = datas[i];
                string stringValue = converter.Convert(value, null, null, null) + "";
                dic.Add(parameterInfo.Name, stringValue);
            }

            string requestUrl = url + methodName;

            return new ServerRequest()
            {
                Url = requestUrl,
                Parameters = dic
            };
        }

        /// <summary>
        /// 
        /// </summary>
        public T InvokeResult<T>(string methodName, params object[] datas)
        {
            ServerRequest request = Parse(methodName, datas);
            return (T)InvokeRequest(request);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Invoke(string methodName, params object[] datas)
        {
            ServerRequest request = Parse(methodName, datas);
            _ = InvokeRequest(request);
        }

    }
}
