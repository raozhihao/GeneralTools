using System;
using System.Collections.Generic;

using GeneralTool.General.Interfaces;
using GeneralTool.General.Logs;
using GeneralTool.General.Models;
using GeneralTool.General.WPFHelper;

namespace GeneralTool.General.TaskLib
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
            this.Log = log;

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
            var request = this.Parse(methodName, datas);

            return this.InvokeRequest(request);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public object InvokeRequest(ServerRequest request)
        {
            using (var client = new FixedHeadSocketClient(this.Log, this.jsonConvert))
            {
                client.Startup(this.ip, this.port);
                return client.SendResultObject(request.Url, request.Parameters);
            }
        }


        private ServerRequest Parse(string methodName, object[] datas)
        {
            //获取方法
            var parameters = new System.Diagnostics.StackFrame(2).GetMethod().GetParameters();
            if (parameters.Length != datas.Length)
                throw new Exception("传递的参数个数与顺序应与方法一致");

            var dic = new Dictionary<string, string>();
            for (int i = 0; i < parameters.Length; i++)
            {
                var parameterInfo = parameters[i];

                var value = datas[i];
                var stringValue = this.converter.Convert(value, null, null, null) + "";
                dic.Add(parameterInfo.Name, stringValue);
            }

            var requestUrl = url + methodName;

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
            var request = this.Parse(methodName, datas);
            return (T)this.InvokeRequest(request);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Invoke(string methodName, params object[] datas)
        {
            var request = this.Parse(methodName, datas);
            this.InvokeRequest(request);
        }


    }
}
