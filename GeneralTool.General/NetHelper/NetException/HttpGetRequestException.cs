namespace GeneralTool.General.NetHelper.NetException
{
    /// <summary>
    /// 创建Request时的异常
    /// </summary>
    public class HttpCreateRequestException : System.Exception
    {
        #region Public 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="message">
        /// 错误信息
        /// </param>
        public HttpCreateRequestException(string message) : base(message)
        {
        }

        #endregion Public 构造函数
    }

    /// <summary>
    /// 获取Response时的异常
    /// </summary>
    public class HttpGetResponseException : System.Exception
    {
        #region Public 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="message">
        /// 错误信息
        /// </param>
        public HttpGetResponseException(string message) : base(message)
        {
        }

        #endregion Public 构造函数
    }

    /// <summary>
    /// 写入数据时的异常
    /// </summary>
    public class HttpWriteStremException : System.Exception
    {
        #region Public 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="message">
        /// 错误信息
        /// </param>
        public HttpWriteStremException(string message) : base(message)
        {
        }

        #endregion Public 构造函数
    }
}