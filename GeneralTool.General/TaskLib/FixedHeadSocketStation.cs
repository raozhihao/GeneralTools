using System;

using GeneralTool.General.Interfaces;
using GeneralTool.General.SocketLib.Interfaces;
using GeneralTool.General.SocketLib.Models;
using GeneralTool.General.SocketLib.Packages;

namespace GeneralTool.General.TaskLib
{
    /// <summary>
    /// 固定包头服务端
    /// </summary>
    public class FixedHeadSocketStation : GenServerStation<FixedHeadRecevieState>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="jsonConvert"></param>
        /// <param name="log"></param>
        public FixedHeadSocketStation(IJsonConvert jsonConvert, ILog log) : base(jsonConvert, log, new Func<IPackage<FixedHeadRecevieState>>(() => new FixedHeadPackage()))
        {

        }
    }
}
