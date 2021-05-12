using System;
using System.Collections.Generic;
using System.Reflection;
using GeneralTool.General.ExceptionHelper;
using GeneralTool.General.Models;

namespace GeneralTool.General.ReflectionHelper
{
    /// <summary>
    /// 反射类
    /// </summary>
    public class ReflectionClass
    {

        /// <summary>
        /// 获取当前所有可用方法
        /// </summary>
        public Dictionary<string, MethodBase> Methods { get; } = new Dictionary<string, MethodBase>();

        /// <summary>
        /// 获取实例
        /// </summary>
        public object ActivatorObj { get; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="subClassType">实现接口的类型,该类型必须有一个无参构造函数</param>
        /// <exception cref="Exception">此方法有可能引发异常</exception>
        public ReflectionClass(Type subClassType)
        {

            this.ActivatorObj = Activator.CreateInstance(subClassType);
            Init();

        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="instance">实际对象</param>
        public ReflectionClass(object instance)
        {
            this.ActivatorObj = instance;
            Init();
        }


        void Init()
        {
            if (this.ActivatorObj == null)
            {
                throw new Exception("注册类未初始化对象");
            }
            //获取接口中的所有方法
            MethodInfo[] methods = this.ActivatorObj.GetType().GetMethods();
            foreach (MethodInfo item in methods)
            {
                string methodName = item.ToString();
                if (Methods.ContainsKey(methodName))
                {
                    continue;
                }
                Methods.Add(methodName, item);
            }
        }

        /// <summary>
        /// 执行方法 
        /// </summary>
        /// <param name="methodName">方法名称</param>
        /// <param name="paramters">方法参数列表</param>
        /// <returns>返回方法执行完成后所返回的对象</returns>
        public object Invoke(string methodName, params object[] paramters)
        {
            if (!Methods.ContainsKey(methodName))
            {
                throw new Exception("没有找到对应的方法:" + methodName);
            }
            MethodBase method = Methods[methodName];

            return method.Invoke(ActivatorObj, paramters);
        }

        /// <summary>
        /// 执行方法
        /// </summary>
        /// <param name="cmd">请求参数</param>
        /// <returns>返回 ResponseCommand</returns>
        public ResponseCommand Invoke(RequestCommand cmd)
        {
            ResponseCommand rc = new ResponseCommand();
            try
            {
                object obj = Invoke(cmd.MethodName, cmd.Parameters);
                rc.Success = true;
                rc.Messages = "";
                rc.ResultObject = obj;
            }
            catch (Exception ex)
            {
                rc.Success = false;
                rc.Messages = ex.GetInnerExceptionMessage();
            }

            return rc;
        }
    }
}
