using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace GeneralTool.CoreLibrary.MVS
{
    /// <summary>
    /// 
    /// </summary>
    public class MVSCamera : ICamera
    {
        private MVSCameraProvider.MV_CC_DEVICE_INFO_LIST m_stDeviceList;
        protected MVSCameraProvider M_MyCamera { get; private set; } = new MVSCameraProvider();
        private readonly IntPtr m_BufForDriver = IntPtr.Zero;
        private static readonly object BufForDriverLock = new object();

        private string _ip;

        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErroMsg { get; protected set; }

        /// <summary>
        /// 相机列表
        /// </summary>
        public List<MVSCameraProvider.MV_GIGE_DEVICE_INFO> GigeDevices { get; private set; }

        private Bitmap renderBitmap;

        /// <summary>
        /// 最大画幅
        /// </summary>
        public Size MaxSize { get; private set; }

        /// <summary>
        /// 是否开启了相机
        /// </summary>
        public bool IsOpen
        {
            get; protected set;
        }

        /// <summary>
        /// 枚举设备列表
        /// </summary>
        /// <returns></returns>
        public static List<string> EnumableDeviceList()
        {
            var devices = DeviceList();
            var list = new List<string>();
            foreach (var device in devices)
            {

                list.Add(ParseToIp(device.nCurrentIp));
            }

            return list;
        }

        /// <summary>
        /// 转换为IP地址
        /// </summary>
        /// <param name="nCurrentIp"></param>
        /// <returns></returns>
        public static string ParseToIp(uint nCurrentIp)
        {
            uint i1 = ((nCurrentIp) & 0xff000000) >> 24;
            uint i2 = ((nCurrentIp) & 0x00ff0000) >> 16;
            uint i3 = ((nCurrentIp) & 0x0000ff00) >> 8;
            uint i4 = ((nCurrentIp) & 0x000000ff);
            return $"{i1}.{i2}.{i3}.{i4}";
        }

        /// <summary>
        /// 转换为IP地址
        /// </summary>
        /// <param name="nCurrentIp"></param>
        /// <returns></returns>
        public static Tuple<uint, uint, uint, uint> ParseToIpEx(uint nCurrentIp)
        {
            uint i1 = ((nCurrentIp) & 0xff000000) >> 24;
            uint i2 = ((nCurrentIp) & 0x00ff0000) >> 16;
            uint i3 = ((nCurrentIp) & 0x0000ff00) >> 8;
            uint i4 = ((nCurrentIp) & 0x000000ff);
            return new Tuple<uint, uint, uint, uint>(i1, i2, i3, i4);
        }

        /// <summary>
        /// 获取所有的GIGE设备
        /// </summary>
        /// <returns></returns>
        public static List<MVSCameraProvider.MV_GIGE_DEVICE_INFO> DeviceList()
        {
            System.GC.Collect();

            var list = new List<MVSCameraProvider.MV_GIGE_DEVICE_INFO>();

            MVSCameraProvider.MV_CC_DEVICE_INFO_LIST m_stDeviceList = new MVSCameraProvider.MV_CC_DEVICE_INFO_LIST
            {
                nDeviceNum = 0
            };
            int nRet = MVSCameraProvider.MV_CC_EnumDevices_NET(MVSCameraProvider.MV_GIGE_DEVICE | MVSCameraProvider.MV_USB_DEVICE, ref m_stDeviceList);
            if (0 != nRet)
            {
                return new List<MVSCameraProvider.MV_GIGE_DEVICE_INFO>();
            }

            // ch:在窗体列表中显示设备名 | en:Display device name in the form list
            for (int i = 0; i < m_stDeviceList.nDeviceNum; i++)
            {
                MVSCameraProvider.MV_CC_DEVICE_INFO device = (MVSCameraProvider.MV_CC_DEVICE_INFO)Marshal.PtrToStructure(m_stDeviceList.pDeviceInfo[i], typeof(MVSCameraProvider.MV_CC_DEVICE_INFO));
                if (device.nTLayerType == MVSCameraProvider.MV_GIGE_DEVICE)
                {
                    var gigeInfo = (MVSCameraProvider.MV_GIGE_DEVICE_INFO)MVSCameraProvider.ByteToStruct(device.SpecialInfo.stGigEInfo, typeof(MVSCameraProvider.MV_GIGE_DEVICE_INFO));


                    list.Add(gigeInfo);
                }
            }

            return list;
        }

        /// <summary>
        /// 枚举设备列表
        /// </summary>
        public virtual int DeviceListAcq(string ip)
        {
            // ch:创建设备列表 | en:Create Device List
            System.GC.Collect();

            GigeDevices?.Clear();
            GigeDevices = new List<MVSCameraProvider.MV_GIGE_DEVICE_INFO>();

            m_stDeviceList.nDeviceNum = 0;
            int nRet = MVSCameraProvider.MV_CC_EnumDevices_NET(MVSCameraProvider.MV_GIGE_DEVICE | MVSCameraProvider.MV_USB_DEVICE, ref m_stDeviceList);
            if (0 != nRet)
            {
                //this.ErroMsg = "Enumerate devices fail!:"+ErrorCode.ErrorCodeInstance.GetErrorString(nRet);
                ShowErrorMsg("枚举设备列表时出错", nRet);
                return -1;
            }

            // ch:在窗体列表中显示设备名 | en:Display device name in the form list
            for (int i = 0; i < m_stDeviceList.nDeviceNum; i++)
            {
                MVSCameraProvider.MV_CC_DEVICE_INFO device = (MVSCameraProvider.MV_CC_DEVICE_INFO)Marshal.PtrToStructure(m_stDeviceList.pDeviceInfo[i], typeof(MVSCameraProvider.MV_CC_DEVICE_INFO));
                if (device.nTLayerType == MVSCameraProvider.MV_GIGE_DEVICE)
                {
                    MVSCameraProvider.MV_GIGE_DEVICE_INFO gigeInfo = (MVSCameraProvider.MV_GIGE_DEVICE_INFO)MVSCameraProvider.ByteToStruct(device.SpecialInfo.stGigEInfo, typeof(MVSCameraProvider.MV_GIGE_DEVICE_INFO));
                    GigeDevices.Add(gigeInfo);
                    string pip = ParseToIp(gigeInfo.nCurrentIp);
                    if (pip == ip)
                    {
                        return i;
                    }

                }
            }

            ErroMsg = $"没有找到IP为 [{ip}] 的相机";
            return -1;
        }

        /// <summary>
        /// 打开相机,以下标
        /// </summary>
        /// <param name="index">相机下标</param>
        /// <param name="exposureTime"></param>
        /// <returns></returns>

        public virtual bool Open(int index = 0, double exposureTime = -1)
        {
            if (IsOpen)
                return true;

            List<string> devices = EnumableDeviceList();
            if (devices == null || devices.Count == 0)
            {
                ErroMsg = "没有找到任何设备";
                return false;
            }

            if (devices.Count <= index)
            {
                ErroMsg = "找不到该下标的相机";
                return false;
            }
            string first = devices[index];
            return Open(first, exposureTime);
        }

        /// <summary>
        /// 打开相机,使用IP
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="exposureTime"></param>
        /// <returns></returns>
        public virtual bool Open(string ip, double exposureTime = -1)
        {
            if (string.IsNullOrWhiteSpace(ip))
            {
                ErroMsg = "没有设置IP地址";
                return false;
            }
            if (_ip != ip)
                Close();
            else if (IsOpen)
                return true;

            _ip = ip;

            if (GigeDevices == null)
                GigeDevices = new List<MVSCameraProvider.MV_GIGE_DEVICE_INFO>();

            int index = DeviceListAcq(ip);

            if (index < 0)
            {
                return false;
            }

            // ch:获取选择的设备信息 | en:Get selected device information
            MVSCameraProvider.MV_CC_DEVICE_INFO device =
                (MVSCameraProvider.MV_CC_DEVICE_INFO)Marshal.PtrToStructure(m_stDeviceList.pDeviceInfo[index], typeof(MVSCameraProvider.MV_CC_DEVICE_INFO));

            // ch:打开设备 | en:Open device
            if (null == M_MyCamera)
            {
                M_MyCamera = new MVSCameraProvider();
                if (null == M_MyCamera)
                {
                    ErroMsg = "无法初始化相机对象";
                    return false;
                }
            }

            int nRet = M_MyCamera.MV_CC_CreateDevice_NET(ref device);
            if (MVSCameraProvider.MV_OK != nRet)
            {
                //this.ErroMsg = "Can't CreateDevice";
                ShowErrorMsg("无法创建相机设备对象", nRet);
                return false;
            }

            nRet = M_MyCamera.MV_CC_OpenDevice_NET();
            if (MVSCameraProvider.MV_OK != nRet)
            {
                _ = M_MyCamera.MV_CC_DestroyDevice_NET();
                //this.ErroMsg = "Device open fail!" + nRet;
                ShowErrorMsg("无法打开相机", nRet);
                return false;
            }

            // ch:探测网络最佳包大小(只对GigE相机有效) | en:Detection network optimal package size(It only works for the GigE camera)
            if (device.nTLayerType == MVSCameraProvider.MV_GIGE_DEVICE)
            {
                int nPacketSize = M_MyCamera.MV_CC_GetOptimalPacketSize_NET();
                if (nPacketSize > 0)
                {
                    nRet = M_MyCamera.MV_CC_SetIntValue_NET("GevSCPSPacketSize", (uint)nPacketSize);
                    if (nRet != MVSCameraProvider.MV_OK)
                    {
                        // this.ErroMsg = "Set Packet Size failed!" + nRet;
                        ShowErrorMsg("设置PacketSize出错", nRet);
                        return false;
                    }
                }
                else
                {
                    ErroMsg = "获取 Packet Size 错误!" + nPacketSize;
                    return false;
                }
            }

            // ch:设置采集连续模式 | en:Set Continues Aquisition Mode
            _ = M_MyCamera.MV_CC_SetEnumValue_NET("AcquisitionMode", (uint)MVSCameraProvider.MV_CAM_ACQUISITION_MODE.MV_ACQ_MODE_CONTINUOUS);
            _ = M_MyCamera.MV_CC_SetEnumValue_NET("TriggerMode", (uint)MVSCameraProvider.MV_CAM_TRIGGER_MODE.MV_TRIGGER_MODE_OFF);
            _ = M_MyCamera.MV_CC_SetEnumValue_NET("TriggerMode", (uint)MVSCameraProvider.MV_CAM_TRIGGER_MODE.MV_TRIGGER_MODE_OFF);

            CameraRectangleInfo rect = GetCurrentAOISize();
            if (rect.IsEmpty)
                return false;

            MaxSize = new Size(rect.MaxWidth, rect.MaxHeight);

            if (exposureTime > 0)
            {
                //设置曝光
                _ = M_MyCamera.MV_CC_SetExposureTime_NET(Convert.ToSingle(exposureTime));
            }
            return StartGrab();
        }

        /// <summary>
        /// 开始采集
        /// </summary>
        /// <returns></returns>
        public virtual bool StartGrab()
        {
            // ch:标志位置位true | en:Set position bit true

            // ch:开始采集 | en:Start Grabbing
            int nRet = M_MyCamera.MV_CC_StartGrabbing_NET();
            if (MVSCameraProvider.MV_OK != nRet)
            {
                IsOpen = false;
                //this.ErroMsg = "Start Grabbing Fail!" + nRet;
                ShowErrorMsg("采集出错", nRet);
                Close();
                return false;
            }

            IsOpen = true;
            return true;
        }

        /// <summary>
        /// 停止采集
        /// </summary>
        public virtual void StopGrab()
        {
            // ch:标志位设为false | en:Set flag bit false

            // ch:停止采集 | en:Stop Grabbing
            int nRet = M_MyCamera.MV_CC_StopGrabbing_NET();
            if (nRet != MVSCameraProvider.MV_OK)
            {
                //ShowErrorMsg("Stop Grabbing Fail!", nRet);
            }
        }

        private void CreateBitmap(ref Bitmap bitmap, int width, int height, PixelFormat format)
        {
            bitmap?.Dispose();
            bitmap = new Bitmap(width, height, format);

            if (bitmap.PixelFormat == PixelFormat.Format8bppIndexed)
            {
                ColorPalette colorPalette = bitmap.Palette;
                for (int i = 0; i < 256; i++)
                {
                    colorPalette.Entries[i] = Color.FromArgb(i, i, i);
                }
                bitmap.Palette = colorPalette;
            }
        }
        private void UpdateBitmap(Bitmap bitmap, byte[] buffer, int width, bool color, int len)
        {
            BitmapData bmpData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, bitmap.PixelFormat);
            /* Get the pointer to the bitmap's buffer. */
            IntPtr ptrBmp = bmpData.Scan0;
            /* Compute the width of a line of the image data. */
            int imageStride = GetStride(width, color);
            /* If the widths in bytes are equal, copy in one go. */
            if (imageStride == bmpData.Stride)
            {
                System.Runtime.InteropServices.Marshal.Copy(buffer, 0, ptrBmp, len);
            }
            else /* The widths in bytes are not equal, copy line by line. This can happen if the image width is not divisible by four. */
            {
                for (int i = 0; i < bitmap.Height; ++i)
                {
                    Marshal.Copy(buffer, i * imageStride, new IntPtr(ptrBmp.ToInt64() + i * bmpData.Stride), width);
                }
            }
            /* Unlock the bits. */
            bitmap.UnlockBits(bmpData);
        }

        private PixelFormat GetFormat(bool color)
        {
            return color ? PixelFormat.Format24bppRgb : PixelFormat.Format8bppIndexed;
        }

        private int GetStride(int width, bool color)
        {
            return color ? width * 3 : width;
        }

        /// <summary>
        /// 获取图片
        /// </summary>
        /// <returns></returns>
        public virtual Bitmap GetBitmap()
        {
            #region New

            if (!IsOpen)
            {
                ErroMsg = "相机未开启或未开始采集";
                return null;
            }
            lock (M_MyCamera)
            {

                ErroMsg = "";
                MVSCameraProvider.MV_FRAME_OUT_INFO_EX stFrameInfoEx = new MVSCameraProvider.MV_FRAME_OUT_INFO_EX();

                byte[] buffer = new byte[5500 * 4000 * 3];

                try
                {
                    IntPtr PData = Marshal.UnsafeAddrOfPinnedArrayElement(buffer, 0);
                    int nRet2 = M_MyCamera.MV_CC_GetImageForBGR_NET(PData, (uint)buffer.Length, ref stFrameInfoEx, 1000);
                    if (nRet2 != MVSCameraProvider.MV_OK)
                    {
                        if (DeviceListAcq(_ip) < 0)
                        {
                            //查看是否有设备
                            IsOpen = false;
                        }
                        else
                        {
                            ShowErrorMsg("没有找到图像", nRet2);
                        }
                        return null;
                    }

                    int width = stFrameInfoEx.nWidth;
                    int height = stFrameInfoEx.nHeight;
                    bool rgbColor = true;
                    int stride = GetStride(width, rgbColor);
                    int len = (int)stFrameInfoEx.nFrameLen;
                    PixelFormat pixelFomart = GetFormat(rgbColor);
                    int bufferLen = stride * height;

                    if (renderBitmap == null || renderBitmap.Width != width || renderBitmap.Height != height)
                        CreateBitmap(ref renderBitmap, width, height, pixelFomart);
                    UpdateBitmap(renderBitmap, buffer, width, true, bufferLen);

                    Bitmap image = renderBitmap.Clone(new Rectangle(0, 0, width, height), pixelFomart);

                    return image;

                }
                catch (Exception ex)
                {
                    ErroMsg = ex + "";
                }
                finally
                {
                    //m_MyCamera.MV_CC_FreeImageBuffer_NET(ref stFrameInfo);
                }

                return null;
            }

            #endregion
        }

        /// <summary>
        /// 获取当前相机信息
        /// </summary>
        /// <returns></returns>
        public virtual CameraRectangleInfo GetCurrentAOISize()
        {
            MVSCameraProvider.MVCC_INTVALUE value = default;
            int code = M_MyCamera.MV_CC_GetWidth_NET(ref value);
            if (code != MVSCameraProvider.MV_OK)
            {
                ShowErrorMsg("获取宽度信息错误", code);
                return new CameraRectangleInfo();
            }

            uint maxWidth = value.nMax;
            uint curWidth = value.nCurValue;
            uint widthInc = value.nInc;

            code = M_MyCamera.MV_CC_GetHeight_NET(ref value);
            if (code != MVSCameraProvider.MV_OK)
            {
                ShowErrorMsg("获取高度信息错误", code);
                return new CameraRectangleInfo();
            }

            uint maxHeight = value.nMax;
            uint curHeight = value.nCurValue;
            uint heightInc = value.nInc;

            code = M_MyCamera.MV_CC_GetAOIoffsetX_NET(ref value);
            if (code != MVSCameraProvider.MV_OK)
            {
                ShowErrorMsg("获取AOI OffsetX信息错误", code);
                return new CameraRectangleInfo();
            }

            uint curOffX = value.nCurValue;
            uint curOffXInc = value.nInc;

            code = M_MyCamera.MV_CC_GetAOIoffsetY_NET(ref value);
            if (code != MVSCameraProvider.MV_OK)
            {
                ShowErrorMsg("获取AOI OffsetY信息错误", code);
                return new CameraRectangleInfo();
            }

            uint curOffY = value.nCurValue;
            uint curOffYInc = value.nInc;
            maxWidth += curOffX;
            maxHeight += curOffY;
            return new CameraRectangleInfo()
            {
                CurrentHeight = (int)curHeight,
                CurrentWidth = (int)curWidth,
                MaxHeight = (int)maxHeight,
                MaxWidth = (int)maxWidth,
                OffsetX = (int)curOffX,
                OffsetY = (int)curOffY,
                HeightInc = (int)heightInc,
                WidthInc = (int)widthInc,
                OffXInc = (int)curOffXInc,
                OffYInc = (int)curOffYInc,
            };
        }

        /// <summary>
        /// 恢复最大画幅
        /// </summary>
        public virtual void RestoryMaxROI()
        {
            _ = SetMaxAOI(true);
        }

        /// <summary>
        /// 更新大小
        /// </summary>
        /// <param name="rect"></param>
        /// <returns></returns>
        public virtual Rectangle ParseAOI(Rectangle rect)
        {
            //查看当前Rect的大小是否与最大值一样
            if (rect.Width == MaxSize.Width && rect.Height == MaxSize.Height)
            {
                //已经OK了
                return new Rectangle(0, 0, rect.Width, rect.Height);
            }

            #region New
            CameraRectangleInfo info = GetCurrentAOISize();
            Point p1 = new Point(rect.X, rect.Y);
            int offXInc = info.OffXInc;
            int offYInc = info.OffYInc;

            //将其扩大
            //计算左上角偏移点
            int xResult = p1.X - p1.X % offXInc;
            xResult = xResult < 0 ? 0 : xResult;

            int yResult = p1.Y - p1.Y % offYInc;
            yResult = yResult < 0 ? 0 : yResult;

            int widthInc = info.WidthInc;
            int heightInc = info.HeightInc;

            //计算宽
            int width = rect.Width + p1.X % offXInc;//如果左顶点x往左偏移过,则可以加大点
            int widthResult = width - width % widthInc ;
            if (widthResult + xResult > MaxSize.Width)
            {
                //如果大于最大宽度,则将宽度减小
                widthResult = MaxSize.Width - xResult;
            }

            //计算高
            int height = rect.Height + p1.Y % offYInc;
            int heightResult = height - height % heightInc ;
            if (heightResult + yResult > MaxSize.Height)
            {
                heightResult = MaxSize.Height - yResult;
            }

            return new Rectangle(xResult, yResult, widthResult, heightResult);

            #endregion

        }

        /// <summary>
        /// 设置ROI
        /// </summary>
        /// <param name="rect"></param>
        /// <returns></returns>
        public virtual bool SetAOI(ref Rectangle rect)
        {
            //查看当前Rect的大小是否与最大值一样
            if (rect.Width == MaxSize.Width && rect.Height == MaxSize.Height)
            {
                //已经OK了
                return true;
            }

            lock (M_MyCamera)
            {
                //先停止采集
                StopGrab();
                //将其设置到最大值
                bool re = SetMaxAOI();
                if (!re) return false;

                rect = ParseAOI(rect);

                _ = M_MyCamera.MV_CC_SetWidth_NET((uint)rect.Width);

                _ = M_MyCamera.MV_CC_SetHeight_NET((uint)rect.Height);

                _ = M_MyCamera.MV_CC_SetAOIoffsetX_NET((uint)rect.Left);

                _ = M_MyCamera.MV_CC_SetAOIoffsetY_NET((uint)rect.Top);

                //检验是因为更换相机后造的
                CameraRectangleInfo resultRect = GetCurrentAOISize();
                rect = resultRect.ToRectangle();
                re = StartGrab();
                return re;
            }
            
        }

        /// <summary>
        /// 设置最大ROI
        /// </summary>
        /// <param name="autoGrab"></param>
        /// <returns></returns>
        public virtual bool SetMaxAOI(bool autoGrab = false)
        {
            if (IsOpen)
            {
                StopGrab();
            }

            int code = M_MyCamera.MV_CC_SetAOIoffsetX_NET(0);
            if (code != MVSCameraProvider.MV_OK)
            {
                ShowErrorMsg("重置OffsetX错误", code);
                return false;
            }

            code = M_MyCamera.MV_CC_SetAOIoffsetY_NET(0);
            if (code != MVSCameraProvider.MV_OK)
            {
                ShowErrorMsg("重置OffsetY错误", code);
                return false;
            }

            code = M_MyCamera.MV_CC_SetWidth_NET((uint)MaxSize.Width);
            if (code != MVSCameraProvider.MV_OK)
            {
                ShowErrorMsg("重置Width错误", code);
                return false;
            }

            code = M_MyCamera.MV_CC_SetHeight_NET((uint)MaxSize.Height);
            if (code != MVSCameraProvider.MV_OK)
            {
                ShowErrorMsg("重置Height错误", code);
                return false;
            }

            return !autoGrab || StartGrab();
        }

        /// <summary>
        /// 关闭相机
        /// </summary>
        public virtual void Close()
        {
            ErroMsg = "";
            try
            {

                lock (BufForDriverLock)
                {
                    if (m_BufForDriver != IntPtr.Zero)
                    {
                        _ = Marshal.Release(m_BufForDriver);
                    }
                }

            }
            catch (Exception ex)
            {
                ErroMsg = ex + "";
            }
            // ch:关闭设备 | en:Close Device
            try
            {
                _ = M_MyCamera.MV_CC_CloseDevice_NET();
                _ = M_MyCamera.MV_CC_DestroyDevice_NET();
            }
            catch (Exception)
            {

            }
            renderBitmap?.Dispose();
            renderBitmap = null;
            IsOpen = false;
        }

        public virtual void ShowErrorMsg(string csMessage, int nErrorNum)
        {
            string errorMsg = nErrorNum == 0 ? csMessage : csMessage + ": Error =" + string.Format("{0:X}", nErrorNum);
            errorMsg += ErrorCode.ErrorCodeInstance.GetErrorString(nErrorNum);
            ErroMsg = errorMsg;
        }

        /// <summary>
        /// 获取曝光信息
        /// </summary>
        /// <returns></returns>
        public virtual CameraExposureTimeInfo GetExposureTime()
        {
            MVSCameraProvider.MVCC_FLOATVALUE value = new MVSCameraProvider.MVCC_FLOATVALUE();
            _ = M_MyCamera.MV_CC_GetExposureTime_NET(ref value);
            CameraExposureTimeInfo info = new CameraExposureTimeInfo()
            {
                CurrentValue = value.fCurValue,
                MaxValue = value.fMax,
                MinValue = value.fMin,
            };
            return info;
        }

        /// <summary>
        /// 设置曝光
        /// </summary>
        /// <param name="time"></param>
        public virtual void SetExposureTime(double time)
        {
            //应对JAI的相机,曝光必须要先停止采集
            lock (M_MyCamera)
            {
                StopGrab();
                _ = M_MyCamera.MV_CC_SetExposureTime_NET(Convert.ToSingle(time));
                _ = StartGrab(); 
            }
        }
    }
}
