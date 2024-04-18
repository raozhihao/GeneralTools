using System;
using System.Collections.Concurrent;

using GeneralTool.CoreLibrary.Extensions;

namespace GeneralTool.CoreLibrary
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

        protected readonly ConcurrentDictionary<string, object> actionTokens = new ConcurrentDictionary<string, object>();
        protected readonly ConcurrentDictionary<string, object> funcTokens = new ConcurrentDictionary<string, object>();

        /// <summary>
        /// 获取所有Action列表
        /// </summary>
        public ConcurrentDictionary<string, object> Actions => this.actionTokens;

        /// <summary>
        /// 获取所有Function列表
        /// </summary>
        public ConcurrentDictionary<string, object> Functions => this.actionTokens;


        /// <summary>
        /// 获取方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="token"></param>
        /// <returns></returns>
        public Action<T> GetAction<T>(string token)
        {
            this.actionTokens.TryGetValue(token, out var action);
            return action as Action<T>;
        }

        /// <summary>
        /// 获取方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="token"></param>
        /// <returns></returns>
        public Func<T> GetFunc<T>(string token)
        {
            this.funcTokens.TryGetValue(token, out var action);
            return action as Func<T>;
        }

        /// <summary>
        /// 发送只有一个参数的消息,但没有返回
        /// </summary>
        /// <typeparam name="Pameter">消息的参数类型</typeparam>
        /// <param name="token">消息的Token</param>
        /// <param name="body">消息内容</param>
        public void SendMessage<Pameter>(string token, Pameter body)
        {
            if (actionTokens.TryGetValue(token, out var item))
            {
                (item as Action<Pameter>)?.Invoke(body);
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
            if (funcTokens.TryGetValue(token, out var item))
            {
                return item == null ? default : (item as Func<Pameter, Result>)(body);
            }
            return default;
        }

        /// <summary>
        /// 注册消息接收,且无返回
        /// </summary>
        /// <typeparam name="Pameter">消息的参数类型</typeparam>
        /// <param name="token">注册的token</param>
        /// <param name="action">要注册的处理方法</param>
        public void RegisterMessage<Pameter>(string token, Action<Pameter> action)
        {
            if (actionTokens.ContainsKey(token))
                actionTokens[token] = action;
            else
                actionTokens.TryAdd(token, action);
        }


        /// <summary>
        /// 注册消息接收,且需要返回
        /// </summary>
        /// <typeparam name="Result">需要返回的类型</typeparam>
        /// <typeparam name="Pameter">消息的参数类型</typeparam>
        /// <param name="token">注册的token</param>
        /// <param name="func">要注册的处理方法</param>
        public void RegisterMessage<Result, Pameter>(string token, Func<Pameter, Result> func)
        {
            if (funcTokens.ContainsKey(token))
                funcTokens[token] = func;
            else
                funcTokens.TryAdd(token, func);
        }

        /// <summary>
        /// 取消注册消息
        /// </summary>
        /// <param name="token">需要取消注册的token,如果为null,或空,则将清除该主体下所有的消息注册</param>
        public void UnRegisterMessage(string token = null)
        {
            if (token != null)
            {
                this.actionTokens.TryRemove(token);
                this.funcTokens.TryRemove(token);
            }
            else
            {
                //移除所有
                this.actionTokens.Clear();
                this.funcTokens.Clear();
            }
        }
    }
}
