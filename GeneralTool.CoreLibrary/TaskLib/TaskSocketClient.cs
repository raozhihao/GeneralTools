using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading;

using GeneralTool.CoreLibrary.Interfaces;
using GeneralTool.CoreLibrary.Logs;
using GeneralTool.CoreLibrary.Models;
using GeneralTool.CoreLibrary.SocketLib.Interfaces;
using GeneralTool.CoreLibrary.SocketLib.Models;
using GeneralTool.CoreLibrary.SocketLib.Packages;
using GeneralTool.CoreLibrary.WPFHelper;

namespace GeneralTool.CoreLibrary.TaskLib
{
    public class FixedTaskInvokeSocketClient : TaskInvokeSocketClient<FixedHeadRecevieState>
    {
        public FixedTaskInvokeSocketClient(IJsonConvert jsonConvert, ILog log) : base(jsonConvert, log, () => new FixedHeadPackage())
        {
        }
    }

    /// <summary>
    /// 针对TaskManager的客户端
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class TaskInvokeSocketClient<T> : BaseNotifyModel, IDisposable where T : ReceiveState, new()
    {
        /// <summary>
        /// 超时时间
        /// </summary>
        public int ReadTimeout { get; set; } = 180000;

        /// <summary>
        /// 接收缓冲区大小
        /// </summary>
        public int ReceiveBufferSize { get; set; } = 8192;
        /// <summary>
        /// 序列化类
        /// </summary>
        public IJsonConvert JsonConvert { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Ip { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public int Port { get; private set; }

        /// <summary>
        /// 远程的类Url
        /// </summary>
        public string TaskUrl { get; private set; }

        /// <summary>
        /// Log
        /// </summary>
        public ILog Log { get; set; }

        /// <summary>
        /// 是否已经进行了初始化
        /// </summary>
        public bool IsInit { get; protected set; }

        /// <summary>
        /// 命令对象
        /// </summary>
        protected TaskSocketClient<T> Client { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Func<IPackage<T>> PackGeFunc { get; set; }

        private bool disposedValue;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="jsonConvert"></param>
        /// <param name="log"></param>
        public TaskInvokeSocketClient(IJsonConvert jsonConvert, ILog log, Func<IPackage<T>> packageFunc)
        {
            if (log == null)
                log = new ConsoleLogInfo();
            if (jsonConvert == null)
                jsonConvert = new BaseJsonCovert();
            JsonConvert = jsonConvert;

            if (packageFunc == null)
                packageFunc = new Func<IPackage<T>>(() => new NoPackage<T>());

            PackGeFunc = packageFunc;
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <param name="taskUrl">远程的route url</param>
        public virtual void Init(string ip, int port, string taskUrl, int receiveBufferSize = 8192)
        {
            Ip = ip;
            Port = port;
            TaskUrl = taskUrl;
            ReceiveBufferSize = receiveBufferSize;
            Client = new TaskSocketClient<T>(Log, JsonConvert, PackGeFunc);
            Client.Client.ReceiveBufferSize = ReceiveBufferSize;
            Client.ReadTimeOut = ReadTimeout;
            IsInit = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="objects"></param>
        /// <returns></returns>
        protected virtual Dictionary<string, string> GetParameterDic(ParameterInfo[] parameters, object[] objects)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();

            for (int i = 0; i < parameters.Length; i++)
            {
                ParameterInfo parameterInfo = parameters[i];

                object value = objects[i];
                string stringVal = value + "";
                Type parameterType = parameterInfo.ParameterType;
                if (parameterType.IsValueType || parameterType == typeof(string))
                {
                    if (!parameterType.IsPrimitive && !parameterType.IsEnum)
                    {
                        //结构体
                        stringVal = JsonConvert.SerializeObject(value);
                    }
                    else
                        stringVal = Convert.ToString(value);
                }
                else
                {
                    //  stringVal = JsonConvert.SerializeObject(value);
                }

                dic.Add(parameterInfo.Name, stringVal);
            }

            return dic;
        }

        /// <summary>
        /// 执行
        /// </summary>
        /// <typeparam name="Tout"></typeparam>
        /// <param name="objects"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public virtual Tout Execute<Tout>(CancellationToken token, params object[] objects)
        {
            if (!IsInit) throw new Exception("没有进行初始化");
            //获取方法
            MethodBase methodStack = new System.Diagnostics.StackFrame(1).GetMethod();

            ParameterInfo[] parameters = methodStack.GetParameters();
            int len = parameters.Length;

            System.Diagnostics.Trace.WriteLine($"Method Stack:[{methodStack}],Len:[{len}]");
            if (len != objects.Length)
                throw new Exception("传递的参数个数与顺序应与方法一致");

            Dictionary<string, string> dic = GetParameterDic(parameters, objects);

            ServerRequest request = new ServerRequest()
            {
                Url = $"{TaskUrl}{methodStack.Name}",
                Parameters = dic
            };

            Client.Startup(Ip, Port);
            ServerResponse reponse = Client.Send(request, token);

            if (!reponse.RequestSuccess)
                throw new Exception(reponse.ErroMsg);

            if (reponse.Result == null)
                return default;

            Type resultType = Type.GetType(reponse.ReturnTypeString);
            return resultType.IsValueType || resultType == typeof(string)
                ? typeof(Tout) == typeof(byte[])
                    ? (Tout)((object)(Encoding.UTF8.GetBytes(reponse.Result + "")))
                    : reponse.Result is IConvertible
                    ? (Tout)Convert.ChangeType(reponse.Result, typeof(Tout))
                    : (Tout)(new StringConverter().ConvertSimpleType(reponse.Result, resultType))
                : (Tout)reponse.Result;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: 释放托管状态(托管对象)
                    Client?.Dispose();
                }

                // TODO: 释放未托管的资源(未托管的对象)并重写终结器
                // TODO: 将大型字段设置为 null
                disposedValue = true;
            }
        }

        // TODO: 仅当“Dispose(bool disposing)”拥有用于释放未托管资源的代码时才替代终结器
        ~TaskInvokeSocketClient()
        {
            // 不要更改此代码。请将清理代码放入“Dispose(bool disposing)”方法中
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            // 不要更改此代码。请将清理代码放入“Dispose(bool disposing)”方法中
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
