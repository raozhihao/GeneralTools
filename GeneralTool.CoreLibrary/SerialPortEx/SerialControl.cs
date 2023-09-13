
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using GeneralTool.CoreLibrary.Extensions;

namespace GeneralTool.CoreLibrary.SerialPortEx
{
    /// <summary>
    /// SerialPort扩展类
    /// </summary>
    public class SerialControl : SerialPort
    {
        /// <summary>
        /// 数据缓存区
        /// </summary>
        private readonly System.Collections.Concurrent.ConcurrentQueue<byte> recDatas = new System.Collections.Concurrent.ConcurrentQueue<byte>();

        /// <summary>
        /// </summary>
        public SerialControl()
        {
            base.DataReceived += SerialControl_DataReceived;
        }


        /// <summary>
        /// 返回出错事件
        /// </summary>
        public event Action<Exception> ErrorMsg;

        /// <summary>
        /// 串口在线状态更改事件,需要以 Open(bool) 激活
        /// </summary>
        public event EventHandler<OnlineStateEventArgs> OnlineStateEvent;


        /// <summary>
        /// 获取是否需要进行后台检测串口在线状态
        /// </summary>
        public bool CheckState { get; private set; }

        /// <summary>
        /// 尾
        /// </summary>
        public byte End { get; set; }

        /// <summary>
        /// 头
        /// </summary>
        public byte Head { get; set; }

        /// <summary>
        /// 当前使用的关键字
        /// </summary>
        public byte CurrentKeyWord { get; protected set; }

        /// <summary>
        /// 返回数据应由 头+关键字+数据量位+[数据主体]+和位+包尾组成,所以返回的数据个数必然大于等于5
        /// </summary>
        private readonly byte dataCount = 5;

        private readonly Stopwatch watch = new Stopwatch();


        /// <summary>
        /// 关闭
        /// </summary>
        public new void Close()
        {
            base.DataReceived -= SerialControl_DataReceived;
            if (CheckState)
                tokenSource.Cancel();

            //清除所有信息
            while (this.recDatas.TryDequeue(out _))
            {

            }

            base.Close();
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

        private CancellationTokenSource tokenSource;

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

            if (CheckState)
            {
                OnlineStateEvent?.Invoke(this, new OnlineStateEventArgs(OnlineState.Online));
                tokenSource = new CancellationTokenSource();

                _ = Task.Run(() =>
                {
                    Trace.WriteLine("开始线程检测");
                    while (!tokenSource.IsCancellationRequested)
                    {
                        if (!IsOpen)
                        {
                            OnlineStateEvent?.Invoke(this, new OnlineStateEventArgs(OnlineState.Unline));
                            break;
                        }

                        Thread.Sleep(10);
                    }
                    Trace.WriteLine("线程检测已断开");
                });
            }
        }

        /// <summary>
        /// 开启串口
        /// </summary>
        /// <param name="backCheck">是否在后台实时检测</param>
        public void Open(bool backCheck)
        {
            CheckState = backCheck;
            Open();
        }

        /// <summary>
        /// 发送并返回数据
        /// </summary>
        /// <param name="request">
        /// </param>
        /// <returns>
        /// </returns>
        public virtual SerialResponse Send(SerialRequest request)
        {

            if (!request.IsSetData)
                throw new Exception("不能发送没有设置数据的消息结构！");

            if (!IsOpen)
            {
                throw new Exception("串口未开启");
            }

            byte[] array = request.ToSendDatas();
            CurrentKeyWord = request.KeyWorld;

            //写入
            Write(array, 0, array.Length);

            if (!UnPackage(out List<byte> reponses))
                throw new Exception("返回错误不一致,可能是超时导致,返回数据为:" + reponses.FomartDatas());

            CheckPacketAllReady(reponses);
            int num = 0;
            if (reponses.Count > dataCount)
                num = reponses[2];
            return new SerialResponse(request, recDatas.ToArray(), (num == 0) ? null : reponses.GetRange(3, num).ToArray());
        }

        /// <summary>
        /// 进行解包操作
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        protected virtual bool UnPackage(out List<byte> list)
        {
            list = new List<byte>();
            watch.Restart();
            //循环读取
            byte sumCount = dataCount;//总的数据长度,包头+关键字+数据长度位+[数据]+和校验+包尾
            do
            {
                if (!IsOpen)
                {
                    try
                    {
                        Close();
                    }
                    catch (Exception)
                    {
                    }
                    throw new Exception("串口已经断开连接");
                }


                if (recDatas.TryDequeue(out byte b))
                {
                    //读取出来,如果是包头
                    if (b == Head && list.Count == 0)
                    {
                        list.Add(b);
                    }
                    else if (list.Count > 0)
                    {
                        list.Add(b);
                        if (list.Count == 3)
                        {
                            //写到第三个了,则查看数据长度
                            sumCount += list[2];//更新整体长度
                        }
                        else if (list.Count == sumCount)
                        {
                            break;
                        }
                    }
                    else
                    {
                        Trace.WriteLine("缓存中有冗余数据量 " + b);
                    }

                }
            } while (watch.ElapsedMilliseconds < ReadTimeout);

            watch.Stop();
            Trace.WriteLine($"解包时间:{watch.ElapsedMilliseconds} ms");
            return list.Count == sumCount;
        }

        /// <summary>
        /// 检测数据
        /// </summary>
        /// <param name="reponses"></param>
        /// <exception cref="Exception"></exception>
        protected virtual void CheckPacketAllReady(List<byte> reponses)
        {

            if (CurrentKeyWord != reponses[1])
                throw new Exception("返回关键字错误");

            byte b = 0;
            for (int i = 0; i < reponses.Count - 2; i++)
                b = (byte)(b + reponses[i]);

            if (b != reponses[reponses.Count - 2])
                throw new Exception("返回和校验错误");

            if (End != reponses.Last())
                throw new Exception("返回包属校验错误");
        }


        private void SerialControl_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                byte[] array = new byte[base.BytesToRead];
                _ = Read(array, 0, array.Length);
                byte[] array2 = array;
                foreach (byte b in array2)
                {
                    recDatas.Enqueue(b);
                }
            }
            catch (Exception ex)
            {
                ErrorMsg?.Invoke(ex);
            }
        }

    }
}