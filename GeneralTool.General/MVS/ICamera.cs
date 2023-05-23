using System.Drawing;

namespace GeneralTool.General.MVS
{
    /// <summary>
    /// 相机接口
    /// </summary>
    public interface ICamera
    {
        /// <summary>
        /// 错误信息
        /// </summary>
        string ErroMsg { get; }

        /// <summary>
        /// 指示相机是否已经开启
        /// </summary>
        bool IsOpen { get; }

        /// <summary>
        /// 相机的最大分辨率信息
        /// </summary>
        Size MaxSize { get; }



        /// <summary>
        /// 关闭相机
        /// </summary>
        void Close();

        /// <summary>
        /// 获取单张图片
        /// </summary>
        /// <returns></returns>
        Bitmap GetBitmap();

        /// <summary>
        /// 获取当前ROI/AOI信息
        /// </summary>
        /// <returns></returns>
        CameraRectangleInfo GetCurrentAOISize();


        /// <summary>
        /// 打开指定的相机
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="exposureTime">相机的初始设置曝光</param>
        /// <returns></returns>
        bool Open(string ip, double exposureTime);


        /// <summary>
        /// 打开指定的相机
        /// </summary>
        /// <param name="index"></param>
        /// <param name="exposureTime">相机的初始设置曝光</param>
        /// <returns></returns>
        bool Open(int index = 0, double exposureTime = -1);

        /// <summary>
        /// 获取曝光
        /// </summary>
        /// <returns></returns>
        CameraExposureTimeInfo GetExposureTime();


        /// <summary>
        /// 设置曝光
        /// </summary>
        /// <returns></returns>
        void SetExposureTime(double time);

        /// <summary>
        /// 设置ROI/AOI
        /// </summary>
        /// <param name="rect">需要设置的范围</param>
        /// <returns>返回设置成功与否以及设置完后的真正范围</returns>
        bool SetAOI(ref Rectangle rect);


        /// <summary>
        /// 开始采集
        /// </summary>
        /// <returns></returns>
        bool StartGrab();

        /// <summary>
        /// 停止采集
        /// </summary>
        void StopGrab();

        /// <summary>
        /// 恢复到最大画幅
        /// </summary>
        void RestoryMaxROI();

        /// <summary>
        /// 换算当前将要设置的矩形
        /// </summary>
        /// <param name="rect"></param>
        /// <returns></returns>
        Rectangle ParseAOI(Rectangle rect);

    }
}