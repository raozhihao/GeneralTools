using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Threading;

namespace GeneralTool.General.SerialPortEx
{
    /// <summary>
    /// SerialPort扩展类
    /// </summary>
    public class SerialControl : SerialPort
    {
        #region Private 字段

        private readonly List<byte> recDatas = new List<byte>();

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

        #endregion Public 事件

        #region Public 属性

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
        }

        /// <summary>
        /// 发关并返回数据
        /// </summary>
        /// <param name="request">
        /// </param>
        /// <returns>
        /// </returns>
        public SerialResponse Send(SerialRequest request)
        {
            if (!request.IsSetData)
            {
                throw new Exception("不能发送没有设置数据的消息结构！");
            }

            recDatas.Clear();
            byte[] array = request.ToSendDatas();
            CurrentRequest = request;
            Write(array, 0, array.Length);
            RecEvent.WaitOne(base.ReadTimeout);
            int num = 0;
            if (recDatas.Count() > 3)
            {
                num = recDatas[2];
            }

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
            {
                return false;
            }

            if (CurrentRequest.Head != recDatas.First())
            {
                return false;
            }

            if (CurrentRequest.KeyWorld != recDatas[1])
            {
                return false;
            }

            int num = recDatas[2];
            if (recDatas.Count < 5 + num)
            {
                return false;
            }

            if (recDatas.Count == 5 + num)
            {
                byte b = 0;
                for (int i = 0; i < recDatas.Count - 2; i++)
                {
                    b = (byte)(b + recDatas[i]);
                }

                if (b != recDatas[recDatas.Count - 2])
                {
                    return false;
                }

                if (CurrentRequest.End != recDatas.Last())
                {
                    return false;
                }

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