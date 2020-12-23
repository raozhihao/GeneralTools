using System;

namespace GeneralTool.General.ExceptionHelper
{
    /// <summary>
    /// 
    /// </summary>
    public class SerializeException : Exception
    {
        ///<inheritdoc/>
        public SerializeException(string message) : base(message)
        {
        }

        ///<inheritdoc/>
        public SerializeException(string message, Exception innerException) : base(message + ",详情请查看内部信息", innerException)
        {
        }
    }
}
