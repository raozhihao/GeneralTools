namespace GeneralTool.CoreLibrary.MVS
{
    /// <summary>
    /// 相机曝光信息
    /// </summary>
    public struct CameraExposureTimeInfo
    {
        /// <summary>
        /// 曝光最大值
        /// </summary>
        public float MaxValue { get; set; }

        /// <summary>
        /// 曝光最小值
        /// </summary>
        public float MinValue { get; set; }

        /// <summary>
        /// 当前曝光值
        /// </summary>
        public float CurrentValue { get; set; }
    }
}