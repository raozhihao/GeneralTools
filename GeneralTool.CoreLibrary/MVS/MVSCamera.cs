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
        MVSCameraProvider.MV_CC_DEVICE_INFO_LIST m_stDeviceList;
        private MVSCameraProvider m_MyCamera = new MVSCameraProvider();
       
        readonly IntPtr m_BufForDriver = IntPtr.Zero;
        private static readonly object BufForDriverLock = new object();

        private string _ip;

        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErroMsg { get; private set; }

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
            get; private set;
        }

        /// <summary>
        /// 枚举设备列表
        /// </summary>
        /// <returns></returns>
        public static List<string> EnumableDeviceList()
        {
            System.GC.Collect();

            var list = new List<string>();

            var m_stDeviceList = new MVSCameraProvider.MV_CC_DEVICE_INFO_LIST
            {
                nDeviceNum = 0
            };
            int nRet = MVSCameraProvider.MV_CC_EnumDevices_NET(MVSCameraProvider.MV_GIGE_DEVICE | MVSCameraProvider.MV_USB_DEVICE, ref m_stDeviceList);
            if (0 != nRet)
            {
                return new List<string>();
            }

            // ch:在窗体列表中显示设备名 | en:Display device name in the form list
            for (int i = 0; i < m_stDeviceList.nDeviceNum; i++)
            {
                MVSCameraProvider.MV_CC_DEVICE_INFO device = (MVSCameraProvider.MV_CC_DEVICE_INFO)Marshal.PtrToStructure(m_stDeviceList.pDeviceInfo[i], typeof(MVSCameraProvider.MV_CC_DEVICE_INFO));
                if (device.nTLayerType == MVSCameraProvider.MV_GIGE_DEVICE)
                {
                    MVSCameraProvider.MV_GIGE_DEVICE_INFO gigeInfo = (MVSCameraProvider.MV_GIGE_DEVICE_INFO)MVSCameraProvider.ByteToStruct(device.SpecialInfo.stGigEInfo, typeof(MVSCameraProvider.MV_GIGE_DEVICE_INFO));

                    var ip = ParseToIp(gigeInfo.nCurrentIp);

                    list.Add(ip);
                }
            }


            return list;
        }

        private static string ParseToIp(uint nCurrentIp)
        {
            var i1 = ((nCurrentIp) & 0xff000000) >> 24;
            var i2 = ((nCurrentIp) & 0x00ff0000) >> 16;
            var i3 = ((nCurrentIp) & 0x0000ff00) >> 8;
            var i4 = ((nCurrentIp) & 0x000000ff);
            return $"{i1}.{i2}.{i3}.{i4}";
        }

        /// <summary>
        /// 枚举设备列表
        /// </summary>
        public int DeviceListAcq(string ip)
        {
            // ch:创建设备列表 | en:Create Device List
            System.GC.Collect();

            this.GigeDevices?.Clear();
            this.GigeDevices = new List<MVSCameraProvider.MV_GIGE_DEVICE_INFO>();

            m_stDeviceList.nDeviceNum = 0;
            int nRet = MVSCameraProvider.MV_CC_EnumDevices_NET(MVSCameraProvider.MV_GIGE_DEVICE | MVSCameraProvider.MV_USB_DEVICE, ref m_stDeviceList);
            if (0 != nRet)
            {
                //this.ErroMsg = "Enumerate devices fail!:"+ErrorCode.ErrorCodeInstance.GetErrorString(nRet);
                this.ShowErrorMsg("枚举设备列表时出错", nRet);
                return -1;
            }

            // ch:在窗体列表中显示设备名 | en:Display device name in the form list
            for (int i = 0; i < m_stDeviceList.nDeviceNum; i++)
            {
                MVSCameraProvider.MV_CC_DEVICE_INFO device = (MVSCameraProvider.MV_CC_DEVICE_INFO)Marshal.PtrToStructure(m_stDeviceList.pDeviceInfo[i], typeof(MVSCameraProvider.MV_CC_DEVICE_INFO));
                if (device.nTLayerType == MVSCameraProvider.MV_GIGE_DEVICE)
                {
                    MVSCameraProvider.MV_GIGE_DEVICE_INFO gigeInfo = (MVSCameraProvider.MV_GIGE_DEVICE_INFO)MVSCameraProvider.ByteToStruct(device.SpecialInfo.stGigEInfo, typeof(MVSCameraProvider.MV_GIGE_DEVICE_INFO));
                    this.GigeDevices.Add(gigeInfo);
                    var pip = ParseToIp(gigeInfo.nCurrentIp);
                    if (pip == ip)
                    {
                        return i;
                    }

                }
            }

            this.ErroMsg = $"没有找到IP为 [{ip}] 的相机";
            return -1;
        }

        /// <summary>
        /// 打开相机,以下标
        /// </summary>
        /// <param name="index">相机下标</param>
        /// <param name="exposureTime"></param>
        /// <returns></returns>

        public bool Open(int index = 0, double exposureTime = -1)
        {
            if (this.IsOpen )
                return true;

            var devices = EnumableDeviceList();
            if (devices == null || devices.Count == 0)
            {
                this.ErroMsg = "没有找到任何设备";
                return false;
            }

            if (devices.Count <= index)
            {
                this.ErroMsg = "找不到该下标的相机";
                return false;
            }
            var first = devices[index];
            return this.Open(first, exposureTime);
        }

        /// <summary>
        /// 打开相机,使用IP
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="exposureTime"></param>
        /// <returns></returns>
        public bool Open(string ip, double exposureTime = -1)
        {
            if (string.IsNullOrWhiteSpace(ip))
            {
                this.ErroMsg = "没有设置IP地址";
                return false;
            }
            if (this._ip != ip)
                this.Close();
            else if (this.IsOpen)
                return true;

            this._ip = ip;

            if (this.GigeDevices == null)
                this.GigeDevices = new List<MVSCameraProvider.MV_GIGE_DEVICE_INFO>();

            var index = this.DeviceListAcq(ip);

            if (index < 0)
            {
                return false;
            }


            // ch:获取选择的设备信息 | en:Get selected device information
            MVSCameraProvider.MV_CC_DEVICE_INFO device =
                (MVSCameraProvider.MV_CC_DEVICE_INFO)Marshal.PtrToStructure(m_stDeviceList.pDeviceInfo[index], typeof(MVSCameraProvider.MV_CC_DEVICE_INFO));

            // ch:打开设备 | en:Open device
            if (null == m_MyCamera)
            {
                m_MyCamera = new MVSCameraProvider();
                if (null == m_MyCamera)
                {
                    this.ErroMsg = "无法初始化相机对象";
                    return false;
                }
            }

            int nRet = m_MyCamera.MV_CC_CreateDevice_NET(ref device);
            if (MVSCameraProvider.MV_OK != nRet)
            {
                //this.ErroMsg = "Can't CreateDevice";
                this.ShowErrorMsg("无法创建相机设备对象", nRet);
                return false;
            }

            nRet = m_MyCamera.MV_CC_OpenDevice_NET();
            if (MVSCameraProvider.MV_OK != nRet)
            {
                m_MyCamera.MV_CC_DestroyDevice_NET();
                //this.ErroMsg = "Device open fail!" + nRet;
                this.ShowErrorMsg("无法打开相机", nRet);
                return false;
            }

            // ch:探测网络最佳包大小(只对GigE相机有效) | en:Detection network optimal package size(It only works for the GigE camera)
            if (device.nTLayerType == MVSCameraProvider.MV_GIGE_DEVICE)
            {
                int nPacketSize = m_MyCamera.MV_CC_GetOptimalPacketSize_NET();
                if (nPacketSize > 0)
                {
                    nRet = m_MyCamera.MV_CC_SetIntValue_NET("GevSCPSPacketSize", (uint)nPacketSize);
                    if (nRet != MVSCameraProvider.MV_OK)
                    {
                        // this.ErroMsg = "Set Packet Size failed!" + nRet;
                        this.ShowErrorMsg("设置PacketSize出错", nRet);
                        return false;
                    }
                }
                else
                {
                    this.ErroMsg = "获取 Packet Size 错误!" + nPacketSize;
                    return false;
                }
            }

            // ch:设置采集连续模式 | en:Set Continues Aquisition Mode
            m_MyCamera.MV_CC_SetEnumValue_NET("AcquisitionMode", (uint)MVSCameraProvider.MV_CAM_ACQUISITION_MODE.MV_ACQ_MODE_CONTINUOUS);
            m_MyCamera.MV_CC_SetEnumValue_NET("TriggerMode", (uint)MVSCameraProvider.MV_CAM_TRIGGER_MODE.MV_TRIGGER_MODE_OFF);
            m_MyCamera.MV_CC_SetEnumValue_NET("TriggerMode", (uint)MVSCameraProvider.MV_CAM_TRIGGER_MODE.MV_TRIGGER_MODE_OFF);

            var rect = this.GetCurrentAOISize();
            if (rect.IsEmpty)
                return false;

            this.MaxSize = new Size(rect.MaxWidth, rect.MaxHeight);

            if (exposureTime > 0)
            {
                //设置曝光
                this.m_MyCamera.MV_CC_SetExposureTime_NET(Convert.ToSingle(exposureTime));
            }
            return this.StartGrab();
        }

        /// <summary>
        /// 开始采集
        /// </summary>
        /// <returns></returns>
        public bool StartGrab()
        {
            // ch:标志位置位true | en:Set position bit true
            

            // ch:开始采集 | en:Start Grabbing
            int nRet = m_MyCamera.MV_CC_StartGrabbing_NET();
            if (MVSCameraProvider.MV_OK != nRet)
            {
                this.IsOpen = false;
                //this.ErroMsg = "Start Grabbing Fail!" + nRet;
                this.ShowErrorMsg("采集出错", nRet);
                this.Close();
                return false;
            }


            this.IsOpen = true;
            return true;
        }


        /// <summary>
        /// 停止采集
        /// </summary>
        public void StopGrab()
        {
            // ch:标志位设为false | en:Set flag bit false
           
            // ch:停止采集 | en:Stop Grabbing
            int nRet = m_MyCamera.MV_CC_StopGrabbing_NET();
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
        public Bitmap GetBitmap()
        {
            #region New

            if (!this.IsOpen)
            {
                this.ErroMsg = "相机未开启或未开始采集";
                return null;
            }
            lock (m_MyCamera)
            {

                this.ErroMsg = "";
                MVSCameraProvider.MV_FRAME_OUT_INFO_EX stFrameInfoEx = new MVSCameraProvider.MV_FRAME_OUT_INFO_EX();

                byte[] buffer = new byte[5500 * 4000 * 3];

                try
                {
                    var PData = Marshal.UnsafeAddrOfPinnedArrayElement(buffer, 0);
                    var nRet2 = m_MyCamera.MV_CC_GetImageForBGR_NET(PData, (uint)buffer.Length, ref stFrameInfoEx, 1000);

                    if (nRet2 != MVSCameraProvider.MV_OK)
                    {
                        if (this.DeviceListAcq(this._ip) < 0)
                        {
                            //查看是否有设备
                            this.IsOpen = false;
                        }
                        else
                        {
                            this.ShowErrorMsg("没有找到图像", nRet2);
                        }
                        return null;
                    }

                    var width = (int)stFrameInfoEx.nWidth;
                    var height = (int)stFrameInfoEx.nHeight;
                    var rgbColor = true;
                    var stride = this.GetStride(width, rgbColor);
                    var len = (int)stFrameInfoEx.nFrameLen;
                    var pixelFomart = this.GetFormat(rgbColor);
                    var bufferLen = stride * height;

                    if (this.renderBitmap == null || this.renderBitmap.Width != width || this.renderBitmap.Height != height)
                        CreateBitmap(ref this.renderBitmap, width, height, pixelFomart);
                    this.UpdateBitmap(renderBitmap, buffer, width, true, bufferLen);

                    var image = renderBitmap.Clone(new Rectangle(0, 0, width, height), pixelFomart);

                    return image;

                }
                catch (Exception ex)
                {
                    this.ErroMsg = ex + "";
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
        public CameraRectangleInfo GetCurrentAOISize()
        {
            MVSCameraProvider.MVCC_INTVALUE value = default;
            var code = this.m_MyCamera.MV_CC_GetWidth_NET(ref value);
            if (code != MVSCameraProvider.MV_OK)
            {
                this.ShowErrorMsg("获取宽度信息错误", code);
                return new CameraRectangleInfo();
            }

            var maxWidth = value.nMax;
            var curWidth = value.nCurValue;
            var widthInc = value.nInc;

            code = m_MyCamera.MV_CC_GetHeight_NET(ref value);
            if (code != MVSCameraProvider.MV_OK)
            {
                this.ShowErrorMsg("获取高度信息错误", code);
                return new CameraRectangleInfo();
            }

            var maxHeight = value.nMax;
            var curHeight = value.nCurValue;
            var heightInc = value.nInc;

            code = m_MyCamera.MV_CC_GetAOIoffsetX_NET(ref value);
            if (code != MVSCameraProvider.MV_OK)
            {
                this.ShowErrorMsg("获取AOI OffsetX信息错误", code);
                return new CameraRectangleInfo();
            }

            var curOffX = value.nCurValue;
            var curOffXInc = value.nInc;

            code = m_MyCamera.MV_CC_GetAOIoffsetY_NET(ref value);
            if (code != MVSCameraProvider.MV_OK)
            {
                this.ShowErrorMsg("获取AOI OffsetY信息错误", code);
                return new CameraRectangleInfo();
            }

            var curOffY = value.nCurValue;
            var curOffYInc = value.nInc;
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
        public void RestoryMaxROI()
        {
            this.SetMaxAOI(true);
        }

        /// <summary>
        /// 更新大小
        /// </summary>
        /// <param name="rect"></param>
        /// <returns></returns>
        public Rectangle ParseAOI(Rectangle rect)
        {
            //查看当前Rect的大小是否与最大值一样
            if (rect.Width == this.MaxSize.Width && rect.Height == this.MaxSize.Height)
            {
                //已经OK了
                return new Rectangle(0, 0, rect.Width, rect.Height);
            }

            #region New
            var info = this.GetCurrentAOISize();
            var p1 = new Point(rect.X, rect.Y);
            var offXInc = info.OffXInc;
            var offYInc = info.OffYInc;

            //将其扩大
            //计算左上角偏移点
            var xResult = p1.X - p1.X % offXInc;
            xResult = xResult < 0 ? 0 : xResult;

            var yResult = p1.Y - p1.Y % offYInc;
            yResult = yResult < 0 ? 0 : yResult;

            var widthInc = info.WidthInc;
            var heightInc = info.HeightInc;


            //计算宽
            var width = rect.Width + p1.X % offXInc;//如果左顶点x往左偏移过,则可以加大点
            var widthResult = width - width % widthInc + widthInc;
            if (widthResult + xResult > this.MaxSize.Width)
            {
                //如果大于最大宽度,则将宽度减小
                widthResult = this.MaxSize.Width - widthResult;
            }

            //计算高
            var height = rect.Height + p1.Y % offYInc;
            var heightResult = height - height % heightInc + heightInc;
            if (heightResult + yResult > this.MaxSize.Height)
            {
                heightResult = this.MaxSize.Height - heightResult;
            }

            return new Rectangle(xResult, yResult, widthResult, heightResult);

            #endregion

        }

        /// <summary>
        /// 设置ROI
        /// </summary>
        /// <param name="rect"></param>
        /// <returns></returns>
        public bool SetAOI(ref Rectangle rect)
        {
            //查看当前Rect的大小是否与最大值一样
            if (rect.Width == this.MaxSize.Width && rect.Height == this.MaxSize.Height)
            {
                //已经OK了
                return true;
            }

            //先停止采集
            this.StopGrab();
            //将其设置到最大值
            var re = this.SetMaxAOI();
            if (!re) return false;

            rect = this.ParseAOI(rect);

            _ = m_MyCamera.MV_CC_SetWidth_NET((uint)rect.Width);

            _ = m_MyCamera.MV_CC_SetHeight_NET((uint)rect.Height);

            _ = m_MyCamera.MV_CC_SetAOIoffsetX_NET((uint)rect.Left);

            _ = m_MyCamera.MV_CC_SetAOIoffsetY_NET((uint)rect.Top);

            //检验是因为更换相机后造的
            CameraRectangleInfo resultRect = GetCurrentAOISize();
            rect = resultRect.ToRectangle();
            re = StartGrab();
            return re;
        }

        /// <summary>
        /// 设置最大ROI
        /// </summary>
        /// <param name="autoGrab"></param>
        /// <returns></returns>
        public bool SetMaxAOI(bool autoGrab = false)
        {
            if (this.IsOpen)
            {
                this.StopGrab();
            }

            var code = m_MyCamera.MV_CC_SetAOIoffsetX_NET(0);
            if (code != MVSCameraProvider.MV_OK)
            {
                this.ShowErrorMsg("重置OffsetX错误", code);
                return false;
            }

            code = m_MyCamera.MV_CC_SetAOIoffsetY_NET(0);
            if (code != MVSCameraProvider.MV_OK)
            {
                this.ShowErrorMsg("重置OffsetY错误", code);
                return false;
            }

            code = m_MyCamera.MV_CC_SetWidth_NET((uint)this.MaxSize.Width);
            if (code != MVSCameraProvider.MV_OK)
            {
                this.ShowErrorMsg("重置Width错误", code);
                return false;
            }

            code = m_MyCamera.MV_CC_SetHeight_NET((uint)this.MaxSize.Height);
            if (code != MVSCameraProvider.MV_OK)
            {
                this.ShowErrorMsg("重置Height错误", code);
                return false;
            }


            if (autoGrab)
            {
                return this.StartGrab();
            }
            return true;
        }

        /// <summary>
        /// 关闭相机
        /// </summary>
        public void Close()
        {
            this.ErroMsg = "";
            try
            {

                lock (BufForDriverLock)
                {
                    if (m_BufForDriver != IntPtr.Zero)
                    {
                        Marshal.Release(m_BufForDriver);
                    }
                }

            }
            catch (Exception ex)
            {
                this.ErroMsg = ex + "";
            }
            // ch:关闭设备 | en:Close Device
            try
            {
                m_MyCamera.MV_CC_CloseDevice_NET();
                m_MyCamera.MV_CC_DestroyDevice_NET();
            }
            catch (Exception)
            {

            }
            this.renderBitmap?.Dispose();
            this.renderBitmap = null;
            this.IsOpen = false;
        }

        private void ShowErrorMsg(string csMessage, int nErrorNum)
        {
            string errorMsg;
            if (nErrorNum == 0)
            {
                errorMsg = csMessage;
            }
            else
            {
                errorMsg = csMessage + ": Error =" + String.Format("{0:X}", nErrorNum);
            }

            errorMsg += ErrorCode.ErrorCodeInstance.GetErrorString(nErrorNum);
            this.ErroMsg = errorMsg;
        }

        /// <summary>
        /// 获取曝光信息
        /// </summary>
        /// <returns></returns>
        public virtual CameraExposureTimeInfo GetExposureTime()
        {
           var value = new MVSCameraProvider.MVCC_FLOATVALUE();
            this.m_MyCamera.MV_CC_GetExposureTime_NET(ref value);
            var info = new CameraExposureTimeInfo()
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
            this.StopGrab();
            this.m_MyCamera.MV_CC_SetExposureTime_NET(Convert.ToSingle(time));
            this.StartGrab();
        }
    }
}
