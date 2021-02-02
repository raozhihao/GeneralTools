using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneralTool.General.ExceptionHelper
{
    /// <summary>
    /// 类型不一致异常
    /// </summary>
    public class TypeNotEqualsException : Exception
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public TypeNotEqualsException(string message) : base(message)
        {
        }
    }
}
