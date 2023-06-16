using System;
using System.Collections.Concurrent;

namespace GeneralTool.CoreLibrary.WPFHelper
{
    /// <summary>
    /// 消息通知类
    /// </summary>
    public class MessageManager
    {
        private static readonly Lazy<MessageManager> instance;

        /// <summary>
        /// 获取消息实例
        /// </summary>
        public static MessageManager Instance { get; private set; }
        static MessageManager()
        {
            instance = new Lazy<MessageManager>(() => new MessageManager());
            Instance = instance.Value;
        }
        private readonly ConcurrentDictionary<string, ConcurrentDictionary<object, object>> actionTokens = new ConcurrentDictionary<string, ConcurrentDictionary<object, object>>();
        private readonly ConcurrentDictionary<string, ConcurrentDictionary<object, object>> funcTokens = new ConcurrentDictionary<string, ConcurrentDictionary<object, object>>();

        /// <summary>
        /// 发送只有一个参数的消息,但没有返回
        /// </summary>
        /// <typeparam name="Pameter">消息的参数类型</typeparam>
        /// <param name="token">消息的Token</param>
        /// <param name="body">消息内容</param>
        public void SendMessage<Pameter>(string token, Pameter body)
        {
            if (actionTokens.TryGetValue(token, out ConcurrentDictionary<object, object> list))
            {
                foreach (System.Collections.Generic.KeyValuePair<object, object> item in list)
                {
                    (item.Value as Action<Pameter>)?.Invoke(body);
                }
            }
        }

        /// <summary>
        /// 发送只有一个参数的消息,但没有返回
        /// </summary>
        /// <typeparam name="Result">消息的返回类型</typeparam>
        /// <typeparam name="Pameter">消息的参数类型</typeparam>
        /// <param name="token">消息的Token</param>
        /// <param name="body">消息内容</param>
        /// <returns>返回注册方所返回的值</returns>
        public Result SendMessage<Result, Pameter>(string token, Pameter body)
        {
            if (funcTokens.TryGetValue(token, out ConcurrentDictionary<object, object> list))
            {
                foreach (System.Collections.Generic.KeyValuePair<object, object> item in list)
                {
                    return item.Value == null ? default : (item.Value as Func<Pameter, Result>)(body);
                }
            }
            return default;
        }

        /// <summary>
        /// 注册消息接收,且无返回
        /// </summary>
        /// <typeparam name="Pameter">消息的参数类型</typeparam>
        /// <param name="register">要注册的主体</param>
        /// <param name="token">注册的token</param>
        /// <param name="action">要注册的处理方法</param>
        public void RegisterMessage<Pameter>(object register, string token, Action<Pameter> action)
        {
            if (!actionTokens.TryGetValue(token, out ConcurrentDictionary<object, object> list))
            {
                if (list == null)
                    list = new ConcurrentDictionary<object, object>();
                _ = list.TryAdd(register, action);
                _ = actionTokens.TryAdd(token, list);
            }
            else
            {
                //如果已存在,则尝试添加
                _ = list.TryAdd(register, action);
            }
        }

        /// <summary>
        /// 注册消息接收,且需要返回
        /// </summary>
        /// <typeparam name="Result">需要返回的类型</typeparam>
        /// <typeparam name="Pameter">消息的参数类型</typeparam>
        /// <param name="register">要注册的主体</param>
        /// <param name="token">注册的token</param>
        /// <param name="func">要注册的处理方法</param>
        public void RegisterMessage<Result, Pameter>(object register, string token, Func<Pameter, Result> func)
        {
            if (!funcTokens.TryGetValue(token, out ConcurrentDictionary<object, object> list))
            {
                if (list == null)
                    list = new ConcurrentDictionary<object, object>();
                _ = list.TryAdd(register, func);
                _ = funcTokens.TryAdd(token, list);
            }
            else
            {
                //如果已存在,则尝试添加
                _ = list.TryAdd(register, func);
            }
        }

        /// <summary>
        /// 取消注册消息
        /// </summary>
        /// <param name="register">需要取消注册的主体</param>
        /// <param name="token">需要取消注册的token,如果为null,或空,则将清除该主体下所有的消息注册</param>
        public void UnRegisterMessage(object register, string token = null)
        {
            if (token == null)
            {
                foreach (System.Collections.Generic.KeyValuePair<string, ConcurrentDictionary<object, object>> item in actionTokens)
                {
                    foreach (System.Collections.Generic.KeyValuePair<object, object> value in item.Value)
                    {
                        if (value.Key == register)
                        {
                            _ = item.Value.TryRemove(value.Key, out _);
                        }
                    }
                }
                foreach (System.Collections.Generic.KeyValuePair<string, ConcurrentDictionary<object, object>> item in funcTokens)
                {
                    foreach (System.Collections.Generic.KeyValuePair<object, object> value in item.Value)
                    {
                        if (value.Key == register)
                        {
                            _ = item.Value.TryRemove(value.Key, out _);
                        }
                    }
                }
                return;
            }
            if (actionTokens.TryGetValue(token, out ConcurrentDictionary<object, object> listAction))
            {
                _ = listAction.TryRemove(register, out _);
            }
            if (funcTokens.TryGetValue(token, out ConcurrentDictionary<object, object> listFunc))
            {
                _ = listFunc.TryRemove(register, out _);
            }
        }
    }
}
