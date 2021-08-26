using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GeneralTool.General.SerialPortEx
{
    /// <summary>
    /// SerialPort扩展类
    /// </summary>
    public class SerialControl : SerialPort
    {
        #region Private 字段

        private readonly List<byte> recDatas = new List<byte>();

        private bool isRequest;

        #endregion Private 字段

        #region Public 构造函数

        /// <summary>
        /// </summary>
        public SerialControl()
        {
            base.DataReceived += SerialControl_DataReceived;
        }


        #endregion Public 构造函数

        #region Public 事件

        /// <summary>
        /// 返回出错事件
        /// </summary>
        public event Action<Exception> ErrorMsg;

        /// <summary>
        /// 串口在线状态更改事件,需要以 Open(bool) 激活
        /// </summary>
        public event EventHandler<OnlineStateEventArgs> OnlineStateEvent;

        #endregion Public 事件

        #region Public 属性

        /// <summary>
        /// 获取是否需要进行后台检测串口在线状态
        /// </summary>
        public bool CheckState { get; private set; }

        /// <summary>
        /// 尾
        /// </summary>
        public byte End
        {
            get;
            set;
        }

        /// <summary>
        /// 头
        /// </summary>
        public byte Head
        {
            get;
            set;
        }

        #endregion Public 属性

        #region Private 属性

        private SerialRequest CurrentRequest
        {
            get;
            set;
        }

        private AutoResetEvent RecEvent
        {
            get;
            set;
        } = new AutoResetEvent(initialState: false);

        #endregion Private 属性

        #region Public 方法

        /// <summary>
        /// 关闭
        /// </summary>
        public new void Close()
        {
            if (this.CheckState)
                tokenSource.Cancel();

            base.Close();
            recDatas.Clear();
        }

        /// <summary>
        /// 创建请求
        /// </summary>
        /// <returns>
        /// </returns>
        public SerialRequest CreateRequest()
        {
            return new SerialRequest(Head, End);
        }

        CancellationTokenSource tokenSource;
        /// <summary>
        /// 开启
        /// </summary>
        public new void Open()
        {
            if (base.IsOpen)
            {
                base.Close();
            }

            base.Open();


            if (this.CheckState)
            {
                this.OnlineStateEvent?.Invoke(this, new OnlineStateEventArgs(OnlineState.Online));
                tokenSource = new CancellationTokenSource();

                Task.Run(() =>
                {
                    System.Diagnostics.Trace.WriteLine("开始线程检测");
                    while (!tokenSource.IsCancellationRequested)
                    {
                        if (!this.IsOpen)
                        {
                            if (this.isRequest)
                                RecEvent.Set();
                            this.OnlineStateEvent?.Invoke(this, new OnlineStateEventArgs(OnlineState.Unline));
                            break;
                        }

                        Thread.Sleep(10);
                    }
                    System.Diagnostics.Trace.WriteLine("线程检测已断开");
                });
            }
        }

        /// <summary>
        /// 开启串口
        /// </summary>
        /// <param name="backCheck">是否在后台实时检测</param>
        public void Open(bool backCheck)
        {
            this.CheckState = true;
            this.Open();
        }

        /// <summary>
        /// 发送并返回数据
        /// </summary>
        /// <param name="request">
        /// </param>
        /// <returns>
        /// </returns>
        public SerialResponse Send(SerialRequest request)
        {
            //重置信号量,以免被上一次的污染
            RecEvent.Reset();
            recDatas.Clear();

            if (!request.IsSetData)
                throw new Exception("不能发送没有设置数据的消息结构！");

            if (!this.IsOpen)
            {
                this.isRequest = false;
                return new SerialResponse(request, recDatas.ToArray(), null);
            }
                
            byte[] array = request.ToSendDatas();
            CurrentRequest = request;
            Write(array, 0, array.Length);
            this.isRequest = true;
            RecEvent.WaitOne(base.ReadTimeout);
            this.isRequest = false;
            int num = 0;
            if (recDatas.Count() > 3)
                num = recDatas[2];

            CurrentRequest = null;
            return new SerialResponse(request, recDatas.ToArray(), (num == 0) ? null : recDatas.GetRange(3, num).ToArray());
        }

        #endregion Public 方法

        #region Protected 方法

        /// <summary>
        /// 检查
        /// </summary>
        /// <returns>
        /// </returns>
        protected virtual bool CheckPacketAllReady()
        {
            if (recDatas.Count < 4)
                return false;

            if (CurrentRequest.Head != recDatas.First())
                return false;

            if (CurrentRequest.KeyWorld != recDatas[1])
                return false;

            int num = recDatas[2];
            if (recDatas.Count < 5 + num)
                return false;

            if (recDatas.Count == 5 + num)
            {
                byte b = 0;
                for (int i = 0; i < recDatas.Count - 2; i++)
                    b = (byte)(b + recDatas[i]);

                if (b != recDatas[recDatas.Count - 2])
                    return false;

                if (CurrentRequest.End != recDatas.Last())
                    return false;

                return true;
            }

            recDatas.Clear();
            return false;
        }

        #endregion Protected 方法

        #region Private 方法

        private void SerialControl_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                byte[] array = new byte[base.BytesToRead];
                Read(array, 0, array.Length);
                byte[] array2 = array;
                foreach (byte b in array2)
                {
                    if (recDatas.Count == 0 && CurrentRequest != null && b != CurrentRequest.Head)
                        continue;

                    if (recDatas.Count == 1 && CurrentRequest != null && b != CurrentRequest.KeyWorld)
                    {
                        recDatas.Clear();
                        continue;
                    }

                    recDatas.Add(b);
                    //如果检测通过
                    if (CheckPacketAllReady())
                        RecEvent.Set();
                }
            }
            catch (Exception ex)
            {
                this.ErrorMsg?.Invoke(ex);
                RecEvent.Set();
            }
        }

        #endregion Private 方法
    }
}