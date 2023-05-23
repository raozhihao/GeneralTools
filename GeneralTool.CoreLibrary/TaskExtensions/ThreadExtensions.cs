using System;
using System.Threading;
using System.Threading.Tasks;

namespace GeneralTool.CoreLibrary.TaskExtensions
{
    /// <summary>
    /// 线程扩展
    /// </summary>
    public class ThreadExtensions
    {
        /// <summary>
        /// 睡眠
        /// </summary>
        /// <param name="sleepCount">睡眠次数,每次睡眠一秒</param>
        /// <param name="token">取消标记</param>
        /// <param name="millisecondsTimeout">每次睡眠时长</param>
        public static void ThreadSleep(int sleepCount, CancellationToken token, int millisecondsTimeout = 1000)
        {
            for (int i = 0; i < sleepCount; i++)
            {
                if (token.IsCancellationRequested)
                {
                    return;
                }
                Thread.Sleep(millisecondsTimeout);
            }
        }

        /// <summary>
        /// 睡眠
        /// </summary>
        /// <param name="sleepCount">睡眠次数,每次睡眠一秒</param>
        /// <param name="token">取消标记</param>
        /// <param name="millisecondsTimeout">每次睡眠时长</param>
        public static void ThreadSleep(int sleepCount, CancellationToken token, TimeSpan millisecondsTimeout)
        {
            for (int i = 0; i < sleepCount; i++)
            {
                if (token.IsCancellationRequested)
                {
                    return;
                }
                Thread.Sleep(millisecondsTimeout);
            }
        }

        /// <summary>
        /// 睡眠
        /// </summary>
        /// <param name="waitCount">睡眠次数,每次睡眠一秒</param>
        /// <param name="token">取消标记</param>
        /// <param name="millisecondsTimeout">每次睡眠时长</param>
        public static async Task TaskWait(int waitCount, CancellationToken token, int millisecondsTimeout = 1000)
        {
            for (int i = 0; i < waitCount; i++)
            {
                if (token.IsCancellationRequested)
                {
                    return;
                }
                await Task.Delay(millisecondsTimeout);
            }
        }

        /// <summary>
        /// 睡眠
        /// </summary>
        /// <param name="waitCount">睡眠次数,每次睡眠一秒</param>
        /// <param name="token">取消标记</param>
        /// <param name="millisecondsTimeout">每次睡眠时长</param>
        public static async Task TaskWait(int waitCount, CancellationToken token, TimeSpan millisecondsTimeout)
        {
            for (int i = 0; i < waitCount; i++)
            {
                if (token.IsCancellationRequested)
                {
                    return;
                }
                await Task.Delay(millisecondsTimeout);
            }
        }
    }
}
