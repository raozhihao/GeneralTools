using GeneralTool.General.ExceptionHelper;
using GeneralTool.General.NetHelper.NetException;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;

namespace GeneralTool.General.NetHelper
{
    /// <summary>
    /// Http帮助类
    /// </summary>
    public class HttpHelper
    {
        #region Public 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        public HttpHelper()
        {
            ReturnEncoding = Encoding.UTF8;
        }

        #endregion Public 构造函数

        #region Public 属性

        /// <summary>
        /// 为 System.Net.CookieCollection 对象的集合提供容器。
        /// </summary>
        public CookieContainer CookieContainer { get; set; } = new CookieContainer();

        /// <summary>
        /// 返回的字符串编码
        /// </summary>
        public Encoding ReturnEncoding { get; set; }

        #endregion Public 属性

        #region Public 方法

        /// <summary>
        /// 添加Cookie
        /// </summary>
        /// <param name="name">
        /// System.Net.Cookie 的名称。 不能在 name 中使用下列字符：等号、分号、逗号、换行符 (\n)、回车符 (\r)、制表符 (\t) 和空格字符 ，货币符号
        /// ($) 不能作为第一个字符。
        /// </param>
        /// <param name="value">
        /// System.Net.Cookie 的值。 下列字符不得用在 value 中：分号、逗号。
        /// </param>
        /// <param name="path">
        /// 此 System.Net.Cookie 适用于的源服务器上的 URI 的子集。 默认值为“/”。
        /// </param>
        public void AddCookie(string name, string value, string path = "/")
        {
            CookieContainer.Add(new Cookie(name, value, path));
        }

        /// <summary>
        /// Http请求
        /// </summary>
        /// <param name="url">
        /// 请求Url
        /// </param>
        /// <param name="queryString">
        /// 请求参数
        /// </param>
        /// <param name="method">
        /// 请求方法
        /// </param>
        /// <param name="userAgent">
        /// </param>
        /// <param name="accept">
        /// </param>
        /// <param name="acceptEncoding">
        /// </param>
        /// <param name="contentType">
        /// </param>
        /// <param name="headers"></param>
        /// <returns>
        /// 返回响应字符串
        /// </returns>
        /// <exception cref="HttpCreateRequestException"/>
        /// <exception cref="HttpGetResponseException"/>
        /// <exception cref="HttpWriteStremException"/>
        public string Http(string url, string queryString, HttpMethod method, string userAgent = "ozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36", string accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3", string acceptEncoding = "gzip, deflate, br", string contentType = "application/x-www-form-urlencoded",Dictionary<string,string> headers=null)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                return "";
            }

            HttpWebRequest request;
            try
            {
                request = (HttpWebRequest)WebRequest.Create(url);
            }
            catch (Exception ex)
            {
                throw new HttpCreateRequestException("创建请求时出错:" + ex.GetInnerExceptionMessage());
            }

            request.Method = method.ToString();
            request.UserAgent = userAgent;

            request.Accept = accept;
            request.Headers.Add("Accept-Encoding", acceptEncoding);

            request.ContentType = contentType;

            if (headers!=null)
            {
                foreach (var item in headers)
                {
                    request.Headers.Add(item.Key, item.Value);
                }
            }
            request.CookieContainer = CookieContainer;

            if (!string.IsNullOrWhiteSpace(queryString))
            {
                byte[] postByts = Encoding.UTF8.GetBytes(queryString);
                request.ContentLength = postByts.Length;
                try
                {
                    using (Stream writeS = request.GetRequestStream())
                    {
                        writeS.Write(postByts, 0, postByts.Length);
                    }
                }
                catch (Exception ex)
                {
                    throw new HttpWriteStremException($"写入请求数据出现错误:{ex.GetInnerExceptionMessage()}");
                }
            }

            string retString = string.Empty;
            try
            {
                using (HttpWebResponse response = (HttpWebResponse)(request.GetResponse()))
                {
                    if (response.ContentEncoding.ToLower().Contains("gzip"))
                    {
                        using (Stream myResponseStream = new System.IO.Compression.GZipStream(response.GetResponseStream(), System.IO.Compression.CompressionMode.Decompress))
                        {
                            using (StreamReader myStreamReader = new StreamReader(myResponseStream, ReturnEncoding))
                            {
                                retString = myStreamReader.ReadToEnd();
                            }
                        }
                    }
                    else
                    {
                        using (StreamReader myStreamReader = new StreamReader(response.GetResponseStream(), ReturnEncoding))
                        {
                            retString = myStreamReader.ReadToEnd();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new HttpGetResponseException($"获取响应数据出现错误:{ex.GetInnerExceptionMessage()}");
            }

            return retString;
        }

        /// <summary>
        /// 简单的Get请求
        /// </summary>
        /// <param name="url">
        /// 请求Url
        /// </param>
        /// <param name="queryString">
        /// 请求参数
        /// </param>
        /// <returns>
        /// 返回响应字符串
        /// </returns>
        /// <exception cref="HttpCreateRequestException"/>
        /// <exception cref="HttpGetResponseException"/>
        /// <exception cref="HttpWriteStremException"/>
        public string HttpGet(string url, string queryString = "")
        {
            return Http(url, queryString, HttpMethod.GET);
        }

        /// <summary>
        /// 简单的POST请求
        /// </summary>
        /// <param name="url">
        /// 请求Url
        /// </param>
        /// <param name="queryString">
        /// 请求参数
        /// </param>
        /// <returns>
        /// 返回响应字符串
        /// </returns>
        /// <exception cref="HttpCreateRequestException"/>
        /// <exception cref="HttpGetResponseException"/>
        /// <exception cref="HttpWriteStremException"/>
        public string HttpPost(string url, string queryString)
        {
            return Http(url, queryString, HttpMethod.POST);
        }

        #endregion Public 方法
    }
}