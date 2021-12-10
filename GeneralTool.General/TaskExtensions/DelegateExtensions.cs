using System;
using System.Reflection;
using System.Threading.Tasks;

using GeneralTool.General.ExceptionHelper;

namespace GeneralTool.General.TaskExtensions
{
    /// <summary>
    /// 返回值
    /// </summary>
    /// <typeparam name="T">
    /// 要返回的类型
    /// </typeparam>
    public struct Result<T>
    {
        #region Public 属性

        /// <summary>
        /// 最后一次的错误信息
        /// </summary>
        public string LastErroMsg { get; set; }

        /// <summary>
        /// 方法是否执行成功
        /// </summary>
        public bool ResultBool { get; set; }

        /// <summary>
        /// 返回值
        /// </summary>
        public T ResultItem { get; set; }

        #endregion Public 属性
    }

    /// <summary>
    /// 为任务重试进行扩展帮助
    /// </summary>
    public static class DelegateExtensions
    {
        #region Private 字段

        private static string lastErroMsg;

        #endregion Private 字段

        #region Public 方法

        /// <summary>
        /// 获取最后一次的错误消息
        /// </summary>
        /// <returns>
        /// </returns>
        public static string GetLastErroMsg()
        {
            return lastErroMsg;
        }

        /// <summary>
        /// 对任务进行计数重试
        /// <list type="bullet">
        /// <item>通过 <see cref="DelegateExtensions.GetLastErroMsg()"/> 获取最后返回的错误信息(如果有)</item>
        /// <item>或者使用 <see cref="DelegateExtensions.RetryResult{T}(Delegate, TimeSpan, int, object[])"/></item>
        /// </list>
        /// </summary>
        /// <typeparam name="T">
        /// 返回的参数类型
        /// </typeparam>
        /// <param name="handler">
        /// 执行的任务
        /// </param>
        /// <param name="reTryCount">
        /// 重试次数
        /// </param>
        /// <param name="paramters">
        /// 任务参数列表
        /// </param>
        /// <returns>
        /// 返回指定类型的数据
        /// </returns>
        /// <exception cref="ArgumentException">
        /// </exception>
        /// <exception cref="TypeNotEqualsException">
        /// </exception>
        public static T Retry<T>(this Delegate handler, int reTryCount = 3, params object[] paramters)
        {
            return handler.Retry<T>(default, reTryCount, paramters);
        }

        /// <summary>
        /// 对任务进行计数重试
        /// <list type="bullet">
        /// <item>通过 <see cref="DelegateExtensions.GetLastErroMsg()"/> 获取最后返回的错误信息(如果有)</item>
        /// <item>或者使用 <see cref="DelegateExtensions.RetryResult{T}(Delegate, TimeSpan, int, object[])"/></item>
        /// </list>
        /// </summary>
        /// <typeparam name="T">
        /// 返回的参数类型
        /// </typeparam>
        /// <param name="handler">
        /// 执行的任务
        /// </param>
        /// <param name="reTryTime">
        /// 重试间隔时间
        /// </param>
        /// <param name="reTryCount">
        /// 重试次数
        /// </param>
        /// <param name="args">
        /// 任务参数列表
        /// </param>
        /// <returns>
        /// 返回指定类型的数据
        /// </returns>
        /// <exception cref="ArgumentException">
        /// </exception>
        /// <exception cref="TypeNotEqualsException">
        /// </exception>
        public static T Retry<T>(this Delegate handler, TimeSpan reTryTime, int reTryCount = 3, params object[] args)
        {
            if (reTryCount == 0)
            {
                System.Diagnostics.Trace.WriteLine("重试完成");
                return default;
            }
            CheckType<T>(handler.Method, args);
            try
            {
                var t = (T)handler.DynamicInvoke(args);
                lastErroMsg = null;
                return t;
            }
            catch (Exception ex)
            {
                var exMsg = ex.GetInnerExceptionMessage();
                string msg = $"任务执行失败,失败消息:{exMsg + Environment.NewLine}当前执行间隔:{reTryTime},当前次数剩余{--reTryCount},准备进行下一次尝试";
                System.Diagnostics.Trace.WriteLine(msg);
                lastErroMsg = exMsg;
                System.Threading.Thread.Sleep(reTryTime);
                return handler.Retry<T>(reTryTime, reTryCount, args);
            }
        }

