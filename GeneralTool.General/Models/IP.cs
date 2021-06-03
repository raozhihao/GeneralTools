using GeneralTool.General.ExceptionHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeneralTool.General.Models
{
    /// <summary>
    /// IP
    /// </summary>
    public class IP
    {
        internal string getIp()
        {
            return this.ip;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ip"></param>
        public IP(string ip)
        {
            this.ip = this.changeToIp(ip);
        }

        private string changeToIp(string ip)
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
                StringBuilder stringBuilder = new StringBuilder();
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ip"></param>
        public static implicit operator IP(string ip)
        {
            return new IP(ip);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ip"></param>
        public static implicit operator string(IP ip)
        {
            return ip.ip;
        }

        private string ip = "127.0.0.1";
    }
}
