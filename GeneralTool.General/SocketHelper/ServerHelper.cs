using GeneralTool.General.Models;
using GeneralTool.General.ReflectionHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace GeneralTool.General.SocketHelper.Server
{
    /// <summary>
    /// 服务帮助类
    /// </summary>
    public class ServerHelper : IDisposable
    {
        private System.Net.Sockets.Socket serverSocket;
        private readonly System.Collections.Concurrent.ConcurrentDictionary<string, ReflectionClass> caches;
        private readonly System.Collections.Concurrent.ConcurrentDictionary<string, System.Net.Sockets.Socket> clients;
        private readonly SerializeHelpers serialize = new SerializeHelpers();


        private bool disposedValue;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="host">路径</param>
        /// <param name="port">端口</param>
        public ServerHelper(string host = "127.0.0.1", int port = 55155)
        {
            caches = new System.Collections.Concurrent.ConcurrentDictionary<string, ReflectionClass>();
            clients = new System.Collections.Concurrent.ConcurrentDictionary<string, System.Net.Sockets.Socket>();
            serverSocket = new System.Net.Sockets.Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serverSocket.Bind(new IPEndPoint(IPAddress.Parse(host), port));
            serverSocket.Blocking = true;

        }


        /// <summary>
        /// 注册类型
        /// </summary>
        /// <typeparam name="TInterface">接口类型</typeparam>
        /// <typeparam name="TCallType">实现 TInterface 的实际类型</typeparam>
        /// <returns></returns>
        public bool RegisterClass<TInterface, TCallType>()
        {
            bool re = typeof(TInterface).IsInterface;
            if (!re)
            {
                throw new Exception("参数 TInterface 不是一个接口类型");
            }

            Type callType = typeof(TCallType);
            re = callType.GetInterface(typeof(TInterface).Name) == null;
            if (re)
            {
                throw new Exception("参数 callClass 未从参数 TInterface 继承");
            }
            string name = typeof(TInterface).Name;
            ReflectionClass reflection = new ReflectionClass(callType);
            if (caches.ContainsKey(name))
            {
                caches[name] = null;
                caches[name] = reflection;
                return true;
            }
            else
            {
                return caches.TryAdd(name, reflection);
            }

        }

        /// <summary>
        /// 注册类型
        /// </summary>
        /// <typeparam name="TInterface">接口类型</typeparam>
        /// <param name="instance">实现 TInterface 的实际对象</param>
        /// <returns></returns>
        public bool RegisterClass<TInterface>(object instance)
        {
            bool re = typeof(TInterface).IsInterface;
            if (!re)
            {
                throw new Exception("参数 TInterface 不是一个接口类型");
            }

            Type callType = instance.GetType();

            re = callType.GetInterface(typeof(TInterface).Name) == null;
            if (re)
            {
                throw new Exception("参数 callClass 未从参数 TInterface 继承");
            }
            string name = typeof(TInterface).Name;
            ReflectionClass reflection = new ReflectionClass(instance);
            if (caches.ContainsKey(name))
            {
                caches[name] = null;
                caches[name] = reflection;
                return true;
            }
            else
            {
                return caches.TryAdd(name, reflection);
            }
        }

        /// <summary>
        /// 开启侦听(在调用此方法之前,请先调用 <see cref="RegisterClass{TInterface, TCallType}"/> 或 <see cref="RegisterClass{TInterface}(object)"/>)
        /// </summary>
        /// <param name="backListenCount"> 挂起连接队列的最大长度。</param>
        /// <returns></returns>
        public bool Start(int backListenCount = 10)
        {
            serverSocket.Listen(backListenCount);
            serverSocket.BeginAccept(AcceptMethod, serverSocket);
            serverSocket.SetSocketKeepAlive();
            return true;
        }


        private void AcceptMethod(IAsyncResult ar)
        {
            Socket socket = ar.AsyncState as System.Net.Sockets.Socket;

            System.Net.Sockets.Socket clientSocket = null;
            try
            {
                clientSocket = socket.EndAccept(ar);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"客户端连接发生异常 : {ex.Message}");
                return;
            }

            socket.BeginAccept(AcceptMethod, socket);

            string key = clientSocket.RemoteEndPoint.ToString();

            if (!clients.ContainsKey(key))
            {
                clients.TryAdd(key, clientSocket);
                Console.WriteLine($"新的客户端 {key} 已连接");
            }
            else
            {
                clients[key] = clientSocket;
                Console.WriteLine($"客户端 {key} 已重新连接");
            }


            List<byte> list = new List<byte>();
            while (true)
            {
                if (!clientSocket.IsClientConnected())
                {
                    Console.WriteLine($"客户端 {key} 已下线");
                    clients.TryRemove(key, out _);
                    clientSocket.Close();
                    clientSocket.Dispose();
                    break;
                }

                byte[] buffer = new byte[1024];
                list.Clear();
                if (!clientSocket.Connected)
                {
                    break;
                }
                while (clientSocket.Available > 0)
                {
                    int len = clientSocket.Receive(buffer, 0, buffer.Length, SocketFlags.None);
                    list.AddRange(buffer.Take(len));
                    if (len < buffer.Length)
                    {
                        break;
                    }
                }

                if (list.Count > 0)
                {
                    RequestCommand cmd = null;
                    try
                    {
                        cmd = serialize.Desrialize<RequestCommand>(list.ToArray());
                    }
                    catch (Exception ex)
                    {
                        ResponseCommand reponseCmd = new ResponseCommand
                        {
                            Success = false,
                            Messages = "反序列化出现错误",
                            ResultObject = ex
                        };
                        byte[] bytes = serialize.Serialize(reponseCmd);
                        clientSocket.Send(bytes, 0);
                        continue;
                    }

                    bool re = caches.TryGetValue(cmd.ClassName, out ReflectionClass value);
                    if (!re)
                    {
                        ResponseCommand reponseCmd = new ResponseCommand
                        {
                            Messages = "服务端未注册该接口的实现类",
                            Success = false
                        };
                        byte[] bytes = serialize.Serialize(reponseCmd);
                        clientSocket.Send(bytes);
                        continue;
                    }
                    else
                    {
                        ResponseCommand reponseCmd = value.Invoke(cmd);
                        try
                        {
                            byte[] bytes = serialize.Serialize(reponseCmd);
                            clientSocket.Send(bytes, 0);
                        }
                        catch (Exception ex)
                        {
                            reponseCmd.Success = false;
                            reponseCmd.Messages = "反序列化出现错误";
                            reponseCmd.ResultObject = ex;
                            byte[] bytes = serialize.Serialize(reponseCmd);
                            clientSocket.Send(bytes, 0);
                        }
                    }
                }
                System.Threading.Thread.Sleep(500);

            }

        }

        /// <summary>
        /// 释放
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: 释放托管状态(托管对象)
                    try
                    {
                        foreach (KeyValuePair<string, Socket> item in clients)
                        {
                            item.Value.Close();
                            item.Value.Dispose();
                        }

                        clients.Clear();
                        serverSocket.Close();
                        serverSocket.Dispose();

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                serverSocket = null;
                // TODO: 释放未托管的资源(未托管的对象)并替代终结器
                // TODO: 将大型字段设置为 null
                disposedValue = true;
            }
        }

        // TODO: 仅当“Dispose(bool disposing)”拥有用于释放未托管资源的代码时才替代终结器
        /// <summary>
        /// 
        /// </summary>
        ~ServerHelper()
        {
            // 不要更改此代码。请将清理代码放入“Dispose(bool disposing)”方法中
            Dispose(disposing: false);
        }

        /// <summary>
        /// 释放
        /// </summary>
        public void Dispose()
        {
            // 不要更改此代码。请将清理代码放入“Dispose(bool disposing)”方法中
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