        /// <summary>
        /// 对任务进行计数重试
        /// <list type="bullet">
        /// <item>通过 <see cref="DelegateExtensions.GetLastErroMsg()"/> 获取最后返回的错误信息(如果有)</item>
        /// <item>
        /// 或者使用 <see cref="DelegateExtensions.RetryResultAsync{T}(Delegate, TimeSpan, int, object[])"/>
        /// </item>
        /// </list>
        /// </summary>
        /// <typeparam name="T">
        /// 返回的参数类型
        /// </typeparam>
        /// <param name="handler">
        /// 执行的任务
        /// </param>
        /// <param name="reTryCount">
        /// 重试次数
        /// </param>
        /// <param name="paramters">
        /// 任务参数列表
        /// </param>
        /// <returns>
        /// 返回指定类型的数据
        /// </returns>
        /// <exception cref="ArgumentException">
        /// </exception>
        /// <exception cref="TypeNotEqualsException">
        /// </exception>
        public static async Task<T> RetryAsync<T>(this Delegate handler, int reTryCount = 3, params object[] paramters)
        {
            return await handler.RetryAsync<T>(default, reTryCount, paramters);
        }

        /// <summary>
        /// 对任务进行计数重试
        /// <list type="bullet">
        /// <item>通过 <see cref="DelegateExtensions.GetLastErroMsg()"/> 获取最后返回的错误信息(如果有)</item>
        /// <item>
        /// 或者使用 <see cref="DelegateExtensions.RetryResultAsync{T}(Delegate, TimeSpan, int, object[])"/>
        /// </item>
        /// </list>
        /// </summary>
        /// <typeparam name="T">
        /// 返回的参数类型
        /// </typeparam>
        /// <param name="handler">
        /// 执行的任务
        /// </param>
        /// <param name="reTryTime">
        /// 重试间隔时间
        /// </param>
        /// <param name="reTryCount">
        /// 重试次数
        /// </param>
        /// <param name="args">
        /// 任务参数列表
        /// </param>
        /// <returns>
        /// 返回指定类型的数据
        /// </returns>
        /// <exception cref="ArgumentException">
        /// </exception>
        /// <exception cref="TypeNotEqualsException">
        /// </exception>
        public static async Task<T> RetryAsync<T>(this Delegate handler, TimeSpan reTryTime, int reTryCount = 3, params object[] args)
        {
            if (reTryCount == 0)
            {
                System.Diagnostics.Trace.WriteLine("重试完成");
                return default;
            }

            CheckType<T>(handler.Method, args);
            try
            {
                var t = await Task.Run(() => (T)handler.DynamicInvoke(args));

                lastErroMsg = null;
                return t;
            }
            catch (Exception ex)
            {
                var exMsg = ex.GetInnerExceptionMessage();
                string msg = $"任务执行失败,失败消息:{exMsg + Environment.NewLine}当前执行间隔:{reTryTime},当前次数剩余{--reTryCount},准备进行下一次尝试";
                System.Diagnostics.Trace.WriteLine(msg);
                lastErroMsg = exMsg;
                await Task.Delay(reTryTime);
                return await handler.RetryAsync<T>(reTryTime, reTryCount, args);
            }
        }

