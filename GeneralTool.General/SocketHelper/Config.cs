using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeneralTool.General.Interfaces;

namespace GeneralTool.General.SocketHelper
{
    public static class Config
    {
        public static IClientHelper ClientHelper { get; internal set; }

        public static void Register(IClientHelper helper = null)
        {
            if (helper == null)
            {
                ClientHelper = new ClientHelper();
            }
            else
            {
                ClientHelper = helper;
            }

        }
    }
}
