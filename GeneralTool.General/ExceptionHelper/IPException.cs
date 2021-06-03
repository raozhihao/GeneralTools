using System;

namespace GeneralTool.General.ExceptionHelper
{
    /// <summary>
    /// 
    /// </summary>
    public class IPException : Exception
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="errorip"></param>
        public IPException(string errorip) : base("ip错误，不是正确的ip")
        {
            this.ErrorIP = errorip;
        }

        /// <summary>
        /// 
        /// </summary>
        public string ErrorIP = "";
    }
}