        /// <summary>
        /// 对任务进行计数重试
        /// </summary>
        /// <typeparam name="T">
        /// 返回的参数类型
        /// </typeparam>
        /// <param name="handler">
        /// 执行的任务
        /// </param>
        /// <param name="reTryTime">
        /// 重试间隔时间
        /// </param>
        /// <param name="reTryCount">
        /// 重试次数
        /// </param>
        /// <param name="args">
        /// 任务参数列表
        /// </param>
        /// <returns>
        /// 返回指定类型的数据
        /// </returns>
        /// <exception cref="ArgumentException">
        /// </exception>
        /// <exception cref="TypeNotEqualsException">
        /// </exception>
        public static Result<T> RetryResult<T>(this Delegate handler, TimeSpan reTryTime, int reTryCount = 3, params object[] args)
        {
            if (reTryCount == 0)
            {
                System.Diagnostics.Trace.WriteLine("重试完成");
                return new Result<T>() { LastErroMsg = lastErroMsg, ResultBool = false, ResultItem = default };
            }
            CheckType<T>(handler.Method, args);
            try
            {
                var t = (T)handler.DynamicInvoke(args);
                lastErroMsg = null;
                return new Result<T>() { LastErroMsg = lastErroMsg, ResultItem = t, ResultBool = true };
            }
            catch (Exception ex)
            {
                var exMsg = ex.GetInnerExceptionMessage();
                string msg = $"任务执行失败,失败消息:{exMsg + Environment.NewLine}当前执行间隔:{reTryTime},当前次数剩余{--reTryCount},准备进行下一次尝试";
                System.Diagnostics.Trace.WriteLine(msg);
                lastErroMsg = exMsg;
                System.Threading.Thread.Sleep(reTryTime);
                return handler.RetryResult<T>(reTryTime, reTryCount, args);
            }
        }

        /// <summary>
        /// 对任务进行计数重试
        /// </summary>
        /// <typeparam name="T">
        /// 返回的参数类型
        /// </typeparam>
        /// <param name="handler">
        /// 执行的任务
        /// </param>
        /// <param name="reTryTime">
        /// 重试间隔时间
        /// </param>
        /// <param name="reTryCount">
        /// 重试次数
        /// </param>
        /// <param name="args">
        /// 任务参数列表
        /// </param>
        /// <returns>
        /// 返回指定类型的数据
        /// </returns>
        /// <exception cref="ArgumentException">
        /// </exception>
        /// <exception cref="TypeNotEqualsException">
        /// </exception>
        public static async Task<Result<T>> RetryResultAsync<T>(this Delegate handler, TimeSpan reTryTime = default, int reTryCount = 3, params object[] args)
        {
            if (reTryCount == 0)
            {
                System.Diagnostics.Trace.WriteLine("重试完成");
                return new Result<T>() { LastErroMsg = lastErroMsg, ResultBool = false, ResultItem = default }; ;
            }

            CheckType<T>(handler.Method, args);
            try
            {
                var t = await Task.Run(() => (T)handler.DynamicInvoke(args));

                lastErroMsg = null;
                return new Result<T>() { LastErroMsg = lastErroMsg, ResultBool = true, ResultItem = t };
            }
            catch (Exception ex)
            {
                var exMsg = ex.GetInnerExceptionMessage();
                string msg = $"任务执行失败,失败消息:{exMsg + Environment.NewLine}当前执行间隔:{reTryTime},当前次数剩余{--reTryCount},准备进行下一次尝试";
                System.Diagnostics.Trace.WriteLine(msg);
                lastErroMsg = exMsg;
                await Task.Delay(reTryTime);
                return await handler.RetryResultAsync<T>(reTryTime, reTryCount, args);
            }
        }

        #endregion Public 方法

        #region Private 方法

        private static void CheckType<T>(MethodInfo method, params object[] args)
        {
            //查看参数计数是否匹配
            bool re = method.GetParameters().Length == args.Length;
            if (!re)
            {
                throw new ArgumentException("参数计数不匹配");
            }

            re = method.ReturnType.IsAssignableFrom(typeof(T));
            if (!re)
            {
                throw new TypeNotEqualsException("返回值类型与方法返回类型不匹配");
            }
        }

        #endregion Private 方法
    }
}