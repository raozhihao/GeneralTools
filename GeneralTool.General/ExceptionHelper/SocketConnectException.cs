using System;

namespace GeneralTool.General.ExceptionHelper
{
    /// <summary>
    /// </summary>
    public class SocketConnectException : Exception
    {
        #region Public 构造函数

        ///<inheritdoc/>
        public SocketConnectException(string message) : base(message)
        {
        }

        ///<inheritdoc/>
        public SocketConnectException(string message, Exception innerException) : base(message + ",详情请查看内部信息", innerException)
        {
        }

        #endregion Public 构造函数
    }
}