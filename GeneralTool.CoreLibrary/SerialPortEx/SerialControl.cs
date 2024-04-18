
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using GeneralTool.CoreLibrary.Extensions;
using GeneralTool.CoreLibrary.Interfaces;
using GeneralTool.CoreLibrary.Logs;

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
                return;
            }

            base.DataReceived += SerialControl_DataReceived;
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
                return new SerialResponse(request, recDatas.ToArray(), null);

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
            var datas = reponses.FomartDatas();
            if (CurrentKeyWord != reponses[1])
                throw new Exception("返回关键字错误:" + datas);

            byte b = 0;
            for (int i = 0; i < reponses.Count - 2; i++)
                b = (byte)(b + reponses[i]);

            if (b != reponses[reponses.Count - 2])
                throw new Exception("返回和校验错误:" + datas);

            if (End != reponses.Last())
                throw new Exception("返回包属校验错误:" + datas);
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

        /// <summary>
        /// 发送指令
        /// </summary>
        /// <param name="keyword">关键字</param>
        /// <param name="data">数据</param>
        /// <param name="connectAction">连接函数</param>
        /// <param name="closeAction">关闭函数</param>
        /// <param name="validateAction">校验函数</param>
        /// <param name="log">日志对象</param>
        /// <returns></returns>
        public byte[] SimpleBatchCommand(byte keyword, byte[] data, Action connectAction = null, Action closeAction = null, Action<byte, byte[]> validateAction = null, ILog log = null)
        {
            if (log == null) log = new ConsoleLogInfo();

            connectAction?.Invoke();

            SerialRequest request = this.CreateRequest();
            if (data == null || data.Length == 0) data = new byte[] { };
            _ = request.SetData(keyword, data);

            string sendDataStr = request.ToSendDatas().FomartDatas();
            log.Debug($"发送命令[{keyword} - {keyword}],发送命令数据:{sendDataStr}");

            SerialResponse reponse = this.Send(request);

            byte[] recDatas = reponse.UserDatas;

            if (recDatas != null)
            {
                string reponseDataStr = recDatas.FomartDatas();
                log.Debug($"命令[{keyword} - {keyword}] 返回:{reponseDataStr}");
            }
            else
            {
                log.Debug($"命令[{keyword} - {keyword}] 返回:null");
            }

            try
            {
                ValidateResult(keyword, recDatas, validateAction);
            }
            finally
            {
                closeAction?.Invoke();
            }
            return recDatas;
        }


        public static void ValidateResult(byte keyword, byte[] result, Action<byte, byte[]> validateAction = null)
        {
            if (result == null)
                throw new Exception("没有接收到任何数据,请检查端口连接");

            if (result.Length == 0)
                throw new Exception("接收到空的数据");

            validateAction?.Invoke(keyword, result);
        }

    }
}