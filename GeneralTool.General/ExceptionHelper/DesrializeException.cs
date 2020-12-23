using System;

namespace GeneralTool.General.ExceptionHelper
{
    /// <summary>
    /// 
    /// </summary>
    public class DesrializeException : Exception
    {
        ///<inheritdoc/>
        public DesrializeException(string message) : base(message)
        {
        }

        ///<inheritdoc/>
        public DesrializeException(string message, Exception innerException) : base(message + ",详情请查看内部信息", innerException)
        {
        }
    }
}
