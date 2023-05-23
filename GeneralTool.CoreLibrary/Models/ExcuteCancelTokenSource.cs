using System;
using System.Threading;
using System.Threading.Tasks;

namespace GeneralTool.CoreLibrary.Models
{
    /// <summary>
    /// 处理取消标记
    /// </summary>
    public class ExcuteCancelTokenSource
    {
        /// <summary>
        /// 
        /// </summary>
        public ExcuteCancelTokenSource()
        {
            this.tokenSource = new CancellationTokenSource();
        }
        /// <summary>
        /// 通知取消
        /// </summary>
        public void Cancel()
        {
            this.isRequestCancel = true;
            this.tokenSource?.Cancel();
        }

        /// <summary>
        /// 是否暂停
        /// </summary>
        public bool IsPauseNotify { get; private set; }

        /// <summary>
        /// 是否已暂停
        /// </summary>
        public bool IsPaused { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public async Task Pause()
        {
            this.IsPauseNotify = true;
            //查看是否已经获取到了暂停信号
            await Task.Run(async () =>
            {
                while (!IsPaused && !this.isRequestCancel)
                {
                    try
                    {
                        await Task.Delay(10, this.Token);
                    }
                    catch (Exception)
                    {
                        break;
                    }
                }
            });
        }

        /// <summary>
        /// 是否已经触发了恢复
        /// </summary>
        public bool IsResumed { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public async Task Resume()
        {
            this.IsResumed = true;
            await Task.Run(async () =>
            {
                while (!this.IsResumed && !this.isRequestCancel)
                {
                    try
                    {
                        await Task.Delay(10, this.Token);
                    }
                    catch (Exception)
                    {
                    }
                }
            });
        }

        /// <summary>
        /// 标记是否已经触发了取消事件
        /// </summary>
        private bool isFirstRequest = false;
        /// <summary>
        /// 重置取消信号
        /// </summary>
        public void Reset()
        {
            WaitPause();
            this.isFirstRequest = false;
            this.isRequestCancel = false;
            this.CanPause = false;
            this.IsPauseNotify = false;
            this.IsResumed = false;
            this.tokenSource?.Dispose();
            this.tokenSource = new CancellationTokenSource();
        }

        /// <summary>
        /// 等待暂停完成
        /// </summary>
        private void WaitPause()
        {
            while (this.IsPauseNotify)
            {
                IsPaused = true;
                if (this.isRequestCancel && !this.isFirstRequest)
                {
                    //停止命令优先级更高
                    this.NotifyCancel();
                    return;
                }
                if (this.IsResumed)
                    break;
                Thread.Sleep(10);
            }

            //重置
            this.IsPauseNotify = false;
            this.IsResumed = false;
        }

        private void NotifyCancel()
        {
            this.LastIndex = 0;
            this.CanceledEvent?.Invoke(this);
            this.CanceledEvent = null;
            this.isFirstRequest = true;
            this.CanPause = false;
        }

        /// <summary>
        /// 绑定的其它值
        /// </summary>
        public object Tag { get; set; }

        private static readonly object locker = new object();

        /// <summary>
        /// 获取是否已经取消
        /// </summary>
        public bool IsCancelNotify
        {
            get
            {
                lock (locker)
                {
                    //如果是已经触发了取消事件,且未在第一次,则触发事件
                    if (this.isRequestCancel && !this.isFirstRequest)
                    {
                        this.NotifyCancel();
                    }
                    else if (!this.isRequestCancel)
                    {
                        WaitPause();
                    }

                    return isRequestCancel;
                }

            }

        }


        /// <summary>
        /// 最后执行的Index坐标
        /// </summary>
        public int LastIndex { get; set; }
        /// <summary>
        /// 指示当前是否可以暂停
        /// </summary>
        public bool CanPause { get; set; }

        private bool isRequestCancel;

        /// <summary>
        /// 取消成功事件
        /// </summary>
        public event Action<ExcuteCancelTokenSource> CanceledEvent;

        /// <summary>
        /// 无任务
        /// </summary>
        public static ExcuteCancelTokenSource None
        {
            get
            {
                return new ExcuteCancelTokenSource();
            }
        }

        private CancellationTokenSource tokenSource;
        /// <summary>
        /// 
        /// </summary>
        public CancellationToken Token
        {
            get
            {
                if (this.tokenSource == null)
                    return CancellationToken.None;
                return this.tokenSource.Token;
            }
        }

        /// <summary>
        /// 是否已经停止
        /// </summary>
        public bool Canceld { get; set; }

    }
}
