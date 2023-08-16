using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace GeneralTool.CoreLibrary.Models
{
    #region oLD
    ///// <summary>
    ///// 处理取消标记
    ///// </summary>
    //public class ExcuteCancelTokenSource
    //{
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    public ExcuteCancelTokenSource()
    //    {
    //        tokenSource = new CancellationTokenSource();
    //    }
    //    /// <summary>
    //    /// 通知取消
    //    /// </summary>
    //    public void Cancel()
    //    {
    //        isRequestCancel = true;
    //        if (this.tokenSource != null)
    //        {
    //            this.tokenSource.Cancel();
    //        }
    //    }

    //    /// <summary>
    //    /// 发送了暂停信号事件
    //    /// </summary>
    //    public event Action PauseEvent;

    //    /// <summary>
    //    /// 发送了恢复信号事件
    //    /// </summary>
    //    public event Action ResumeEvent;

    //    /// <summary>
    //    /// 是否暂停
    //    /// </summary>
    //    public bool IsPauseNotify { get; private set; }

    //    /// <summary>
    //    /// 是否已暂停
    //    /// </summary>
    //    public bool IsPaused { get; private set; }
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    public async Task Pause()
    //    {
    //        this.PauseEvent?.Invoke();
    //        IsPauseNotify = true;
    //        //查看是否已经获取到了暂停信号
    //        await Task.Run(async () =>
    //        {

    //            while (!IsPaused && !isRequestCancel)
    //            {
    //                if (this.IsResumed) break;
    //                try
    //                {
    //                    await Task.Delay(10, Token);
    //                }
    //                catch (Exception)
    //                {
    //                    break;
    //                }
    //            }

    //        });
    //    }

    //    /// <summary>
    //    /// 是否已经触发了恢复
    //    /// </summary>
    //    public bool IsResumed { get; private set; }
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    public async Task Resume()
    //    {
    //        this.ResumeEvent?.Invoke();
    //        IsResumed = true;
    //        await Task.Run(async () =>
    //        {
    //            while (!IsResumed && !isRequestCancel)
    //            {
    //                if(this.IsPauseNotify) break;
    //                try
    //                {
    //                    await Task.Delay(10, Token);
    //                }
    //                catch (Exception)
    //                {
    //                }
    //            }
    //        });
    //    }

    //    /// <summary>
    //    /// 标记是否已经触发了取消事件
    //    /// </summary>
    //    private bool isFirstRequest = false;
    //    /// <summary>
    //    /// 重置取消信号
    //    /// </summary>
    //    public void Reset()
    //    {
    //        WaitPause();
    //        isFirstRequest = false;
    //        isRequestCancel = false;
    //        CanPause = false;
    //        IsPauseNotify = false;
    //        IsResumed = false;
    //        tokenSource?.Dispose();
    //        tokenSource = new CancellationTokenSource();
    //    }

    //    /// <summary>
    //    /// 等待暂停完成
    //    /// </summary>
    //    private void WaitPause()
    //    {
    //        while (IsPauseNotify)
    //        {
    //            IsPaused = true;
    //            if (isRequestCancel && !isFirstRequest)
    //            {
    //                //停止命令优先级更高
    //                NotifyCancel();
    //                return;
    //            }
    //            if (IsResumed)
    //                break;
    //            Thread.Sleep(10);
    //        }

    //        //重置
    //        IsPauseNotify = false;
    //        IsResumed = false;
    //    }

    //    private void NotifyCancel()
    //    {
    //        LastIndex = 0;
    //        CanceledEvent?.Invoke(this);
    //        CanceledEvent = null;
    //        isFirstRequest = true;
    //        CanPause = false;
    //    }

    //    /// <summary>
    //    /// 绑定的其它值
    //    /// </summary>
    //    public object Tag { get; set; }

    //    private static readonly object locker = new object();

    //    /// <summary>
    //    /// 获取是否已经取消
    //    /// </summary>
    //    public bool IsCancelNotify
    //    {
    //        get
    //        {
    //            lock (locker)
    //            {
    //                //如果是已经触发了取消事件,且未在第一次,则触发事件
    //                if (isRequestCancel && !isFirstRequest)
    //                {
    //                    NotifyCancel();
    //                }
    //                else if (!isRequestCancel)
    //                {
    //                    WaitPause();
    //                }

    //                return isRequestCancel;
    //            }

    //        }

    //    }

    //    /// <summary>
    //    /// 最后执行的Index坐标
    //    /// </summary>
    //    public int LastIndex { get; set; }
    //    /// <summary>
    //    /// 指示当前是否可以暂停
    //    /// </summary>
    //    public bool CanPause { get; set; }

    //    private bool isRequestCancel;

    //    /// <summary>
    //    /// 取消成功事件
    //    /// </summary>
    //    public event Action<ExcuteCancelTokenSource> CanceledEvent;

    //    /// <summary>
    //    /// 无任务
    //    /// </summary>
    //    public static ExcuteCancelTokenSource None
    //    {
    //        get
    //        {
    //            return new ExcuteCancelTokenSource();
    //        }
    //    }

    //    private CancellationTokenSource tokenSource;
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    public CancellationToken Token
    //    {
    //        get
    //        {
    //            return tokenSource == null ? CancellationToken.None : tokenSource.Token;
    //        }
    //    }

    //    /// <summary>
    //    /// 是否已经停止
    //    /// </summary>
    //    public bool Canceld { get; set; }

    //} 
    #endregion

    #region NEW

    /// <summary>
    /// 暂停参数
    /// </summary>
    public class TokenPausArgs : EventArgs
    {
        public TokenPausArgs(bool paused)
        {
            Paused = paused;
        }

        /// <summary>
        /// 是否已完成暂停
        /// </summary>
        public bool Paused { get; set; }

    }

    /// <summary>
    /// 恢复参数
    /// </summary>
    public class TokenResumeArgs : EventArgs
    {
        public TokenResumeArgs(bool resumed)
        {
            Resumed = resumed;
        }

        /// <summary>
        /// 是否已完成恢复
        /// </summary>
        public bool Resumed { get; set; }

    }

    

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
            tokenSource = new CancellationTokenSource();
        }
        /// <summary>
        /// 通知取消
        /// </summary>
        public void Cancel()
        {
            this.tokenSource?.Cancel();
            IsRequestCancel = true;
        }

        /// <summary>
        /// 发送了暂停信号事件
        /// </summary>
        public event EventHandler<TokenPausArgs> PauseEvent;

        /// <summary>
        /// 发送了恢复信号事件
        /// </summary>
        public event EventHandler<TokenResumeArgs> ResumeEvent;


        /// <summary>
        /// 是否暂停
        /// </summary>
        public bool IsPauseNotify { get; private set; }

        /// <summary>
        /// 是否已暂停
        /// </summary>
        public bool IsPaused { get; private set; }

        /// <summary>
        /// 暂停
        /// </summary>
        public async Task Pause()
        {
            Trace.WriteLine("Start pause");
            this.PauseEvent?.Invoke(this, new TokenPausArgs(false));
            IsPauseNotify = true;
            //查看是否已经获取到了暂停信号
            await Task.Run(async () =>
            {
                while (!IsPaused && !IsRequestCancel)
                {
                    if (this.IsResumed) break;
                    try
                    {
                        await Task.Delay(10, Token);
                    }
                    catch (Exception)
                    {
                        break;
                    }
                }

            });
            this.PauseEvent?.Invoke(this, new TokenPausArgs(true));
            Trace.WriteLine("End Pause");
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
            if (!this.IsPauseNotify) return;
            Trace.WriteLine("Start resume");
            this.ResumeEvent?.Invoke(this, new TokenResumeArgs(false));
            IsResumed = true;
            await Task.Run(async () =>
            {
                while (!IsResumed && !IsRequestCancel)
                {
                    if (this.IsPauseNotify) break;
                    try
                    {
                        await Task.Delay(10, Token);
                    }
                    catch (Exception)
                    {
                    }
                }
            });
            this.ResumeEvent?.Invoke(this, new TokenResumeArgs(true));
            Trace.WriteLine("End Resume");
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
            this.NotifyCancel();
            //WaitPause();
            isFirstRequest = false;
            IsRequestCancel = false;
            CanPause = false;
            IsPauseNotify = false;
            IsResumed = false;
            tokenSource?.Dispose();
            tokenSource = new CancellationTokenSource();
        }

        /// <summary>
        /// 等待暂停完成
        /// </summary>
        private void WaitPause()
        {
            while (IsPauseNotify)
            {
                IsPaused = true;
                if (IsRequestCancel && !isFirstRequest)
                {
                    //停止命令优先级更高
                    NotifyCancel();
                    return;
                }
                if (IsResumed)
                    break;
                Thread.Sleep(10);
            }

            //重置
            IsPauseNotify = false;
            IsResumed = false;
        }

        private void NotifyCancel()
        {
            CanceledEvent?.Invoke(this);
            CanceledEvent = null;
            isFirstRequest = true;
            CanPause = false;
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
                    if (IsRequestCancel && !isFirstRequest)
                    {
                        NotifyCancel();
                    }
                    else if (!IsRequestCancel)
                    {
                        WaitPause();
                    }

                    return IsRequestCancel;
                }

            }

        }

        /// <summary>
        /// 指示当前是否可以暂停
        /// </summary>
        public bool CanPause { get; set; }

        /// <summary>
        /// 是否发送了取消标记
        /// </summary>
        public bool IsRequestCancel { get; private set; }

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
                return tokenSource == null ? CancellationToken.None : tokenSource.Token;
            }
        }

        /// <summary>
        /// 是否已经停止
        /// </summary>
        public bool Canceld { get; set; }

    }

    #endregion
}
