using System;
using System.Text.RegularExpressions;

using GeneralTool.CoreLibrary.Enums;

namespace GeneralTool.CoreLibrary.Models
{
    /// <summary>
    /// adb查询到的设备,直接copy的 SharpAdbClient
    /// </summary>
    public class AdbDeviceData
    {
        internal const string DeviceDataRegexString = "^(?<serial>[a-zA-Z0-9_-]+(?:\\s?[\\.a-zA-Z0-9_-]+)?(?:\\:\\d{1,})?)\\s+(?<state>device|connecting|offline|unknown|bootloader|recovery|download|authorizing|unauthorized|host|no permissions)(?<message>.*?)(\\s+usb:(?<usb>[^:]+))?(?:\\s+product:(?<product>[^:]+))?(\\s+model\\:(?<model>[\\S]+))?(\\s+device\\:(?<device>[\\S]+))?(\\s+features:(?<features>[^:]+))?(\\s+transport_id:(?<transport_id>[^:]+))?$";

        private static readonly Regex Regex = new Regex("^(?<serial>[a-zA-Z0-9_-]+(?:\\s?[\\.a-zA-Z0-9_-]+)?(?:\\:\\d{1,})?)\\s+(?<state>device|connecting|offline|unknown|bootloader|recovery|download|authorizing|unauthorized|host|no permissions)(?<message>.*?)(\\s+usb:(?<usb>[^:]+))?(?:\\s+product:(?<product>[^:]+))?(\\s+model\\:(?<model>[\\S]+))?(\\s+device\\:(?<device>[\\S]+))?(\\s+features:(?<features>[^:]+))?(\\s+transport_id:(?<transport_id>[^:]+))?$", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        /// <summary>
        /// 设备序列号
        /// </summary>
        public string Serial
        {
            get;
            set;
        }

        /// <summary>
        /// 设备当前状态
        /// </summary>
        public AdbDeviceState State
        {
            get;
            set;
        }

        /// <summary>
        /// 设备名称
        /// </summary>
        public string Model
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string Product
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string Features
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string Usb
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string TransportId
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string Message
        {
            get;
            set;
        }

        /// <summary>
        /// 获取设备
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static AdbDeviceData CreateFromAdbData(string data)
        {
            Match match = Regex.Match(data);
            return match.Success
                ? new AdbDeviceData
                {
                    Serial = match.Groups["serial"].Value,
                    State = GetStateFromString(match.Groups["state"].Value),
                    Model = match.Groups["model"].Value,
                    Product = match.Groups["product"].Value,
                    Name = match.Groups["device"].Value,
                    Features = match.Groups["features"].Value,
                    Usb = match.Groups["usb"].Value,
                    TransportId = match.Groups["transport_id"].Value,
                    Message = match.Groups["message"].Value
                }
                : throw new ArgumentException("Invalid device list data '" + data + "'");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Serial;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        internal static AdbDeviceState GetStateFromString(string state)
        {
            AdbDeviceState result;
            if (string.Equals(state, "device", StringComparison.OrdinalIgnoreCase))
            {
                result = AdbDeviceState.Online;
            }
            else if (string.Equals(state, "no permissions", StringComparison.OrdinalIgnoreCase))
            {
                result = AdbDeviceState.NoPermissions;
            }
            else if (!Enum.TryParse(state, ignoreCase: true, out result))
            {
                result = AdbDeviceState.Unknown;
            }

            return result;
        }
    }
}
