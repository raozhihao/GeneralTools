using System;
using System.Net.Sockets;

namespace GeneralTool.CoreLibrary.TaskLib
{
    /// <summary>
    /// </summary>
    public class RecDataObject : IDisposable
    {
        #region Public 字段

        /// <summary>
        /// </summary>
        public byte[] Datas;

        /// <summary>
        /// </summary>
        public Socket Socket;

        /// <summary>
        /// </summary>
        public string StringDatas;

        #endregion Public 字段

        #region Private 字段

        /// <summary>
        /// </summary>
        private bool disposedValue = false;

        #endregion Private 字段

        #region Internal 构造函数

        /// <summary>
        /// </summary>
        /// <param name="Socket">
        /// </param>
        /// <param name="Datas">
        /// </param>
        /// <param name="StringDatas">
        /// </param>
        internal RecDataObject(Socket Socket, byte[] Datas, string StringDatas)
        {
            this.Socket = Socket;
            this.Datas = Datas;
            this.StringDatas = StringDatas;
        }

        #endregion Internal 构造函数

        #region Private 析构函数

        /// <summary>
        /// </summary>
        ~RecDataObject()
        {
            this.Dispose();
        }

        #endregion Private 析构函数

        #region Public 方法

        /// <summary>
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion Public 方法

        #region Internal 方法

        /// <summary>
        /// </summary>
        internal void DelEndString()
        {
            this.StringDatas = this.StringDatas.Split(new char[1])[0];
        }

        #endregion Internal 方法

        #region Protected 方法

        /// <summary>
        /// </summary>
        /// <param name="disposing">
        /// </param>
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

        #endregion Protected 方法
    }
}