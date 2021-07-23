using GeneralTool.General.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net;
using System.IO;
using GeneralTool.General.ExceptionHelper;
using GeneralTool.General;
using GeneralTool.General.Models;

namespace TaskTest
{
    public class HttpServerStation : GeneralTool.General.TaskLib.ServerStationBase
    {
        public HttpServerStation(ILog log) : base(log)
        {
        }

        HttpListener httpListener;
        public override bool Close()
        { 
            this.httpListener?.Stop();
            this.httpListener?.Close();
            this.httpListener = null;
            return true;
        }

        public override bool Start(string ip, int port)
        {
            httpListener = new HttpListener();
            //监听的路径
            httpListener.Prefixes.Add($"http://{ip}:{port}/");
            //设置匿名访问
            httpListener.AuthenticationSchemes = AuthenticationSchemes.Anonymous;
            //开始监听
            httpListener.Start();
            Console.WriteLine("开始监听...");
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
                Console.WriteLine("Get context erro:" + ex.Message);
                throw;
            }


            httpListener.BeginGetContext(BeginCall, httpListener);
            //取得请求的对象
            HttpListenerRequest request = context.Request;
            Console.WriteLine("{0} {1} {2}", request.HttpMethod, request.RawUrl, request.ProtocolVersion);
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
                Console.WriteLine(ex.Message);
            }
        }

        private void ExcuteRequest(HttpListenerContext context, string msg)
        {
          
            var cmd = msg.DeserializeJsonToObject<ServerRequest>();
            Console.WriteLine($"Request:{msg}");

            // 取得回应对象
            HttpListenerResponse response = context.Response;

            // 设置回应头部内容，长度，编码
            response.ContentEncoding = Encoding.UTF8;

            response.ContentType = "text/xml; charset=utf-8";
            string responseString = "Return .." + Guid.NewGuid();
            Console.WriteLine($"Response:{responseString}" + Environment.NewLine);
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
