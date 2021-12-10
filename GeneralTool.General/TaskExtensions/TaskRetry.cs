using System;
using System.Threading;

namespace GeneralTool.General.TaskExtensions
{
    /// <summary>
    /// 任务重试
    /// </summary>
    public class TaskRetry
    {
        /// <summary>
        /// 任务重试
        /// </summary>
        /// <typeparam name="T">传递的参数类型</typeparam>
        /// <param name="predicate">方法</param>
        /// <param name="data">参数</param>
        /// <param name="token">取消标记</param>
        /// <param name="reCount">重试次数</param>
        public static int Retry<T>(Predicate<T> predicate, T data, CancellationToken token, int reCount = 5)
        {
            var msg = string.Empty;
            int index = 0;
            do
            {
                if (token.IsCancellationRequested)
                    return index + 1;
                if (predicate(data))
                {
                    return index + 1;
                }
            } while (index++ < reCount);

            return index;
        }


        /// <summary>
        /// 任务重试
        /// </summary>
        /// <typeparam name="T">传递的参数类型</typeparam>
        /// <param name="predicate">方法</param>
        /// <param name="data">参数</param>
        /// <param name="token">取消标记</param>
        /// <param name="timeOut">重试超时时间</param>
        public static int Retry<T>(Predicate<T> predicate, T data, CancellationToken token, TimeSpan timeOut)
        {
            var msg = string.Empty;
            int index = 0;
            var now = DateTime.Now;
            do
            {
                if (token.IsCancellationRequested)
                    return index + 1;
                if (predicate(data))
                {
                    break;
                }
                index++;

            } while (DateTime.Now - now <= timeOut);

            return index;
        }
    }
}
