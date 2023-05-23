using System;

using GeneralTool.CoreLibrary.Interfaces;
using GeneralTool.CoreLibrary.SocketLib.Interfaces;
using GeneralTool.CoreLibrary.SocketLib.Models;
using GeneralTool.CoreLibrary.SocketLib.Packages;

namespace GeneralTool.CoreLibrary.TaskLib
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
