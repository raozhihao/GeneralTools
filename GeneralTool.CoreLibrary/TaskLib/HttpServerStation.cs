using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;

using GeneralTool.CoreLibrary.Attributes;
using GeneralTool.CoreLibrary.Extensions;
using GeneralTool.CoreLibrary.Interfaces;
using GeneralTool.CoreLibrary.Models;

namespace GeneralTool.CoreLibrary.TaskLib
{
    /// <summary>
    /// Http处理程序
    /// </summary>
    public class HttpServerStation : ServerStationBase
    {
        /// <summary>
        /// 请求到达
        /// </summary>
        public event EventHandler<RequestInfo> HandlerRequest;

        /// <summary>
        /// 请求到达
        /// </summary>
        public event EventHandler<ContextRequest> HalderContext;

        /// <summary>
        /// 
        /// </summary>
        public IJsonConvert JsonConvert { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="log"></param>
        /// <param name="json"></param>
        public HttpServerStation(ILog log = null, IJsonConvert json = null) : base(log)
        {
            if (json == null) json = new BaseJsonCovert();
            JsonConvert = json;
        }

        private HttpListener httpListener;

        /// <summary>
        /// 关闭
        /// </summary>
        /// <returns></returns>
        public override bool Close()
        {
            httpListener?.Stop();
            httpListener?.Close();
            httpListener = null;
            return true;
        }

        /// <summary>
        /// 开启
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        public override bool Start(string ip, int port)
        {
            httpListener = new HttpListener();
            //监听的路径
            httpListener.Prefixes.Add($"http://{ip}:{port}/");
            //设置匿名访问
            httpListener.AuthenticationSchemes = AuthenticationSchemes.Anonymous;
            //开始监听
            httpListener.Start();
            Log.Debug("开始监听...");
            _ = httpListener.BeginGetContext(BeginCall, httpListener);
            return true;
        }
        private void BeginCall(IAsyncResult ar)
        {
            HttpListener httpListener = ar.AsyncState as HttpListener;
            HttpListenerContext context = null;
            try
            {
                context = httpListener.EndGetContext(ar);
            }
            catch (Exception ex)
            {
                if (httpListener.IsListening)
                {
                    Log.Debug("Get context erro:" + ex.GetInnerExceptionMessage());
                }
                return;
            }

            _ = httpListener.BeginGetContext(BeginCall, httpListener);

            //取得请求的对象
            HttpListenerRequest request = context.Request;
            Log.Debug($"{request.HttpMethod} ,{request.RawUrl} ,{request.ProtocolVersion}");
            StreamReader reader = new StreamReader(request.InputStream, Encoding.UTF8);
            string msg = string.Empty;
            try
            {
                msg = reader.ReadToEnd();//读取传过来的信息
            }
            catch (Exception ex)
            {
                Log.Error($"请求读取失败 ：{ex.GetInnerExceptionMessage()}");
                return;
            }

            string reponseString = "";
            try
            {
                //将响应对象进行处理
                reponseString = ExcuteRequest(context, msg);
            }
            catch (Exception ex)
            {
                reponseString = ex.GetInnerExceptionMessage();

                Log.Log(reponseString);
            }

            if (reponseString == null) return;

            HttpListenerResponse response = context.Response;
            // 设置回应头部内容，长度，编码
            response.ContentEncoding = Encoding.UTF8;

            response.ContentType = "application/json; charset=utf-8";

            WriteResponse(response, reponseString);
        }
        private string ExcuteRequest(HttpListenerContext context, string msg)
        {
            if (HalderContext != null)
            {
                HandlerContextMethod(context);
                return null;
            }

            // 取得回应对象
            HttpListenerResponse response = context.Response;
            string url = context.Request.Url.LocalPath;
            url = url.Substring(1);//将前面的/去除
            //判断url是否存在
            bool findUrl = false;
            foreach (KeyValuePair<TaskKey, RequestAddressItem> item in RequestRoute)
            {
                if (item.Key.Name == url)
                {
                    findUrl = true;
                    break;
                }
            }
            if (!findUrl)
            {
                //不存在,返回
                string erro = $"不存在所请示的 [url] - [{url}]";
                Log.Error(erro);
                return erro;
            }

            ServerRequest cmd = new ServerRequest();

            //获取参数
            string method = context.Request.HttpMethod;
            string queryString = WebUtility.UrlDecode(context.Request.Url.Query);
            Dictionary<string, string> dic = queryString.ParseUrlToQueryDictionary();

            if (method.Equals("POST", StringComparison.InvariantCultureIgnoreCase))
            {
                try
                {
                    dic = JsonConvert.DeserializeObject<Dictionary<string, string>>(msg);
                    if (dic == null) dic = new Dictionary<string, string>();
                }
                catch (Exception ex)
                {
                    return JsonConvert.SerializeObject(new ServerResponse() { RequestSuccess = false, ErroMsg = "POST 参数不正确,无法正确转为Json字典" + ex.GetInnerExceptionMessage(), RequestUrl = url });
                }

                string dicString = string.Join(",", dic.Keys);
                foreach (KeyValuePair<TaskKey, RequestAddressItem> item in RequestRoute)
                {
                    if (item.Key.Name != url) continue;

                    ParameterInfo[] parameters = item.Value.MethodInfo.GetParameters();
                    string mps = string.Join(",", parameters.Select(p => p.Name));
                    if (mps != dicString) continue;


                    if (parameters.Length == 1)
                    {
                        ParameterInfo pa = parameters[0];
                        WaterMarkAttribute wa = pa.GetCustomAttribute<WaterMarkAttribute>();
                        if (wa != null && wa.IsJson)
                        {
                            //如果是Json类型,则直接设置了
                            //var value = this.JsonConvert.DeserializeObject(msg, pa.ParameterType);
                            dic = new Dictionary<string, string>
                        {
                            { pa.Name, msg }
                        };
                        }
                    }

                    break;
                }


            }

            foreach (KeyValuePair<string, string> item in dic)
            {
                cmd.Parameters.Add(item.Key, item.Value);
            }
            cmd.Url = url;

            //如果有事件,则交给用户去处理
            if (HandlerRequest != null)
            {
                //判断请求方法是否正确
                RequestAddressItem item = GetRequestItem(cmd);//RequestRoute[url];

                if (item.HttpMethod.ToString().ToLower() != context.Request.HttpMethod.ToLower())
                {
                    return JsonConvert.SerializeObject(new ServerResponse() { RequestSuccess = false, ErroMsg = $"远程请示的Http Metod与接口不一致,请示的 url : {url} ,请示的Http Method : {context.Request.HttpMethod}", RequestUrl = url });
                }

                RequestInfo info = new RequestInfo(cmd, response, item);
                HandlerRequestMethod(info);
                return null;
            }

            Log.Debug($"Request:{msg}");

            string responseString = GetReponseString(cmd, JsonConvert);

            Log.Log($"Response:{responseString}" + Environment.NewLine);

            return responseString;
        }

        private void HandlerContextMethod(HttpListenerContext context)
        {
            HalderContext?.Invoke(this, new ContextRequest(context));
        }

        /// <summary>
        /// 执行
        /// </summary>
        protected virtual void HandlerRequestMethod(RequestInfo requestInfo)
        {
            HandlerRequest?.Invoke(this, requestInfo);
        }

        private void WriteResponse(HttpListenerResponse response, string responseString)
        {
            byte[] buff = Encoding.UTF8.GetBytes(responseString);

            // 输出回应内容
            try
            {
                //必须设置响应中正文的字节数，否则在关闭输出流时将会出错
                response.ContentLength64 = buff.Length;
                Stream output = response.OutputStream;

                output.Write(buff, 0, buff.Length);
                output.Close();

            }
            catch (Exception ex)
            {
                Log.Error($"写入响应失败 :{ex.GetInnerExceptionMessage()}");
            }
            finally
            {
                response.Close();
            }
        }
    }
}
