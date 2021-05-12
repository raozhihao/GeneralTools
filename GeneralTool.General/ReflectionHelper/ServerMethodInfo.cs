using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneralTool.General.ReflectionHelper
{
    /// <summary>
    /// 服务端方法
    /// </summary>
    public class ServerMethodInfo
    {
        /// <summary>
        /// 方法名称
        /// </summary>
        public string MethodName { get; set; }
        /// <summary>
        /// 参数列表
        /// </summary>
        public int ParametersCount { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ServerMethodInfo(string methodName, int parametersCount)
        {
            this.MethodName = methodName;
            this.ParametersCount = parametersCount;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj is ServerMethodInfo info)
            {
                if (info.MethodName != this.MethodName)
                    return false;

                if (this.ParametersCount != info.ParametersCount)
                    return false;

                //不判断参数计数,使其能反馈给调用
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serverMethod"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public static bool operator ==(ServerMethodInfo serverMethod, ServerMethodInfo info)
        {
            return serverMethod.Equals(info);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serverMethod"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public static bool operator !=(ServerMethodInfo serverMethod, ServerMethodInfo info)
        {
            return !serverMethod.Equals(info);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return this.MethodName.GetHashCode();
        }


    }
}
