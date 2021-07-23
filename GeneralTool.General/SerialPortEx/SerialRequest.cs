﻿using System.Collections.Generic;
using System.Linq;

namespace GeneralTool.General.SerialPortEx
{
    /// <summary>
    /// 请求类
    /// </summary>
    public class SerialRequest
    {
        private List<byte> SendDatas = new List<byte>();

        /// <summary>
        /// 请求头
        /// </summary>
        public byte Head
        {
            get;
            private set;
        }

        /// <summary>
        /// 请求尾
        /// </summary>
        public byte End
        {
            get;
            private set;
        }

        /// <summary>
        /// 关键字
        /// </summary>
        public byte KeyWorld
        {
            get;
            private set;
        }

        /// <summary>
        /// 长度
        /// </summary>
        public int PacketDataLength
        {
            get;
            private set;
        }

        /// <summary>
        /// 是否在发送中
        /// </summary>
        public bool IsSetData
        {
            get;
            private set;
        } = false;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="packetHead"></param>
        /// <param name="packetEnd"></param>
        public SerialRequest(byte packetHead, byte packetEnd)
        {
            Head = packetHead;
            End = packetEnd;
            SendDatas.Add(Head);
        }

        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="keyWorld"></param>
        /// <param name="datas"></param>
        /// <returns></returns>
        public virtual bool SetData(byte keyWorld, ICollection<byte> datas)
        {
            if (IsSetData)
            {
                return false;
            }

            KeyWorld = keyWorld;
            SendDatas.Add(keyWorld);
            SendDatas.Add((byte)datas.Count());
            SendDatas.AddRange(datas);
            Parity();
            IsSetData = true;
            return true;
        }

        private void Parity()
        {
            byte b = 0;
            foreach (byte sendData in SendDatas)
            {
                b = (byte)(b + sendData);
            }

            SendDatas.Add(b);
            SendDatas.Add(End);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public byte[] ToSendDatas()
        {
            return SendDatas.ToArray();
        }
    }
}