using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneralTool.CoreLibrary.Extensions
{
    /// <summary>
    /// 一些帮助方法
    /// </summary>
    public static class AppHelper
    {
        /// <summary>
        /// 调用GC方法
        /// </summary>
        public static void GCMeomery()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
    }
}
