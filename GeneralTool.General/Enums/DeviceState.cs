using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneralTool.General.Enums
{
    /// <summary>
    /// Adb设备状态
    /// </summary>
    public enum AdbDeviceState
    {
        /// <summary>
        /// 
        /// </summary>
        Offline,
        /// <summary>
        /// 
        /// </summary>
        BootLoader,
        /// <summary>
        /// 
        /// </summary>
        Online,
        /// <summary>
        /// 
        /// </summary>
        Host,
        /// <summary>
        /// 
        /// </summary>
        Recovery,
        /// <summary>
        /// 
        /// </summary>
        NoPermissions,
        /// <summary>
        /// 
        /// </summary>
        Sideload,
        /// <summary>
        /// 
        /// </summary>
        Unauthorized,
        /// <summary>
        /// 
        /// </summary>
        Authorizing,
        /// <summary>
        /// 
        /// </summary>
        Unknown
    }
}
