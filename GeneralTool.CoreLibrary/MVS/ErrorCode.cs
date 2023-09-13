using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace GeneralTool.CoreLibrary.MVS
{
    /// <summary>
    /// 
    /// </summary>
    public class ErrorCode
    {
        private static readonly Lazy<ErrorCode> errorCodeInstance;
        /// <summary>
        /// 
        /// </summary>
        public static ErrorCode ErrorCodeInstance { get; }
        static ErrorCode()
        {
            errorCodeInstance = new Lazy<ErrorCode>(() => new ErrorCode());
            ErrorCodeInstance = errorCodeInstance.Value;
        }
        private readonly Dictionary<int, string> errorCodes = new Dictionary<int, string>();

        /// <summary>
        /// 
        /// </summary>
        public ErrorCode()
        {
            //初始化所有错误码
            System.Reflection.FieldInfo[] fields = typeof(Errors).GetFields();
            foreach (System.Reflection.FieldInfo item in fields)
            {
                if ((Attribute.GetCustomAttribute(item, typeof(DescriptionAttribute)) as DescriptionAttribute) != null)
                {
                    errorCodes.Add(Convert.ToInt32(item.GetValue(null)), (Attribute.GetCustomAttribute(item, typeof(DescriptionAttribute)) as DescriptionAttribute).Description);
                }
            }
        }

        /// <summary>
        /// 获取错误
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public string this[int code]
        {
            get
            {
                return errorCodes.TryGetValue(code, out string codeString) ? codeString : code + "";
            }
        }

        /// <summary>
        /// 获取错误信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public string GetErrorString(int code)
        {
            return this[code];
        }

        /// <summary>
        /// 如果错误则抛异常
        /// </summary>
        /// <param name="code"></param>
        /// <exception cref="Exception"></exception>
        public void IfErrorThrowExecption(int code)
        {
            if (code != MVSCameraProvider.MV_OK)
            {
                throw new Exception(this[code]);
            }
        }
    }
}
