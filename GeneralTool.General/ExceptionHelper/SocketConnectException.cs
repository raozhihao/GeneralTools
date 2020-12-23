using System;

namespace GeneralTool.General.ExceptionHelper
{
    /// <summary>
    /// 
    /// </summary>
    public class SocketConnectException : Exception
    {
        ///<inheritdoc/>
        public SocketConnectException(string message) : base(message)
        {
        }

        ///<inheritdoc/>
        public SocketConnectException(string message, Exception innerException) : base(message + ",详情请查看内部信息", innerException)
        {
        }
    }
}
