using System;
using System.Net.Sockets;

namespace GeneralTool.General.TaskLib
{
    /// <summary>
    /// 
    /// </summary>
    public class RecDataObject : IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Socket"></param>
        /// <param name="Datas"></param>
        /// <param name="StringDatas"></param>
        internal RecDataObject(Socket Socket, byte[] Datas, string StringDatas)
        {
            this.Socket = Socket;
            this.Datas = Datas;
            this.StringDatas = StringDatas;
        }

        /// <summary>
        /// 
        /// </summary>
        ~RecDataObject()
        {
            this.Dispose();
        }

        /// <summary>
        /// 
        /// </summary>
        internal void DelEndString()
        {
            this.StringDatas = this.StringDatas.Split(new char[1])[0];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            bool flag = !this.disposedValue;
            if (flag)
            {
                if (disposing)
                {
                    this.Datas = null;
                    this.StringDatas = null;
                }
                this.disposedValue = true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 
        /// </summary>
        public Socket Socket;

        /// <summary>
        /// 
        /// </summary>
        public byte[] Datas;

        /// <summary>
        /// 
        /// </summary>
        public string StringDatas;

        /// <summary>
        /// 
        /// </summary>
        private bool disposedValue = false;
    }
}
