using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using GeneralTool.CoreLibrary.Extensions;

namespace GeneralTool.CoreLibrary.SerialPortEx
{
    public class SerialPortAsync : SerialPort
    {

        private List<byte> currentDatas = new List<byte>();

        public event Action<List<byte>> OnDataReceived;

        /// <summary>
        /// 返回出错事件
        /// </summary>
        public event Action<Exception> ErrorMsg;

        /// <summary>
        /// 尾
        /// </summary>
        public byte End { get; set; }

        /// <summary>
        /// 头
        /// </summary>
        public byte Head { get; set; }

        /// <summary>
        /// 返回数据应由 头+关键字+数据量位+[数据主体]+和位+包尾组成,所以返回的数据个数必然大于等于5
        /// </summary>
        private readonly byte dataCount = 5;

     

        /// <summary>
        /// 关闭
        /// </summary>
        public new void Close()
        {
            base.DataReceived -= SerialControl_DataReceived;
            base.Close();
            this.packThread?.Join();
            this.currentDatas.Clear();
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

        private Thread packThread;

        /// <summary>
        /// 开启
        /// </summary>
        public new void Open()
        {
            if (base.IsOpen)
                return;

            //this.ReadBufferSize = 2147483647;
            base.DataReceived += SerialControl_DataReceived;
            this.packThread = new Thread(LoopPackMethod) { IsBackground = true };

            base.Open();

            this.packThread.Start();
        }

        private void LoopPackMethod()
        {
            while (this.IsOpen)
            {
                this.UnPackage();
            }
        }

        private SpinLock spinlock = new SpinLock(true);

        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="request">
        /// </param>
        /// <returns>
        /// </returns>
        public virtual void Send(SerialRequest request)
        {

            if (!request.IsSetData)
                throw new Exception("不能发送没有设置数据的消息结构！");

            if (!IsOpen)
            {
                throw new Exception("串口未开启");
            }

            byte[] array = request.ToSendDatas();

            var lockTaken = false;

            try
            {
                while (lockTaken == false)
                {
                    spinlock.Enter(ref lockTaken);
                }
                Write(array, 0, array.Length);
            }
            finally
            {
                if (lockTaken)
                    spinlock.Exit();
            }
        }


        private void SerialControl_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {

                byte[] array = new byte[base.BytesToRead];
                var len = Read(array, 0, array.Length);

               // Trace.WriteLine($"===============    {array.FomartDatas()}     Thread ID: {Thread.CurrentThread.ManagedThreadId}   ========================");
                this.currentDatas.AddRange(array);
               // Trace.WriteLine($"--------------- 还有包缓存字节数：{this.currentDatas.Count}");
            }
            catch (Exception ex)
            {
                ErrorMsg?.Invoke(ex);
            }
        }

        /// <summary>
        /// 进行解包操作
        /// </summary>
        /// <returns></returns>
        protected virtual void UnPackage()
        {
            if (!IsOpen)
            {
                return;
            }

            if (!this.CheckDatas())
                return;

            //检查最小数据长度 包头+关键字+数据长度+[数据]+和校验+包尾 最小五位
            if (this.currentDatas.Count < this.dataCount)
                return;//不是完整的包,不用管

            //读取数据位
            var dataLen = this.currentDatas[2];
            //总共的数量由前三位+数据长度+后两位
            var sum = dataCount + dataLen;
            if (this.currentDatas.Count < sum)
                return;//不足数据 有5位是必要的,再加上数据长度才能拼成一个完整的包

            //拿取完整的包,将包放到缓冲区中,由外部去处理
            var getRange = this.currentDatas.GetRange(0, sum);

            this.currentDatas.RemoveRange(0, sum);

            //Trace.WriteLine($"+++++++++++  解出完整包，发出..............  [{getRange.FomartDatas()}]");
            ThreadPool.QueueUserWorkItem((o) =>
            {
                this.OnDataReceived?.Invoke(o as List<byte>);
            }, getRange);
        }

        private bool CheckDatas()
        {
            //检查最小数据长度 包头+关键字+数据长度+[数据]+和校验+包尾 最小五位 + 最少两位数据
            if (this.currentDatas.Count < this.dataCount + 2)
                return false;//不是完整的包,不用管

            //找出包头
            var index = this.currentDatas.IndexOf(this.Head);
            if (index == 0)
                return true;

            if (index == -1)
                return false;//没有找到包头

            //当前包头在后面,切掉之前的数据
            this.currentDatas.RemoveRange(0, index + 1);
            return true;
        }

        /// <summary>
        /// 获取用户数据并检验数据包
        /// </summary>
        /// <param name="keyword">关键字</param>
        /// <param name="packet">所有包</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public byte[] UnPacket(byte keyword, byte[] packet)
        {
            if (packet.Length < 5) throw new Exception("数据长度不正确");

            //校验头
            if (packet[0] != this.Head) throw new Exception("包头不正确");
            if (packet[1] != keyword) throw new Exception("关键字不正确");
            if (packet.Last() != this.End) throw new Exception("包尾不正确");

            //数据长度校验
            var dataLen = packet[2];
            var sum = this.dataCount + dataLen;
            if (sum != packet.Length) throw new Exception("数据长度不正确");

            //和校验
            byte b = 0;
            for (int i = 0; i < packet.Length - 2; i++)
                b = (byte)(b + packet[i]);

            if (b != packet[packet.Length - 2])
                throw new Exception("返回和校验错误");

            var receBuffer = new byte[dataLen];
            Array.Copy(packet, 3, receBuffer, 0, dataLen);
            return receBuffer;
        }
    }
}
