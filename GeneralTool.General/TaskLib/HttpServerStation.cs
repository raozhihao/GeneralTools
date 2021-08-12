﻿using GeneralTool.General.ExceptionHelper;
using GeneralTool.General.Interfaces;
using GeneralTool.General.Models;
using System;
using System.IO;
using System.Net;
using System.Text;

namespace GeneralTool.General.TaskLib
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
        /// <param name="log"></param>
        public HttpServerStation(ILog log) : base(log)
        {
        }
        HttpListener httpListener;

        /// <summary>
        /// 关闭
        /// </summary>
        /// <returns></returns>
        public override bool Close()
        {
            this.httpListener?.Stop();
            this.httpListener?.Close();
            this.httpListener = null;
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
            Log.Log("开始监听...");
            httpListener.BeginGetContext(BeginCall, httpListener);
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
                Log.Log("Get context erro:" + ex.Message);
                throw;
            }


            httpListener.BeginGetContext(BeginCall, httpListener);

            //取得请求的对象
            HttpListenerRequest request = context.Request;
            Log.Log($"{request.HttpMethod} ,{request.RawUrl} ,{request.ProtocolVersion}");
            var reader = new StreamReader(request.InputStream);
            string msg = string.Empty;
            try
            {
                msg = reader.ReadToEnd();//读取传过来的信息
            }
            catch (Exception ex)
            {
                Log.Error($"请求读取失败 ：{ex.GetInnerExceptionMessage()}");
                throw;
            }

            try
            {
                //将响应对象进行处理
                ExcuteRequest(context, msg);
            }
            catch (Exception ex)
            {
                Log.Log(ex.Message);
            }
        }
        private void ExcuteRequest(HttpListenerContext context, string msg)
        {
            if (this.HalderContext != null)
            {
                HandlerContextMethod(context);
                return;
            }

            // 取得回应对象
            var response = context.Response;
            var url = context.Request.Url.LocalPath;
            url = url.Substring(1);//将前面的/去除
            //判断url是否存在
            if (!this.RequestRoute.ContainsKey(url))
            {
                //不存在,返回
                return;
            }

            var cmd = new ServerRequest();
            var parameters = context.Request.QueryString;
            for (int i = 0; i < parameters.Count; i++)
            {
                var key = parameters.GetKey(i);
                var value = parameters[i];
                cmd.Paramters.Add(key, value);
            }

            cmd.Url = url;

            //如果有事件,则交给用户去处理
            if (this.HandlerRequest != null)
            {
                //判断请求方法是否正确
                var item = this.RequestRoute[url];
                if (item.HttpMethod.ToString().ToLower() != context.Request.HttpMethod.ToLower())
                    return;

                var info = new RequestInfo(cmd, response, item);
                this.HandlerRequestMethod(info);
                return;
            }

            Log.Log($"Request:{msg}");

            // 设置回应头部内容，长度，编码
            response.ContentEncoding = Encoding.UTF8;

            response.ContentType = "application/json; charset=utf-8";


            var serverResponse = this.GetServerResponse(cmd);
            string responseString = serverResponse.SerializeToJsonString();

            Log.Log($"Response:{responseString}" + Environment.NewLine);

            WriteResponse(response, responseString);
        }

        private void HandlerContextMethod(HttpListenerContext context)
        {
            this.HalderContext?.Invoke(this, new ContextRequest(context));
        }

        /// <summary>
        /// 执行
        /// </summary>
        protected virtual void HandlerRequestMethod(RequestInfo requestInfo)
        {
            this.HandlerRequest(this, requestInfo);
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