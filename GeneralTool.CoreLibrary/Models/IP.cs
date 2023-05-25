using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using GeneralTool.CoreLibrary.ExceptionHelper;

namespace GeneralTool.CoreLibrary.Models
{
    /// <summary>
    /// IP
    /// </summary>

    [Serializable]
    public class IP
    {
        #region Private 字段

        private readonly string ip = "127.0.0.1";

        #endregion Private 字段

        #region Public 构造函数

        /// <summary>
        /// </summary>
        /// <param name="ip">
        /// </param>
        public IP(string ip)
        {
            this.ip = this.ChangeToIp(ip);
        }

        #endregion Public 构造函数

        #region Public 方法

        /// <summary>
        /// </summary>
        /// <param name="ip">
        /// </param>
        public static implicit operator IP(string ip)
        {
            return new IP(ip);
        }

        /// <summary>
        /// </summary>
        /// <param name="ip">
        /// </param>
        public static implicit operator string(IP ip)
        {
            return ip.ip;
        }

        #endregion Public 方法

        #region Internal 方法

        internal string GetIp()
        {
            return this.ip;
        }

        #endregion Internal 方法

        #region Private 方法

        private string ChangeToIp(string ip)
        {
            List<string> list = ip.Split(new char[]
            {
                '.'
            }).ToList<string>();
            bool flag = list.Count != 4;
            if (flag)
            {
                throw new IPException(ip);
            }
            string result;
            try
            {
                var stringBuilder = new StringBuilder();
                foreach (string value in list)
                {
                    int num = Convert.ToInt32(value);
                    bool flag2 = num < 0 || num > 255;
                    if (flag2)
                    {
                        throw new IPException(ip);
                    }
                    stringBuilder.Append(num.ToString());
                    stringBuilder.Append(".");
                }
                stringBuilder.Remove(stringBuilder.Length - 1, 1);
                list.Clear();
                list = null;
                result = stringBuilder.ToString();
            }
            catch (Exception)
            {
                throw new IPException(ip);
            }
            return result;
        }

        #endregion Private 方法
    }
}