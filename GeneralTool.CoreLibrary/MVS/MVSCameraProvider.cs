using System;
using System.Runtime.InteropServices;

namespace GeneralTool.CoreLibrary.MVS
{
    /// <summary>
    /// MyCamera
    /// </summary>
    // Token: 0x02000002 RID: 2
    public class MVSCameraProvider
    {
        /// <summary>
        /// Get SDK Version
        /// </summary>
        /// <returns>Always return 4 Bytes of version number |Main  |Sub   |Rev   |Test|
        ///                                                   8bits  8bits  8bits  8bits 
        /// </returns>
        // Token: 0x06000001 RID: 1 RVA: 0x000020D0 File Offset: 0x000002D0
        public static uint MV_CC_GetSDKVersion_NET()
        {
            return MV_CC_GetSDKVersion();
        }

        /// <summary>
        /// Get supported Transport Layer
        /// </summary>
        /// <returns>Supported Transport Layer number</returns>
        // Token: 0x06000002 RID: 2 RVA: 0x000020D7 File Offset: 0x000002D7
        public static int MV_CC_EnumerateTls_NET()
        {
            return MV_CC_EnumerateTls();
        }

        /// <summary>
        /// Enumerate Device
        /// </summary>
        /// <param name="nTLayerType">Enumerate TLs</param>
        /// <param name="stDevList">Device List</param>
        /// <returns>Success, return MV_OK. Failure, return error code </returns>
        // Token: 0x06000003 RID: 3 RVA: 0x000020DE File Offset: 0x000002DE
        public static int MV_CC_EnumDevices_NET(uint nTLayerType, ref MV_CC_DEVICE_INFO_LIST stDevList)
        {
            return MV_CC_EnumDevices(nTLayerType, ref stDevList);
        }

        /// <summary>
        /// Enumerate device according to manufacture name
        /// </summary>
        /// <param name="nTLayerType">Enumerate TLs</param>
        /// <param name="stDevList">Device List</param>
        /// <param name="pManufacturerName">Manufacture Name</param>
        /// <returns>Success, return MV_OK. Failure, return error code </returns>
        // Token: 0x06000004 RID: 4 RVA: 0x000020E7 File Offset: 0x000002E7
        public static int MV_CC_EnumDevicesEx_NET(uint nTLayerType, ref MV_CC_DEVICE_INFO_LIST stDevList, string pManufacturerName)
        {
            return MV_CC_EnumDevicesEx(nTLayerType, ref stDevList, pManufacturerName);
        }

        /// <summary>
        /// Is the device accessible
        /// </summary>
        /// <param name="stDevInfo">Device Information</param>
        /// <param name="nAccessMode">Access Right</param>
        /// <returns>Access, return true. Not access, return false</returns>
        // Token: 0x06000005 RID: 5 RVA: 0x000020F1 File Offset: 0x000002F1
        public static bool MV_CC_IsDeviceAccessible_NET(ref MV_CC_DEVICE_INFO stDevInfo, uint nAccessMode)
        {
            return MV_CC_IsDeviceAccessible(ref stDevInfo, nAccessMode);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        // Token: 0x06000006 RID: 6 RVA: 0x000020FA File Offset: 0x000002FA
        public MVSCameraProvider()
        {
            handle = IntPtr.Zero;
        }

        /// <summary>
        /// Create Device
        /// </summary>
        /// <param name="stDevInfo">Device Information</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        // Token: 0x06000008 RID: 8 RVA: 0x00002138 File Offset: 0x00000338
        public int MV_CC_CreateDevice_NET(ref MV_CC_DEVICE_INFO stDevInfo)
        {
            if (IntPtr.Zero != handle)
            {
                _ = MV_CC_DestroyHandle(handle);
                handle = IntPtr.Zero;
            }
            return MV_CC_CreateHandle(ref handle, ref stDevInfo);
        }

        /// <summary>
        /// Create Device without log
        /// </summary>
        /// <param name="stDevInfo">Device Information</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        // Token: 0x06000009 RID: 9 RVA: 0x0000216F File Offset: 0x0000036F
        public int MV_CC_CreateDeviceWithoutLog_NET(ref MV_CC_DEVICE_INFO stDevInfo)
        {
            if (IntPtr.Zero != handle)
            {
                _ = MV_CC_DestroyHandle(handle);
                handle = IntPtr.Zero;
            }
            return MV_CC_CreateHandleWithoutLog(ref handle, ref stDevInfo);
        }

        /// <summary>
        /// Destroy Device
        /// </summary>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        // Token: 0x0600000A RID: 10 RVA: 0x000021A8 File Offset: 0x000003A8
        public int MV_CC_DestroyDevice_NET()
        {
            int result = MV_CC_DestroyHandle(handle);
            handle = IntPtr.Zero;
            return result;
        }

        /// <summary>
        /// Open Device
        /// </summary>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        // Token: 0x0600000B RID: 11 RVA: 0x000021CD File Offset: 0x000003CD
        public int MV_CC_OpenDevice_NET()
        {
            return MV_CC_OpenDevice(handle, 1U, 0);
        }

        /// <summary>
        /// Open Device
        /// </summary>
        /// <param name="nAccessMode">Access Right</param>
        /// <param name="nSwitchoverKey">Switch key of access right</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        // Token: 0x0600000C RID: 12 RVA: 0x000021DC File Offset: 0x000003DC
        public int MV_CC_OpenDevice_NET(uint nAccessMode, ushort nSwitchoverKey)
        {
            return MVSCameraProvider.MV_CC_OpenDevice(handle, nAccessMode, nSwitchoverKey);
        }

        /// <summary>
        /// Close Device
        /// </summary>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        // Token: 0x0600000D RID: 13 RVA: 0x000021EB File Offset: 0x000003EB
        public int MV_CC_CloseDevice_NET()
        {
            return MV_CC_CloseDevice(handle);
        }

        /// <summary>
        /// Is the device connected
        /// </summary>
        /// <returns>Connected, return true. Not Connected or DIsconnected, return false</returns>
        // Token: 0x0600000E RID: 14 RVA: 0x000021F8 File Offset: 0x000003F8
        public bool MV_CC_IsDeviceConnected_NET()
        {
            return MV_CC_IsDeviceConnected(handle);
        }

        /// <summary>
        /// Register the image callback function
        /// </summary>
        /// <param name="cbOutput">Callback function pointer</param>
        /// <param name="pUser">User defined variable</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        // Token: 0x0600000F RID: 15 RVA: 0x00002205 File Offset: 0x00000405
        public int MV_CC_RegisterImageCallBackEx_NET(cbOutputExdelegate cbOutput, IntPtr pUser)
        {
            return MV_CC_RegisterImageCallBackEx(handle, cbOutput, pUser);
        }

        /// <summary>
        /// Register the RGB image callback function
        /// </summary>
        /// <param name="cbOutput">Callback function pointer</param>
        /// <param name="pUser">User defined variable</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        // Token: 0x06000010 RID: 16 RVA: 0x00002214 File Offset: 0x00000414
        public int MV_CC_RegisterImageCallBackForRGB_NET(cbOutputExdelegate cbOutput, IntPtr pUser)
        {
            return MV_CC_RegisterImageCallBackForRGB(handle, cbOutput, pUser);
        }

        /// <summary>
        /// Register the BGR image callback function
        /// </summary>
        /// <param name="cbOutput">Callback function pointer</param>
        /// <param name="pUser">User defined variable</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        // Token: 0x06000011 RID: 17 RVA: 0x00002223 File Offset: 0x00000423
        public int MV_CC_RegisterImageCallBackForBGR_NET(cbOutputExdelegate cbOutput, IntPtr pUser)
        {
            return MV_CC_RegisterImageCallBackForBGR(handle, cbOutput, pUser);
        }

        /// <summary>
        /// Start Grabbing
        /// </summary>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        // Token: 0x06000012 RID: 18 RVA: 0x00002232 File Offset: 0x00000432
        public int MV_CC_StartGrabbing_NET()
        {
            return MV_CC_StartGrabbing(handle);
        }

        /// <summary>
        /// Stop Grabbing
        /// </summary>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        // Token: 0x06000013 RID: 19 RVA: 0x0000223F File Offset: 0x0000043F
        public int MV_CC_StopGrabbing_NET()
        {
            return MV_CC_StopGrabbing(handle);
        }

        /// <summary>
        /// Get one frame of RGB image, this function is using query to get data
        /// query whether the internal cache has data, get data if there has, return error code if no data
        /// </summary>
        /// <param name="pData">Image data receiving buffer</param>
        /// <param name="nDataSize">Buffer size</param>
        /// <param name="pFrameInfo">Image information</param>
        /// <param name="nMsec">Waiting timeout</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        // Token: 0x06000014 RID: 20 RVA: 0x0000224C File Offset: 0x0000044C
        public int MV_CC_GetImageForRGB_NET(IntPtr pData, uint nDataSize, ref MV_FRAME_OUT_INFO_EX pFrameInfo, int nMsec)
        {
            return MV_CC_GetImageForRGB(handle, pData, nDataSize, ref pFrameInfo, nMsec);
        }

        /// <summary>
        /// Get one frame of BGR image, this function is using query to get data
        /// query whether the internal cache has data, get data if there has, return error code if no data
        /// </summary>
        /// <param name="pData">Image data receiving buffer</param>
        /// <param name="nDataSize">Buffer size</param>
        /// <param name="pFrameInfo">Image information</param>
        /// <param name="nMsec">Waiting timeout</param>
        /// <returns>Success, return MV_OK. Failure, return error cod</returns>
        // Token: 0x06000015 RID: 21 RVA: 0x0000225E File Offset: 0x0000045E
        public int MV_CC_GetImageForBGR_NET(IntPtr pData, uint nDataSize, ref MV_FRAME_OUT_INFO_EX pFrameInfo, int nMsec)
        {
            return MV_CC_GetImageForBGR(handle, pData, nDataSize, ref pFrameInfo, nMsec);
        }

        /// <summary>
        /// Get a frame of an image using an internal cache
        /// </summary>
        /// <param name="pFrame">Image data and image information</param>
        /// <param name="nMsec">Waiting timeout</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        // Token: 0x06000016 RID: 22 RVA: 0x00002270 File Offset: 0x00000470
        public int MV_CC_GetImageBuffer_NET(ref MV_FRAME_OUT pFrame, int nMsec)
        {
            return MV_CC_GetImageBuffer(handle, ref pFrame, nMsec);
        }

        /// <summary>
        /// Free image buffer（used with MV_CC_GetImageBuffer）
        /// </summary>
        /// <param name="pFrame">Image data and image information</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        // Token: 0x06000017 RID: 23 RVA: 0x0000227F File Offset: 0x0000047F
        public int MV_CC_FreeImageBuffer_NET(ref MV_FRAME_OUT pFrame)
        {
            return MV_CC_FreeImageBuffer(handle, ref pFrame);
        }

        /// <summary>
        /// Get a frame of an image
        /// </summary>
        /// <param name="pData">Image data receiving buffer</param>
        /// <param name="nDataSize">Buffer size</param>
        /// <param name="pFrameInfo">Image information</param>
        /// <param name="nMsec">Waiting timeout</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        // Token: 0x06000018 RID: 24 RVA: 0x0000228D File Offset: 0x0000048D
        public int MV_CC_GetOneFrameTimeout_NET(IntPtr pData, uint nDataSize, ref MV_FRAME_OUT_INFO_EX pFrameInfo, int nMsec)
        {
            return MV_CC_GetOneFrameTimeout(handle, pData, nDataSize, ref pFrameInfo, nMsec);
        }

        /// <summary>
        /// Clear image Buffers to clear old data
        /// </summary>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        // Token: 0x06000019 RID: 25 RVA: 0x0000229F File Offset: 0x0000049F
        public int MV_CC_ClearImageBuffer_NET()
        {
            return MV_CC_ClearImageBuffer(handle);
        }

        /// <summary>
        /// Display one frame image
        /// </summary>
        /// <param name="pDisplayInfo">Image information</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        // Token: 0x0600001A RID: 26 RVA: 0x000022AC File Offset: 0x000004AC
        public int MV_CC_DisplayOneFrame_NET(ref MV_DISPLAY_FRAME_INFO pDisplayInfo)
        {
            return MV_CC_DisplayOneFrame(handle, ref pDisplayInfo);
        }

        /// <summary>
        /// Set the number of the internal image cache nodes in SDK(Greater than or equal to 1, to be called before the capture)
        /// </summary>
        /// <param name="nNum">Number of cache nodes</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        // Token: 0x0600001B RID: 27 RVA: 0x000022BA File Offset: 0x000004BA
        public int MV_CC_SetImageNodeNum_NET(uint nNum)
        {
            return MV_CC_SetImageNodeNum(handle, nNum);
        }

        /// <summary>
        /// Set Grab Strategy
        /// </summary>
        /// <param name="enGrabStrategy">The value of grab strategy</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        // Token: 0x0600001C RID: 28 RVA: 0x000022C8 File Offset: 0x000004C8
        public int MV_CC_SetGrabStrategy_NET(MV_GRAB_STRATEGY enGrabStrategy)
        {
            return MV_CC_SetGrabStrategy(handle, enGrabStrategy);
        }

        /// <summary>
        /// Set The Size of Output Queue(Only work under the strategy of MV_GrabStrategy_LatestImages，rang：1-ImageNodeNum)
        /// </summary>
        /// <param name="nOutputQueueSize">The Size of Output Queue</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        // Token: 0x0600001D RID: 29 RVA: 0x000022D6 File Offset: 0x000004D6
        public int MV_CC_SetOutputQueueSize_NET(uint nOutputQueueSize)
        {
            return MV_CC_SetOutputQueueSize(handle, nOutputQueueSize);
        }

        /// <summary>
        /// Get device information(Called before start grabbing)
        /// </summary>
        /// <param name="pstDevInfo">device information</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        // Token: 0x0600001E RID: 30 RVA: 0x000022E4 File Offset: 0x000004E4
        public int MV_CC_GetDeviceInfo_NET(ref MV_CC_DEVICE_INFO pstDevInfo)
        {
            return MV_CC_GetDeviceInfo(handle, ref pstDevInfo);
        }

        /// <summary>
        /// Get various type of information
        /// </summary>
        /// <param name="pstInfo">Various type of information</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        // Token: 0x0600001F RID: 31 RVA: 0x000022F2 File Offset: 0x000004F2
        public int MV_CC_GetAllMatchInfo_NET(ref MV_ALL_MATCH_INFO pstInfo)
        {
            return MV_CC_GetAllMatchInfo(handle, ref pstInfo);
        }

        /// <summary>
        /// Get Integer value
        /// </summary>
        /// <param name="strKey">Key value, for example, using "Width" to get width</param>
        /// <param name="pstValue">Value of device features</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        // Token: 0x06000020 RID: 32 RVA: 0x00002300 File Offset: 0x00000500
        public int MV_CC_GetIntValueEx_NET(string strKey, ref MVCC_INTVALUE_EX pstValue)
        {
            return MV_CC_GetIntValueEx(handle, strKey, ref pstValue);
        }

        /// <summary>
        /// Set Integer value
        /// </summary>
        /// <param name="strKey">Key value, for example, using "Width" to set width</param>
        /// <param name="nValue">Feature value to set</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        // Token: 0x06000021 RID: 33 RVA: 0x0000230F File Offset: 0x0000050F
        public int MV_CC_SetIntValueEx_NET(string strKey, long nValue)
        {
            return MV_CC_SetIntValueEx(handle, strKey, nValue);
        }

        /// <summary>
        /// Get Enum value
        /// </summary>
        /// <param name="strKey">Key value, for example, using "PixelFormat" to get pixel format</param>
        /// <param name="pstValue">Value of device features</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        // Token: 0x06000022 RID: 34 RVA: 0x0000231E File Offset: 0x0000051E
        public int MV_CC_GetEnumValue_NET(string strKey, ref MVCC_ENUMVALUE pstValue)
        {
            return MV_CC_GetEnumValue(handle, strKey, ref pstValue);
        }

        /// <summary>
        /// Set Enum value
        /// </summary>
        /// <param name="strKey">Key value, for example, using "PixelFormat" to set pixel format</param>
        /// <param name="nValue">Feature value to set</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        // Token: 0x06000023 RID: 35 RVA: 0x0000232D File Offset: 0x0000052D
        public int MV_CC_SetEnumValue_NET(string strKey, uint nValue)
        {
            return MV_CC_SetEnumValue(handle, strKey, nValue);
        }

        /// <summary>
        /// Set Enum value
        /// </summary>
        /// <param name="strKey">Key value, for example, using "PixelFormat" to set pixel format</param>
        /// <param name="sValue">Feature String to set</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        // Token: 0x06000024 RID: 36 RVA: 0x0000233C File Offset: 0x0000053C
        public int MV_CC_SetEnumValueByString_NET(string strKey, string sValue)
        {
            return MV_CC_SetEnumValueByString(handle, strKey, sValue);
        }

        /// <summary>
        /// Get Float value
        /// </summary>
        /// <param name="strKey">Key value</param>
        /// <param name="pstValue">Value of device features</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        // Token: 0x06000025 RID: 37 RVA: 0x0000234B File Offset: 0x0000054B
        public int MV_CC_GetFloatValue_NET(string strKey, ref MVCC_FLOATVALUE pstValue)
        {
            return MV_CC_GetFloatValue(handle, strKey, ref pstValue);
        }

        /// <summary>
        /// Set float value
        /// </summary>
        /// <param name="strKey">Key value</param>
        /// <param name="fValue">Feature value to set</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        // Token: 0x06000026 RID: 38 RVA: 0x0000235A File Offset: 0x0000055A
        public int MV_CC_SetFloatValue_NET(string strKey, float fValue)
        {
            return MV_CC_SetFloatValue(handle, strKey, fValue);
        }

        /// <summary>
        /// Get Boolean value
        /// </summary>
        /// <param name="strKey">Key value</param>
        /// <param name="pbValue">Value of device features</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        // Token: 0x06000027 RID: 39 RVA: 0x00002369 File Offset: 0x00000569
        public int MV_CC_GetBoolValue_NET(string strKey, ref bool pbValue)
        {
            return MV_CC_GetBoolValue(handle, strKey, ref pbValue);
        }

        /// <summary>
        /// Set Boolean value
        /// </summary>
        /// <param name="strKey">Key value</param>
        /// <param name="bValue">Feature value to set</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        // Token: 0x06000028 RID: 40 RVA: 0x00002378 File Offset: 0x00000578
        public int MV_CC_SetBoolValue_NET(string strKey, bool bValue)
        {
            return MV_CC_SetBoolValue(handle, strKey, bValue);
        }

        /// <summary>
        /// Get String value
        /// </summary>
        /// <param name="strKey">Key value</param>
        /// <param name="pstValue">Value of device features</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        // Token: 0x06000029 RID: 41 RVA: 0x00002387 File Offset: 0x00000587
        public int MV_CC_GetStringValue_NET(string strKey, ref MVCC_STRINGVALUE pstValue)
        {
            return MV_CC_GetStringValue(handle, strKey, ref pstValue);
        }

        /// <summary>
        /// Set String value
        /// </summary>
        /// <param name="strKey">Key value</param>
        /// <param name="strValue">Feature value to set</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        // Token: 0x0600002A RID: 42 RVA: 0x00002396 File Offset: 0x00000596
        public int MV_CC_SetStringValue_NET(string strKey, string strValue)
        {
            return MV_CC_SetStringValue(handle, strKey, strValue);
        }

        /// <summary>
        /// Send Command
        /// </summary>
        /// <param name="strKey">Key value</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        // Token: 0x0600002B RID: 43 RVA: 0x000023A5 File Offset: 0x000005A5
        public int MV_CC_SetCommandValue_NET(string strKey)
        {
            return MV_CC_SetCommandValue(handle, strKey);
        }

        /// <summary>
        /// Invalidate GenICam Nodes
        /// </summary>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        // Token: 0x0600002C RID: 44 RVA: 0x000023B3 File Offset: 0x000005B3
        public int MV_CC_InvalidateNodes_NET()
        {
            return MV_CC_InvalidateNodes(handle);
        }

        /// <summary>
        /// Device Local Upgrade
        /// </summary>
        /// <param name="pFilePathName">File path and name</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        // Token: 0x0600002D RID: 45 RVA: 0x000023C0 File Offset: 0x000005C0
        public int MV_CC_LocalUpgrade_NET(string pFilePathName)
        {
            return MV_CC_LocalUpgrade(handle, pFilePathName);
        }

        /// <summary>
        /// Get Upgrade Progress
        /// </summary>
        /// <param name="pnProcess">Value of Progress</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        // Token: 0x0600002E RID: 46 RVA: 0x000023CE File Offset: 0x000005CE
        public int MV_CC_GetUpgradeProcess_NET(ref uint pnProcess)
        {
            return MV_CC_GetUpgradeProcess(handle, ref pnProcess);
        }

        /// <summary>
        /// Read Memory
        /// </summary>
        /// <param name="pBuffer">Used as a return value, save the read-in memory value(Memory value is stored in accordance with the big end model)</param>
        /// <param name="nAddress">Memory address to be read, which can be obtained from the Camera.xml file of the device, the form xml node value of xxx_RegAddr</param>
        /// <param name="nLength">Length of the memory to be read</param>
        /// <returns>Success, return MV_OK. Failure, return error code </returns>
        // Token: 0x0600002F RID: 47 RVA: 0x000023DC File Offset: 0x000005DC
        public int MV_CC_ReadMemory_NET(IntPtr pBuffer, long nAddress, long nLength)
        {
            return MV_CC_ReadMemory(handle, pBuffer, nAddress, nLength);
        }

        /// <summary>
        /// Write Memory
        /// </summary>
        /// <param name="pBuffer">Memory value to be written ( Note the memory value to be stored in accordance with the big end model)</param>
        /// <param name="nAddress">Memory address to be written, which can be obtained from the Camera.xml file of the device, the form xml node value of xxx_RegAddr</param>
        /// <param name="nLength">Length of the memory to be written</param>
        /// <returns>Success, return MV_OK. Failure, return error code </returns>
        // Token: 0x06000030 RID: 48 RVA: 0x000023EC File Offset: 0x000005EC
        public int MV_CC_WriteMemory_NET(IntPtr pBuffer, long nAddress, long nLength)
        {
            return MV_CC_WriteMemory(handle, pBuffer, nAddress, nLength);
        }

        /// <summary>
        /// Register Exception Message CallBack, call after open device
        /// </summary>
        /// <param name="cbException">Exception Message CallBack Function</param>
        /// <param name="pUser">User defined variable</param>
        /// <returns>Success, return MV_OK. Failure, return error code </returns>
        // Token: 0x06000031 RID: 49 RVA: 0x000023FC File Offset: 0x000005FC
        public int MV_CC_RegisterExceptionCallBack_NET(cbExceptiondelegate cbException, IntPtr pUser)
        {
            return MV_CC_RegisterExceptionCallBack(handle, cbException, pUser);
        }

        /// <summary>
        /// Register event callback, which is called after the device is opened
        /// </summary>
        /// <param name="cbEvent">Event CallBack Function</param>
        /// <param name="pUser">User defined variable</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        // Token: 0x06000032 RID: 50 RVA: 0x0000240B File Offset: 0x0000060B
        public int MV_CC_RegisterAllEventCallBack_NET(cbEventdelegateEx cbEvent, IntPtr pUser)
        {
            return MV_CC_RegisterAllEventCallBack(handle, cbEvent, pUser);
        }

        /// <summary>
        /// Register single event callback, which is called after the device is opened
        /// </summary>
        /// <param name="pEventName">Event name</param>
        /// <param name="cbEvent">Event CallBack Function</param>
        /// <param name="pUser">User defined variable</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        // Token: 0x06000033 RID: 51 RVA: 0x0000241A File Offset: 0x0000061A
        public int MV_CC_RegisterEventCallBackEx_NET(string pEventName, cbEventdelegateEx cbEvent, IntPtr pUser)
        {
            return MV_CC_RegisterEventCallBackEx(handle, pEventName, cbEvent, pUser);
        }

        /// <summary>
        /// Force IP
        /// </summary>
        /// <param name="nIP">IP to set</param>
        /// <param name="nSubNetMask">Subnet mask</param>
        /// <param name="nDefaultGateWay">Default gateway</param>
        /// <returns>Success, return MV_OK. Failure, return error code </returns>
        // Token: 0x06000034 RID: 52 RVA: 0x0000242A File Offset: 0x0000062A
        public int MV_GIGE_ForceIpEx_NET(uint nIP, uint nSubNetMask, uint nDefaultGateWay)
        {
            return MV_GIGE_ForceIpEx(handle, nIP, nSubNetMask, nDefaultGateWay);
        }

        /// <summary>
        /// IP configuration method
        /// </summary>
        /// <param name="nType">IP type, refer to MV_IP_CFG_x</param>
        /// <returns>Success, return MV_OK. Failure, return error code </returns>
        // Token: 0x06000035 RID: 53 RVA: 0x0000243A File Offset: 0x0000063A
        public int MV_GIGE_SetIpConfig_NET(uint nType)
        {
            return MV_GIGE_SetIpConfig(handle, nType);
        }

        /// <summary>
        /// Set to use only one mode,type: MV_NET_TRANS_x. When do not set, priority is to use driver by default
        /// </summary>
        /// <param name="nType">Net transmission mode, refer to MV_NET_TRANS_x</param>
        /// <returns>Success, return MV_OK. Failure, return error code </returns>
        // Token: 0x06000036 RID: 54 RVA: 0x00002448 File Offset: 0x00000648
        public int MV_GIGE_SetNetTransMode_NET(uint nType)
        {
            return MV_GIGE_SetNetTransMode(handle, nType);
        }

        /// <summary>
        /// Get net transmission information
        /// </summary>
        /// <param name="pstInfo">Transmission information</param>
        /// <returns>Success, return MV_OK. Failure, return error code </returns>
        // Token: 0x06000037 RID: 55 RVA: 0x00002456 File Offset: 0x00000656
        public int MV_GIGE_GetNetTransInfo_NET(ref MV_NETTRANS_INFO pstInfo)
        {
            return MV_GIGE_GetNetTransInfo(handle, ref pstInfo);
        }

        /// <summary>
        /// Setting the ACK mode of devices Discovery
        /// </summary>
        /// <param name="nMode">ACK mode（Default-Broadcast）,0-Unicast,1-Broadcast</param>
        /// <returns>Success, return MV_OK. Failure, return error code </returns>
        // Token: 0x06000038 RID: 56 RVA: 0x00002464 File Offset: 0x00000664
        public int MV_GIGE_SetDiscoveryMode_NET(uint nMode)
        {
            return MV_GIGE_SetDiscoveryMode(nMode);
        }

        /// <summary>
        /// Set GVSP streaming timeout
        /// </summary>
        /// <param name="nMillisec">Timeout, default 300ms, range: &gt;10ms</param>
        /// <returns>Success, return MV_OK. Failure, return error code </returns>
        // Token: 0x06000039 RID: 57 RVA: 0x0000246C File Offset: 0x0000066C
        public int MV_GIGE_SetGvspTimeout_NET(uint nMillisec)
        {
            return MV_GIGE_SetGvspTimeout(handle, nMillisec);
        }

        /// <summary>
        /// Get GVSP streaming timeout
        /// </summary>
        /// <param name="pMillisec">Timeout, ms as unit</param>
        /// <returns>Success, return MV_OK. Failure, return error code </returns>
        // Token: 0x0600003A RID: 58 RVA: 0x0000247A File Offset: 0x0000067A
        public int MV_GIGE_GetGvspTimeout_NET(ref uint pMillisec)
        {
            return MV_GIGE_GetGvspTimeout(handle, ref pMillisec);
        }

        /// <summary>
        /// Set GVCP cammand timeout
        /// </summary>
        /// <param name="nMillisec">Timeout, ms as unit, range: 0-10000</param>
        /// <returns>Success, return MV_OK. Failure, return error code </returns>
        // Token: 0x0600003B RID: 59 RVA: 0x00002488 File Offset: 0x00000688
        public int MV_GIGE_SetGvcpTimeout_NET(uint nMillisec)
        {
            return MV_GIGE_SetGvcpTimeout(handle, nMillisec);
        }

        /// <summary>
        /// Get GVCP cammand timeout
        /// </summary>
        /// <param name="pMillisec">Timeout, ms as unit</param>
        /// <returns>Success, return MV_OK. Failure, return error code </returns>
        // Token: 0x0600003C RID: 60 RVA: 0x00002496 File Offset: 0x00000696
        public int MV_GIGE_GetGvcpTimeout_NET(ref uint pMillisec)
        {
            return MV_GIGE_GetGvcpTimeout(handle, ref pMillisec);
        }

        /// <summary>
        /// Set the number of retry GVCP cammand
        /// </summary>
        /// <param name="nRetryGvcpTimes">The number of retries，rang：0-100</param>
        /// <returns>Success, return MV_OK. Failure, return error code </returns>
        // Token: 0x0600003D RID: 61 RVA: 0x000024A4 File Offset: 0x000006A4
        public int MV_GIGE_SetRetryGvcpTimes_NET(uint nRetryGvcpTimes)
        {
            return MV_GIGE_SetRetryGvcpTimes(handle, nRetryGvcpTimes);
        }

        /// <summary>
        /// Get the number of retry GVCP cammand
        /// </summary>
        /// <param name="pRetryGvcpTimes">The number of retries</param>
        /// <returns>Success, return MV_OK. Failure, return error code </returns>
        // Token: 0x0600003E RID: 62 RVA: 0x000024B2 File Offset: 0x000006B2
        public int MV_GIGE_GetRetryGvcpTimes_NET(ref uint pRetryGvcpTimes)
        {
            return MV_GIGE_GetRetryGvcpTimes(handle, ref pRetryGvcpTimes);
        }

        /// <summary>
        /// Get the optimal Packet Size, Only support GigE Camera
        /// </summary>
        /// <returns>Optimal packet size</returns>
        // Token: 0x0600003F RID: 63 RVA: 0x000024C0 File Offset: 0x000006C0
        public int MV_CC_GetOptimalPacketSize_NET()
        {
            return MV_CC_GetOptimalPacketSize(handle);
        }

        /// <summary>
        /// Set whethe to enable resend, and set resend
        /// </summary>
        /// <param name="bEnable">Enable resend</param>
        /// <param name="nMaxResendPercent">Max resend persent</param>
        /// <param name="nResendTimeout">Resend timeout</param>
        /// <returns>Success, return MV_OK. Failure, return error code </returns>
        // Token: 0x06000040 RID: 64 RVA: 0x000024CD File Offset: 0x000006CD
        public int MV_GIGE_SetResend_NET(uint bEnable, uint nMaxResendPercent, uint nResendTimeout)
        {
            return MV_GIGE_SetResend(handle, bEnable, nMaxResendPercent, nResendTimeout);
        }

        /// <summary>
        /// Set the max resend retry times
        /// </summary>
        /// <param name="nRetryTimes">The max times to retry resending lost packets，default 20</param>
        /// <returns>Success, return MV_OK. Failure, return error code </returns>
        // Token: 0x06000041 RID: 65 RVA: 0x000024DD File Offset: 0x000006DD
        public int MV_GIGE_SetResendMaxRetryTimes_NET(uint nRetryTimes)
        {
            return MV_GIGE_SetResendMaxRetryTimes(handle, nRetryTimes);
        }

        /// <summary>
        /// Get the max resend retry times
        /// </summary>
        /// <param name="pnRetryTimes">the max times to retry resending lost packets</param>
        /// <returns>Success, return MV_OK. Failure, return error code </returns>
        // Token: 0x06000042 RID: 66 RVA: 0x000024EB File Offset: 0x000006EB
        public int MV_GIGE_GetResendMaxRetryTimes_NET(ref uint pnRetryTimes)
        {
            return MV_GIGE_GetResendMaxRetryTimes(handle, ref pnRetryTimes);
        }

        /// <summary>
        /// Set time interval between same resend requests
        /// </summary>
        /// <param name="nMillisec">The time interval between same resend requests,default 10ms</param>
        /// <returns>Success, return MV_OK. Failure, return error code </returns>
        // Token: 0x06000043 RID: 67 RVA: 0x000024F9 File Offset: 0x000006F9
        public int MV_GIGE_SetResendTimeInterval_NET(uint nMillisec)
        {
            return MV_GIGE_SetResendTimeInterval(handle, nMillisec);
        }

        /// <summary>
        /// Get time interval between same resend requests
        /// </summary>
        /// <param name="pnMillisec">The time interval between same resend requests</param>
        /// <returns>Success, return MV_OK. Failure, return error code </returns>
        // Token: 0x06000044 RID: 68 RVA: 0x00002507 File Offset: 0x00000707
        public int MV_GIGE_GetResendTimeInterval_NET(ref uint pnMillisec)
        {
            return MV_GIGE_GetResendTimeInterval(handle, ref pnMillisec);
        }

        /// <summary>
        /// Set transmission type,Unicast or Multicast
        /// </summary>
        /// <param name="pstTransmissionType">Struct of transmission type</param>
        /// <returns>Success, return MV_OK. Failure, return error code </returns>
        // Token: 0x06000045 RID: 69 RVA: 0x00002515 File Offset: 0x00000715
        public int MV_GIGE_SetTransmissionType_NET(ref MV_CC_TRANSMISSION_TYPE pstTransmissionType)
        {
            return MV_GIGE_SetTransmissionType(handle, ref pstTransmissionType);
        }

        /// <summary>
        /// Issue Action Command
        /// </summary>
        /// <param name="pstActionCmdInfo">Action Command info</param>
        /// <param name="pstActionCmdResults">Action Command Result List</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        // Token: 0x06000046 RID: 70 RVA: 0x00002523 File Offset: 0x00000723
        public int MV_GIGE_IssueActionCommand_NET(ref MV_ACTION_CMD_INFO pstActionCmdInfo, ref MV_ACTION_CMD_RESULT_LIST pstActionCmdResults)
        {
            return MV_GIGE_IssueActionCommand(ref pstActionCmdInfo, ref pstActionCmdResults);
        }

        /// <summary>
        /// Get Multicast Status
        /// </summary>
        /// <param name="pstDevInfo">Device Information</param>
        /// <param name="pStatus">Status of Multicast</param>
        /// <returns>Success, return MV_OK. Failure, return error code </returns>
        // Token: 0x06000047 RID: 71 RVA: 0x0000252C File Offset: 0x0000072C
        public static int MV_GIGE_GetMulticastStatus_NET(ref MV_CC_DEVICE_INFO pstDevInfo, ref bool pStatus)
        {
            return MV_GIGE_GetMulticastStatus(ref pstDevInfo, ref pStatus);
        }

        /// <summary>
        /// Set device baudrate using one of the CL_BAUDRATE_XXXX value
        /// </summary>
        /// <param name="nBaudrate">Baudrate to set. Refer to the 'CameraParams.h' for parameter definitions, for example, #define MV_CAML_BAUDRATE_9600  0x00000001</param>
        /// <returns>Success, return MV_OK. Failure, return error code </returns>
        // Token: 0x06000048 RID: 72 RVA: 0x00002535 File Offset: 0x00000735
        public int MV_CAML_SetDeviceBaudrate_NET(uint nBaudrate)
        {
            return MV_CAML_SetDeviceBaudrate(handle, nBaudrate);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nBaudrate"></param>
        /// <returns></returns>
        public int MV_CAML_SetDeviceBauderate_NET(uint nBaudrate)
        {
            return MV_CAML_SetDeviceBaudrate(handle, nBaudrate);
        }

        /// <summary>
        /// Get device baudrate, using one of the CL_BAUDRATE_XXXX value
        /// </summary>
        /// <param name="pnCurrentBaudrate">Return pointer of baud rate to user. 
        ///                                 Refer to the 'CameraParams.h' for parameter definitions, for example, #define MV_CAML_BAUDRATE_9600  0x00000001</param>
        /// <returns>Success, return MV_OK. Failure, return error code </returns>
        // Token: 0x0600004A RID: 74 RVA: 0x00002551 File Offset: 0x00000751
        public int MV_CAML_GetDeviceBaudrate_NET(ref uint pnCurrentBaudrate)
        {
            return MV_CAML_GetDeviceBaudrate(handle, ref pnCurrentBaudrate);
        }

        /// <summary>
        /// 
        /// </summary>
        public int MV_CAML_GetDeviceBauderate_NET(ref uint pnCurrentBaudrate)
        {
            return MV_CAML_GetDeviceBaudrate(handle, ref pnCurrentBaudrate);
        }

        /// <summary>
        /// Get supported baudrates of the combined device and host interface
        /// </summary>
        /// <param name="pnBaudrateAblity">Return pointer of the supported baudrates to user. 'OR' operation results of the supported baudrates. 
        ///                                Refer to the 'CameraParams.h' for single value definitions, for example, #define MV_CAML_BAUDRATE_9600  0x00000001</param>
        /// <returns>Success, return MV_OK. Failure, return error code </returns>
        // Token: 0x0600004C RID: 76 RVA: 0x0000256D File Offset: 0x0000076D
        public int MV_CAML_GetSupportBaudrates_NET(ref uint pnBaudrateAblity)
        {
            return MV_CAML_GetSupportBaudrates(handle, ref pnBaudrateAblity);
        }

        /// <summary>
        /// 
        /// </summary>
        public int MV_CAML_GetSupportBauderates_NET(ref uint pnBaudrateAblity)
        {
            return MV_CAML_GetSupportBaudrates(handle, ref pnBaudrateAblity);
        }

        /// <summary>
        /// Sets the timeout for operations on the serial port
        /// </summary>
        /// <param name="nMillisec">Timeout in [ms] for operations on the serial port.</param>
        /// <returns>Success, return MV_OK. Failure, return error code </returns>
        // Token: 0x0600004E RID: 78 RVA: 0x00002589 File Offset: 0x00000789
        public int MV_CAML_SetGenCPTimeOut_NET(uint nMillisec)
        {
            return MV_CAML_SetGenCPTimeOut(handle, nMillisec);
        }

        /// <summary>
        /// Set transfer size of U3V device
        /// </summary>
        /// <param name="nTransferSize">Transfer size，Byte，default：1M，rang：&gt;=0x10000</param>
        /// <returns>Success, return MV_OK. Failure, return error code </returns>
        // Token: 0x0600004F RID: 79 RVA: 0x00002597 File Offset: 0x00000797
        public int MV_USB_SetTransferSize_NET(uint nTransferSize)
        {
            return MV_USB_SetTransferSize(handle, nTransferSize);
        }

        /// <summary>
        /// Get transfer size of U3V device
        /// </summary>
        /// <param name="pTransferSize">Transfer size，Byte</param>
        /// <returns>Success, return MV_OK. Failure, return error code </returns>
        // Token: 0x06000050 RID: 80 RVA: 0x000025A5 File Offset: 0x000007A5
        public int MV_USB_GetTransferSize_NET(ref uint pTransferSize)
        {
            return MV_USB_GetTransferSize(handle, ref pTransferSize);
        }

        /// <summary>
        /// Set transfer ways of U3V device
        /// </summary>
        /// <param name="nTransferWays">Transfer ways，rang：1-10</param>
        /// <returns>Success, return MV_OK. Failure, return error code </returns>
        // Token: 0x06000051 RID: 81 RVA: 0x000025B3 File Offset: 0x000007B3
        public int MV_USB_SetTransferWays_NET(uint nTransferWays)
        {
            return MV_USB_SetTransferWays(handle, nTransferWays);
        }

        /// <summary>
        /// Get transfer ways of U3V device
        /// </summary>
        /// <param name="pTransferWays">Transfer ways</param>
        /// <returns>Success, return MV_OK. Failure, return error code </returns>
        // Token: 0x06000052 RID: 82 RVA: 0x000025C1 File Offset: 0x000007C1
        public int MV_USB_GetTransferWays_NET(ref uint pTransferWays)
        {
            return MV_USB_GetTransferWays(handle, ref pTransferWays);
        }

        /// <summary>
        /// Enumerate interfaces by GenTL
        /// </summary>
        /// <param name="stIFInfoList"> Interface information list</param>
        /// <param name="pGenTLPath">Path of GenTL's cti file</param>
        /// <returns></returns>
        // Token: 0x06000053 RID: 83 RVA: 0x000025CF File Offset: 0x000007CF
        public static int MV_CC_EnumInterfacesByGenTL_NET(ref MV_GENTL_IF_INFO_LIST stIFInfoList, string pGenTLPath)
        {
            return MV_CC_EnumInterfacesByGenTL(ref stIFInfoList, pGenTLPath);
        }

        /// <summary>
        /// Enumerate Device Based On GenTL
        /// </summary>
        /// <param name="stIFInfo">Interface information</param>
        /// <param name="stDevList">Device List</param>
        /// <returns>Success, return MV_OK. Failure, return error code </returns>
        // Token: 0x06000054 RID: 84 RVA: 0x000025D8 File Offset: 0x000007D8
        public static int MV_CC_EnumDevicesByGenTL_NET(ref MV_GENTL_IF_INFO stIFInfo, ref MV_GENTL_DEV_INFO_LIST stDevList)
        {
            return MV_CC_EnumDevicesByGenTL(ref stIFInfo, ref stDevList);
        }

        /// <summary>
        /// Create Device Handle Based On GenTL Device Info
        /// </summary>
        /// <param name="stDevInfo">Device Information Structure</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        // Token: 0x06000055 RID: 85 RVA: 0x000025E1 File Offset: 0x000007E1
        public int MV_CC_CreateDeviceByGenTL_NET(ref MV_GENTL_DEV_INFO stDevInfo)
        {
            if (IntPtr.Zero != handle)
            {
                _ = MV_CC_DestroyHandle(handle);
                handle = IntPtr.Zero;
            }
            return MV_CC_CreateHandleByGenTL(ref handle, ref stDevInfo);
        }

        /// <summary>
        /// Get camera feature tree XML
        /// </summary>
        /// <param name="pData">XML data receiving buffer</param>
        /// <param name="nDataSize">Buffer size</param>
        /// <param name="pnDataLen">Actual data length</param>
        /// <returns>Success, return MV_OK. Failure, return error code </returns>
        // Token: 0x06000056 RID: 86 RVA: 0x00002618 File Offset: 0x00000818
        public int MV_XML_GetGenICamXML_NET(IntPtr pData, uint nDataSize, ref uint pnDataLen)
        {
            return MV_XML_GetGenICamXML(handle, pData, nDataSize, ref pnDataLen);
        }

        /// <summary>
        /// Get Access mode of cur node
        /// </summary>
        /// <param name="pstrName">Name of node</param>
        /// <param name="pAccessMode">Access mode of the node</param>
        /// <returns>Success, return MV_OK. Failure, return error code </returns>
        // Token: 0x06000057 RID: 87 RVA: 0x00002628 File Offset: 0x00000828
        public int MV_XML_GetNodeAccessMode_NET(string pstrName, ref MV_XML_AccessMode pAccessMode)
        {
            return MV_XML_GetNodeAccessMode(handle, pstrName, ref pAccessMode);
        }

        /// <summary>
        /// Get Interface Type of cur node
        /// </summary>
        /// <param name="pstrName">Name of node</param>
        /// <param name="pInterfaceType">Interface Type of the node</param>
        /// <returns>Success, return MV_OK. Failure, return error code </returns>
        // Token: 0x06000058 RID: 88 RVA: 0x00002637 File Offset: 0x00000837
        public int MV_XML_GetNodeInterfaceType_NET(string pstrName, ref MV_XML_InterfaceType pInterfaceType)
        {
            return MV_XML_GetNodeInterfaceType(handle, pstrName, ref pInterfaceType);
        }

        /// <summary>
        /// Save image, support Bmp and Jpeg. Encoding quality(50-99]
        /// </summary>
        /// <param name="stSaveParam">Save image parameters structure</param>
        /// <returns>Success, return MV_OK. Failure, return error code </returns>
        // Token: 0x06000059 RID: 89 RVA: 0x00002646 File Offset: 0x00000846
        public int MV_CC_SaveImageEx_NET(ref MV_SAVE_IMAGE_PARAM_EX stSaveParam)
        {
            return MV_CC_SaveImageEx2(handle, ref stSaveParam);
        }

        /// <summary>
        /// Save the image file, support Bmp、 Jpeg、Png and Tiff. Encoding quality(50-99]
        /// </summary>
        /// <param name="pstSaveFileParam">Save the image file parameter structure</param>
        /// <returns>Success, return MV_OK. Failure, return error code </returns>
        // Token: 0x0600005A RID: 90 RVA: 0x00002654 File Offset: 0x00000854
        public int MV_CC_SaveImageToFile_NET(ref MV_SAVE_IMG_TO_FILE_PARAM pstSaveFileParam)
        {
            return MV_CC_SaveImageToFile(handle, ref pstSaveFileParam);
        }

        /// <summary>
        /// Save 3D point data, support PLY、CSV and OBJ
        /// </summary>
        /// <param name="pstPointDataParam">Save 3D point data parameters structure</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        // Token: 0x0600005B RID: 91 RVA: 0x00002662 File Offset: 0x00000862
        public int MV_CC_SavePointCloudData_NET(ref MV_SAVE_POINT_CLOUD_PARAM pstPointDataParam)
        {
            return MV_CC_SavePointCloudData(handle, ref pstPointDataParam);
        }

        /// <summary>
        /// Rotate Image
        /// </summary>
        /// <param name="pstRotateParam">Rotate image parameter structure</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        // Token: 0x0600005C RID: 92 RVA: 0x00002670 File Offset: 0x00000870
        public int MV_CC_RotateImage_NET(ref MV_CC_ROTATE_IMAGE_PARAM pstRotateParam)
        {
            return MV_CC_RotateImage(handle, ref pstRotateParam);
        }

        /// <summary>
        /// Flip Image
        /// </summary>
        /// <param name="pstFlipParam">Flip image parameter structure</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        // Token: 0x0600005D RID: 93 RVA: 0x0000267E File Offset: 0x0000087E
        public int MV_CC_FlipImage_NET(ref MV_CC_FLIP_IMAGE_PARAM pstFlipParam)
        {
            return MV_CC_FlipImage(handle, ref pstFlipParam);
        }

        /// <summary>
        /// Pixel format conversion
        /// </summary>
        /// <param name="pstCvtParam">Convert Pixel Type parameter structure</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        // Token: 0x0600005E RID: 94 RVA: 0x0000268C File Offset: 0x0000088C
        public int MV_CC_ConvertPixelType_NET(ref MV_PIXEL_CONVERT_PARAM pstCvtParam)
        {
            return MV_CC_ConvertPixelType(handle, ref pstCvtParam);
        }

        /// <summary>
        /// Interpolation algorithm type setting
        /// </summary>
        /// <param name="BayerCvtQuality">Bayer interpolation method  0-Fast 1-Equilibrium 2-Optimal</param>
        /// <returns>Success, return MV_OK. Failure, return error code </returns>
        // Token: 0x0600005F RID: 95 RVA: 0x0000269A File Offset: 0x0000089A
        public int MV_CC_SetBayerCvtQuality_NET(uint BayerCvtQuality)
        {
            return MV_CC_SetBayerCvtQuality(handle, BayerCvtQuality);
        }

        /// <summary>
        /// Set Gamma value
        /// </summary>
        /// <param name="fBayerGammaValue">Gamma value[0.1,4.0]</param>
        /// <returns>Success, return MV_OK. Failure, return error code </returns>
        // Token: 0x06000060 RID: 96 RVA: 0x000026A8 File Offset: 0x000008A8
        public int MV_CC_SetBayerGammaValue_NET(float fBayerGammaValue)
        {
            return MV_CC_SetBayerGammaValue(handle, fBayerGammaValue);
        }

        /// <summary>
        /// Set Gamma param
        /// </summary>
        /// <param name="pstGammaParam">Gamma parameter structure</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        // Token: 0x06000061 RID: 97 RVA: 0x000026B6 File Offset: 0x000008B6
        public int MV_CC_SetBayerGammaParam_NET(ref MV_CC_GAMMA_PARAM pstGammaParam)
        {
            return MV_CC_SetBayerGammaParam(handle, ref pstGammaParam);
        }

        /// <summary>
        /// Set CCM param
        /// </summary>
        /// <param name="pstCCMParam">CCM parameter structure</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        // Token: 0x06000062 RID: 98 RVA: 0x000026C4 File Offset: 0x000008C4
        public int MV_CC_SetBayerCCMParam_NET(ref MV_CC_CCM_PARAM pstCCMParam)
        {
            return MV_CC_SetBayerCCMParam(handle, ref pstCCMParam);
        }

        /// <summary>
        /// Set CCM param
        /// </summary>
        /// <param name="pstCCMParam">CCM parameter structure</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        // Token: 0x06000063 RID: 99 RVA: 0x000026D2 File Offset: 0x000008D2
        public int MV_CC_SetBayerCCMParamEx_NET(ref MV_CC_CCM_PARAM_EX pstCCMParam)
        {
            return MV_CC_SetBayerCCMParamEx(handle, ref pstCCMParam);
        }

        /// <summary>
        /// Set CLUT param
        /// </summary>
        /// <param name="pstCLUTParam">CLUT parameter structure</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        // Token: 0x06000064 RID: 100 RVA: 0x000026E0 File Offset: 0x000008E0
        public int MV_CC_SetBayerCLUTParam_NET(ref MV_CC_CLUT_PARAM pstCLUTParam)
        {
            return MV_CC_SetBayerCLUTParam(handle, ref pstCLUTParam);
        }

        /// <summary>
        /// Adjust image contrast
        /// </summary>
        /// <param name="pstContrastParam">Contrast parameter structure</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        // Token: 0x06000065 RID: 101 RVA: 0x000026EE File Offset: 0x000008EE
        public int MV_CC_ImageContrast_NET(ref MV_CC_CONTRAST_PARAM pstContrastParam)
        {
            return MV_CC_ImageContrast(handle, ref pstContrastParam);
        }

        /// <summary>
        /// Image sharpen
        /// </summary>
        /// <param name="pstSharpenParam">Sharpen parameter structure</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        // Token: 0x06000066 RID: 102 RVA: 0x000026FC File Offset: 0x000008FC
        public int MV_CC_ImageSharpen_NET(ref MV_CC_SHARPEN_PARAM pstSharpenParam)
        {
            return MV_CC_ImageSharpen(handle, ref pstSharpenParam);
        }

        /// <summary>
        /// Color Correct(include CCM and CLUT)
        /// </summary>
        /// <param name="pstColorCorrectParam">Color Correct parameter structure</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        // Token: 0x06000067 RID: 103 RVA: 0x0000270A File Offset: 0x0000090A
        public int MV_CC_ColorCorrect_NET(ref MV_CC_COLOR_CORRECT_PARAM pstColorCorrectParam)
        {
            return MV_CC_ColorCorrect(handle, ref pstColorCorrectParam);
        }

        /// <summary>
        /// Noise Estimate
        /// </summary>
        /// <param name="pstNoiseEstimateParam">Noise Estimate parameter structure</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        // Token: 0x06000068 RID: 104 RVA: 0x00002718 File Offset: 0x00000918
        public int MV_CC_NoiseEstimate_NET(ref MV_CC_NOISE_ESTIMATE_PARAM pstNoiseEstimateParam)
        {
            return MV_CC_NoiseEstimate(handle, ref pstNoiseEstimateParam);
        }

        /// <summary>
        /// Spatial Denoise
        /// </summary>
        /// <param name="pstSpatialDenoiseParam">Spatial Denoise parameter structure</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        // Token: 0x06000069 RID: 105 RVA: 0x00002726 File Offset: 0x00000926
        public int MV_CC_SpatialDenoise_NET(ref MV_CC_SPATIAL_DENOISE_PARAM pstSpatialDenoiseParam)
        {
            return MV_CC_SpatialDenoise(handle, ref pstSpatialDenoiseParam);
        }

        /// <summary>
        /// LSC Calib
        /// </summary>
        /// <param name="pstLSCCalibParam">LSC Calib parameter structure</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        // Token: 0x0600006A RID: 106 RVA: 0x00002734 File Offset: 0x00000934
        public int MV_CC_LSCCalib_NET(ref MV_CC_LSC_CALIB_PARAM pstLSCCalibParam)
        {
            return MV_CC_LSCCalib(handle, ref pstLSCCalibParam);
        }

        /// <summary>
        /// LSC Correct
        /// </summary>
        /// <param name="pstLSCCorrectParam">LSC Correct parameter structure</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        // Token: 0x0600006B RID: 107 RVA: 0x00002742 File Offset: 0x00000942
        public int MV_CC_LSCCorrect_NET(ref MV_CC_LSC_CORRECT_PARAM pstLSCCorrectParam)
        {
            return MV_CC_LSCCorrect(handle, ref pstLSCCorrectParam);
        }

        /// <summary>
        /// High Bandwidth Decode
        /// </summary>
        /// <param name="pstDecodeParam">High Bandwidth Decode parameter structure</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        // Token: 0x0600006C RID: 108 RVA: 0x00002750 File Offset: 0x00000950
        public int MV_CC_HB_Decode_NET(ref MV_CC_HB_DECODE_PARAM pstDecodeParam)
        {
            return MV_CC_HB_Decode(handle, ref pstDecodeParam);
        }

        /// <summary>
        /// Noise estimate of Bayer format
        /// </summary>
        /// <param name="pstNoiseEstimateParam">Noise estimate parameter structure</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        // Token: 0x0600006D RID: 109 RVA: 0x0000275E File Offset: 0x0000095E
        public int MV_CC_BayerNoiseEstimate_NET(ref MV_CC_BAYER_NOISE_ESTIMATE_PARAM pstNoiseEstimateParam)
        {
            return MV_CC_BayerNoiseEstimate(handle, ref pstNoiseEstimateParam);
        }

        /// <summary>
        /// Spatial Denoise of Bayer format
        /// </summary>
        /// <param name="pstSpatialDenoiseParam">Spatial Denoise parameter structure</param>
        /// <returns>Success, return MV_OK. Failure, return error code</returns>
        // Token: 0x0600006E RID: 110 RVA: 0x0000276C File Offset: 0x0000096C
        public int MV_CC_BayerSpatialDenoise_NET(ref MV_CC_BAYER_SPATIAL_DENOISE_PARAM pstSpatialDenoiseParam)
        {
            return MV_CC_BayerSpatialDenoise(handle, ref pstSpatialDenoiseParam);
        }

        /// <summary>
        /// Save camera feature
        /// </summary>
        /// <param name="pFileName">File name</param>
        /// <returns>Success, return MV_OK. Failure, return error code </returns>
        // Token: 0x0600006F RID: 111 RVA: 0x0000277A File Offset: 0x0000097A
        public int MV_CC_FeatureSave_NET(string pFileName)
        {
            return MV_CC_FeatureSave(handle, pFileName);
        }

        /// <summary>
        /// Load camera feature
        /// </summary>
        /// <param name="pFileName">File name</param>
        /// <returns>Success, return MV_OK. Failure, return error code </returns>
        // Token: 0x06000070 RID: 112 RVA: 0x00002788 File Offset: 0x00000988
        public int MV_CC_FeatureLoad_NET(string pFileName)
        {
            return MV_CC_FeatureLoad(handle, pFileName);
        }

        /// <summary>
        /// Read the file from the camera
        /// </summary>
        /// <param name="pstFileAccess">File access structure</param>
        /// <returns>Success, return MV_OK. Failure, return error code </returns>
        // Token: 0x06000071 RID: 113 RVA: 0x00002796 File Offset: 0x00000996
        public int MV_CC_FileAccessRead_NET(ref MV_CC_FILE_ACCESS pstFileAccess)
        {
            return MV_CC_FileAccessRead(handle, ref pstFileAccess);
        }

        /// <summary>
        /// Write the file to camera
        /// </summary>
        /// <param name="pstFileAccess">File access structure</param>
        /// <returns>Success, return MV_OK. Failure, return error code </returns>
        // Token: 0x06000072 RID: 114 RVA: 0x000027A4 File Offset: 0x000009A4
        public int MV_CC_FileAccessWrite_NET(ref MV_CC_FILE_ACCESS pstFileAccess)
        {
            return MV_CC_FileAccessWrite(handle, ref pstFileAccess);
        }

        /// <summary>
        /// Get File Access Progress 
        /// </summary>
        /// <param name="pstFileAccessProgress">File access Progress</param>
        /// <returns>Success, return MV_OK. Failure, return error code </returns>
        // Token: 0x06000073 RID: 115 RVA: 0x000027B2 File Offset: 0x000009B2
        public int MV_CC_GetFileAccessProgress_NET(ref MV_CC_FILE_ACCESS_PROGRESS pstFileAccessProgress)
        {
            return MV_CC_GetFileAccessProgress(handle, ref pstFileAccessProgress);
        }

        /// <summary>
        /// Start Record
        /// </summary>
        /// <param name="pstRecordParam">Record param structure</param>
        /// <returns>Success, return MV_OK. Failure, return error code </returns>
        // Token: 0x06000074 RID: 116 RVA: 0x000027C0 File Offset: 0x000009C0
        public int MV_CC_StartRecord_NET(ref MV_CC_RECORD_PARAM pstRecordParam)
        {
            return MV_CC_StartRecord(handle, ref pstRecordParam);
        }

        /// <summary>
        /// Input RAW data to Record
        /// </summary>
        /// <param name="pstInputFrameInfo">Record data structure</param>
        /// <returns>Success, return MV_OK. Failure, return error code </returns>
        // Token: 0x06000075 RID: 117 RVA: 0x000027CE File Offset: 0x000009CE
        public int MV_CC_InputOneFrame_NET(ref MV_CC_INPUT_FRAME_INFO pstInputFrameInfo)
        {
            return MV_CC_InputOneFrame(handle, ref pstInputFrameInfo);
        }

        /// <summary>
        /// Stop Record
        /// </summary>
        /// <returns>Success, return MV_OK. Failure, return error code </returns>
        // Token: 0x06000076 RID: 118 RVA: 0x000027DC File Offset: 0x000009DC
        public int MV_CC_StopRecord_NET()
        {
            return MV_CC_StopRecord(handle);
        }

        /// <summary>
        /// Set SDK log path (Interfaces not recommended)
        /// If the logging service MvLogServer is enabled, the interface is invalid and The logging service is enabled by default
        /// </summary>
        /// <param name="pSDKLogPath"></param>
        /// <returns></returns>
        // Token: 0x06000077 RID: 119 RVA: 0x000027E9 File Offset: 0x000009E9
        public static int MV_CC_SetSDKLogPath_NET(string pSDKLogPath)
        {
            return MV_CC_SetSDKLogPath(pSDKLogPath);
        }

        /// <summary>
        /// Get basic information of image (Interfaces not recommended)
        /// </summary>
        /// <param name="pstInfo"></param>
        /// <returns></returns>
        // Token: 0x06000078 RID: 120 RVA: 0x000027F1 File Offset: 0x000009F1
        public int MV_CC_GetImageInfo_NET(ref MV_IMAGE_BASIC_INFO pstInfo)
        {
            return MV_CC_GetImageInfo(handle, ref pstInfo);
        }

        /// <summary>
        /// Get GenICam proxy (Interfaces not recommended)
        /// </summary>
        /// <returns></returns>
        // Token: 0x06000079 RID: 121 RVA: 0x000027FF File Offset: 0x000009FF
        public IntPtr MV_CC_GetTlProxy_NET()
        {
            return MV_CC_GetTlProxy(handle);
        }

        /// <summary>
        /// Get root node (Interfaces not recommended)
        /// </summary>
        /// <param name="pstNode"></param>
        /// <returns></returns>
        // Token: 0x0600007A RID: 122 RVA: 0x0000280C File Offset: 0x00000A0C
        public int MV_XML_GetRootNode_NET(ref MV_XML_NODE_FEATURE pstNode)
        {
            return MV_XML_GetRootNode(handle, ref pstNode);
        }

        /// <summary>
        /// Get all children node of specific node from xml, root node is Root (Interfaces not recommended)
        /// </summary>
        /// <param name="pstNode"></param>
        /// <param name="pstNodesList"></param>
        /// <returns></returns>
        // Token: 0x0600007B RID: 123 RVA: 0x0000281A File Offset: 0x00000A1A
        public int MV_XML_GetChildren_NET(ref MV_XML_NODE_FEATURE pstNode, IntPtr pstNodesList)
        {
            return MV_XML_GetChildren(handle, ref pstNode, pstNodesList);
        }

        /// <summary>
        /// Get all children node of specific node from xml, root node is Root (Interfaces not recommended)
        /// </summary>
        /// <param name="pstNode"></param>
        /// <param name="pstNodesList"></param>
        /// <returns></returns>
        // Token: 0x0600007C RID: 124 RVA: 0x00002829 File Offset: 0x00000A29
        public int MV_XML_GetChildren_NET(ref MV_XML_NODE_FEATURE pstNode, ref MV_XML_NODES_LIST pstNodesList)
        {
            return MV_XML_GetChildren(handle, ref pstNode, ref pstNodesList);
        }

        /// <summary>
        /// Get current node feature (Interfaces not recommended)
        /// </summary>
        /// <param name="pstNode"></param>
        /// <param name="pstFeature"></param>
        /// <returns></returns>
        // Token: 0x0600007D RID: 125 RVA: 0x00002838 File Offset: 0x00000A38
        public int MV_XML_GetNodeFeature_NET(ref MV_XML_NODE_FEATURE pstNode, IntPtr pstFeature)
        {
            return MV_XML_GetNodeFeature(handle, ref pstNode, pstFeature);
        }

        /// <summary>
        /// Update node (Interfaces not recommended)
        /// </summary>
        /// <param name="enType"></param>
        /// <param name="pstFeature"></param>
        /// <returns></returns>
        // Token: 0x0600007E RID: 126 RVA: 0x00002847 File Offset: 0x00000A47
        public int MV_XML_UpdateNodeFeature_NET(MV_XML_InterfaceType enType, IntPtr pstFeature)
        {
            return MV_XML_UpdateNodeFeature(handle, enType, pstFeature);
        }

        /// <summary>
        /// Register update callback (Interfaces not recommended)
        /// </summary>
        /// <param name="cbXmlUpdate"></param>
        /// <param name="pUser"></param>
        /// <returns></returns>
        // Token: 0x0600007F RID: 127 RVA: 0x00002856 File Offset: 0x00000A56
        public int MV_XML_RegisterUpdateCallBack_NET(cbXmlUpdatedelegate cbXmlUpdate, IntPtr pUser)
        {
            return MV_XML_RegisterUpdateCallBack(handle, cbXmlUpdate, pUser);
        }

        /// <summary>
        /// This interface is abandoned, it is recommended to use the MV_CC_GetOneFrameTimeOut
        /// </summary>
        /// <param name="pData"></param>
        /// <param name="nDataSize"></param>
        /// <param name="pFrameInfo"></param>
        /// <returns></returns>
        // Token: 0x06000080 RID: 128 RVA: 0x00002865 File Offset: 0x00000A65
        public int MV_CC_GetOneFrame_NET(IntPtr pData, uint nDataSize, ref MV_FRAME_OUT_INFO pFrameInfo)
        {
            return MV_CC_GetOneFrame(handle, pData, nDataSize, ref pFrameInfo);
        }

        /// <summary>
        /// This interface is abandoned, it is recommended to use the MV_CC_GetOneFrameTimeOut
        /// </summary>
        /// <param name="pData"></param>
        /// <param name="nDataSize"></param>
        /// <param name="pFrameInfo"></param>
        /// <returns></returns>
        // Token: 0x06000081 RID: 129 RVA: 0x00002875 File Offset: 0x00000A75
        public int MV_CC_GetOneFrameEx_NET(IntPtr pData, uint nDataSize, ref MV_FRAME_OUT_INFO_EX pFrameInfo)
        {
            return MV_CC_GetOneFrameEx(handle, pData, nDataSize, ref pFrameInfo);
        }

        /// <summary>
        /// This interface is abandoned, it is recommended to use the MV_CC_RegisterImageCallBackEx
        /// </summary>
        /// <param name="cbOutput"></param>
        /// <param name="pUser"></param>
        /// <returns></returns>
        // Token: 0x06000082 RID: 130 RVA: 0x00002885 File Offset: 0x00000A85
        public int MV_CC_RegisterImageCallBack_NET(cbOutputdelegate cbOutput, IntPtr pUser)
        {
            return MV_CC_RegisterImageCallBack(handle, cbOutput, pUser);
        }

        /// <summary>
        /// This interface is abandoned, it is recommended to use the MV_CC_SaveImageEx
        /// </summary>
        /// <param name="stSaveParam"></param>
        /// <returns></returns>
        // Token: 0x06000083 RID: 131 RVA: 0x00002894 File Offset: 0x00000A94
        public int MV_CC_SaveImage_NET(ref MV_SAVE_IMAGE_PARAM stSaveParam)
        {
            return MV_CC_SaveImage(ref stSaveParam);
        }

        /// <summary>
        /// This interface is abandoned, it is recommended to use the MV_GIGE_ForceIpEx
        /// </summary>
        /// <param name="nIP"></param>
        /// <returns></returns>
        // Token: 0x06000084 RID: 132 RVA: 0x0000289C File Offset: 0x00000A9C
        public int MV_GIGE_ForceIp_NET(uint nIP)
        {
            return MV_GIGE_ForceIp(handle, nIP);
        }

        /// <summary>
        /// This interface is abandoned, it is recommended to use the MV_CC_RegisterEventCallBackEx
        /// </summary>
        /// <param name="cbEvent"></param>
        /// <param name="pUser"></param>
        /// <returns></returns>
        // Token: 0x06000085 RID: 133 RVA: 0x000028AA File Offset: 0x00000AAA
        public int MV_CC_RegisterEventCallBack_NET(cbEventdelegate cbEvent, IntPtr pUser)
        {
            return MV_CC_RegisterEventCallBack(handle, cbEvent, pUser);
        }

        /// <summary>
        /// This interface is abandoned, it is recommended to use the MV_CC_DisplayOneFrame
        /// </summary>
        /// <param name="hWnd"></param>
        /// <returns></returns>
        // Token: 0x06000086 RID: 134 RVA: 0x000028B9 File Offset: 0x00000AB9
        public int MV_CC_Display_NET(IntPtr hWnd)
        {
            return MV_CC_Display(handle, hWnd);
        }

        /// <summary>
        /// This interface is abandoned, it is recommended to use the MV_CC_GetIntValueEx
        /// </summary>
        /// <param name="strKey"></param>
        /// <param name="pstValue"></param>
        /// <returns></returns>
        // Token: 0x06000087 RID: 135 RVA: 0x000028C7 File Offset: 0x00000AC7
        public int MV_CC_GetIntValue_NET(string strKey, ref MVCC_INTVALUE pstValue)
        {
            return MV_CC_GetIntValue(handle, strKey, ref pstValue);
        }

        /// <summary>
        /// This interface is abandoned, it is recommended to use the MV_CC_SetIntValueEx
        /// </summary>
        /// <param name="strKey"></param>
        /// <param name="nValue"></param>
        /// <returns></returns>
        // Token: 0x06000088 RID: 136 RVA: 0x000028D6 File Offset: 0x00000AD6
        public int MV_CC_SetIntValue_NET(string strKey, uint nValue)
        {
            return MV_CC_SetIntValue(handle, strKey, nValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="pstValue"></param>
        /// <returns></returns>
        // Token: 0x06000089 RID: 137 RVA: 0x000028E5 File Offset: 0x00000AE5
        public int MV_CC_GetWidth_NET(ref MVCC_INTVALUE pstValue)
        {
            return MV_CC_GetWidth(handle, ref pstValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="nValue"></param>
        /// <returns></returns>
        // Token: 0x0600008A RID: 138 RVA: 0x000028F3 File Offset: 0x00000AF3
        public int MV_CC_SetWidth_NET(uint nValue)
        {
            return MV_CC_SetWidth(handle, nValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="pstValue"></param>
        /// <returns></returns>
        // Token: 0x0600008B RID: 139 RVA: 0x00002901 File Offset: 0x00000B01
        public int MV_CC_GetHeight_NET(ref MVCC_INTVALUE pstValue)
        {
            return MV_CC_GetHeight(handle, ref pstValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="nValue"></param>
        /// <returns></returns>
        // Token: 0x0600008C RID: 140 RVA: 0x0000290F File Offset: 0x00000B0F
        public int MV_CC_SetHeight_NET(uint nValue)
        {
            return MV_CC_SetHeight(handle, nValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="pstValue"></param>
        /// <returns></returns>
        // Token: 0x0600008D RID: 141 RVA: 0x0000291D File Offset: 0x00000B1D
        public int MV_CC_GetAOIoffsetX_NET(ref MVCC_INTVALUE pstValue)
        {
            return MV_CC_GetAOIoffsetX(handle, ref pstValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="nValue"></param>
        /// <returns></returns>
        // Token: 0x0600008E RID: 142 RVA: 0x0000292B File Offset: 0x00000B2B
        public int MV_CC_SetAOIoffsetX_NET(uint nValue)
        {
            return MV_CC_SetAOIoffsetX(handle, nValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="pstValue"></param>
        /// <returns></returns>
        // Token: 0x0600008F RID: 143 RVA: 0x00002939 File Offset: 0x00000B39
        public int MV_CC_GetAOIoffsetY_NET(ref MVCC_INTVALUE pstValue)
        {
            return MV_CC_GetAOIoffsetY(handle, ref pstValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="nValue"></param>
        /// <returns></returns>
        // Token: 0x06000090 RID: 144 RVA: 0x00002947 File Offset: 0x00000B47
        public int MV_CC_SetAOIoffsetY_NET(uint nValue)
        {
            return MV_CC_SetAOIoffsetY(handle, nValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="pstValue"></param>
        /// <returns></returns>
        // Token: 0x06000091 RID: 145 RVA: 0x00002955 File Offset: 0x00000B55
        public int MV_CC_GetAutoExposureTimeLower_NET(ref MVCC_INTVALUE pstValue)
        {
            return MV_CC_GetAutoExposureTimeLower(handle, ref pstValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="nValue"></param>
        /// <returns></returns>
        // Token: 0x06000092 RID: 146 RVA: 0x00002963 File Offset: 0x00000B63
        public int MV_CC_SetAutoExposureTimeLower_NET(uint nValue)
        {
            return MV_CC_SetAutoExposureTimeLower(handle, nValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="pstValue"></param>
        /// <returns></returns>
        // Token: 0x06000093 RID: 147 RVA: 0x00002971 File Offset: 0x00000B71
        public int MV_CC_GetAutoExposureTimeUpper_NET(ref MVCC_INTVALUE pstValue)
        {
            return MV_CC_GetAutoExposureTimeUpper(handle, ref pstValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="nValue"></param>
        /// <returns></returns>
        // Token: 0x06000094 RID: 148 RVA: 0x0000297F File Offset: 0x00000B7F
        public int MV_CC_SetAutoExposureTimeUpper_NET(uint nValue)
        {
            return MV_CC_SetAutoExposureTimeUpper(handle, nValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="pstValue"></param>
        /// <returns></returns>
        // Token: 0x06000095 RID: 149 RVA: 0x0000298D File Offset: 0x00000B8D
        public int MV_CC_GetBrightness_NET(ref MVCC_INTVALUE pstValue)
        {
            return MV_CC_GetBrightness(handle, ref pstValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="nValue"></param>
        /// <returns></returns>
        // Token: 0x06000096 RID: 150 RVA: 0x0000299B File Offset: 0x00000B9B
        public int MV_CC_SetBrightness_NET(uint nValue)
        {
            return MV_CC_SetBrightness(handle, nValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="pstValue"></param>
        /// <returns></returns>
        // Token: 0x06000097 RID: 151 RVA: 0x000029A9 File Offset: 0x00000BA9
        public int MV_CC_GetFrameRate_NET(ref MVCC_FLOATVALUE pstValue)
        {
            return MV_CC_GetFrameRate(handle, ref pstValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="fValue"></param>
        /// <returns></returns>
        // Token: 0x06000098 RID: 152 RVA: 0x000029B7 File Offset: 0x00000BB7
        public int MV_CC_SetFrameRate_NET(float fValue)
        {
            return MV_CC_SetFrameRate(handle, fValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="pstValue"></param>
        /// <returns></returns>
        // Token: 0x06000099 RID: 153 RVA: 0x000029C5 File Offset: 0x00000BC5
        public int MV_CC_GetGain_NET(ref MVCC_FLOATVALUE pstValue)
        {
            return MV_CC_GetGain(handle, ref pstValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="fValue"></param>
        /// <returns></returns>
        // Token: 0x0600009A RID: 154 RVA: 0x000029D3 File Offset: 0x00000BD3
        public int MV_CC_SetGain_NET(float fValue)
        {
            return MV_CC_SetGain(handle, fValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="pstValue"></param>
        /// <returns></returns>
        // Token: 0x0600009B RID: 155 RVA: 0x000029E1 File Offset: 0x00000BE1
        public int MV_CC_GetExposureTime_NET(ref MVCC_FLOATVALUE pstValue)
        {
            return MV_CC_GetExposureTime(handle, ref pstValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="fValue"></param>
        /// <returns></returns>
        // Token: 0x0600009C RID: 156 RVA: 0x000029EF File Offset: 0x00000BEF
        public int MV_CC_SetExposureTime_NET(float fValue)
        {
            return MV_CC_SetExposureTime(handle, fValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="pstValue"></param>
        /// <returns></returns>
        // Token: 0x0600009D RID: 157 RVA: 0x000029FD File Offset: 0x00000BFD
        public int MV_CC_GetPixelFormat_NET(ref MVCC_ENUMVALUE pstValue)
        {
            return MV_CC_GetPixelFormat(handle, ref pstValue);
        }

        /// <summary>
        /// 
        /// </summary>
        public int MV_CC_SetPixelFormat_NET(uint nValue)
        {
            return MV_CC_SetPixelFormat(handle, nValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="pstValue"></param>
        /// <returns></returns>
        // Token: 0x0600009F RID: 159 RVA: 0x00002A19 File Offset: 0x00000C19
        public int MV_CC_GetAcquisitionMode_NET(ref MVCC_ENUMVALUE pstValue)
        {
            return MV_CC_GetAcquisitionMode(handle, ref pstValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="nValue"></param>
        /// <returns></returns>
        // Token: 0x060000A0 RID: 160 RVA: 0x00002A27 File Offset: 0x00000C27
        public int MV_CC_SetAcquisitionMode_NET(uint nValue)
        {
            return MV_CC_SetAcquisitionMode(handle, nValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="pstValue"></param>
        /// <returns></returns>
        // Token: 0x060000A1 RID: 161 RVA: 0x00002A35 File Offset: 0x00000C35
        public int MV_CC_GetGainMode_NET(ref MVCC_ENUMVALUE pstValue)
        {
            return MV_CC_GetGainMode(handle, ref pstValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="nValue"></param>
        /// <returns></returns>
        // Token: 0x060000A2 RID: 162 RVA: 0x00002A43 File Offset: 0x00000C43
        public int MV_CC_SetGainMode_NET(uint nValue)
        {
            return MV_CC_SetGainMode(handle, nValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="pstValue"></param>
        /// <returns></returns>
        // Token: 0x060000A3 RID: 163 RVA: 0x00002A51 File Offset: 0x00000C51
        public int MV_CC_GetExposureAutoMode_NET(ref MVCC_ENUMVALUE pstValue)
        {
            return MV_CC_GetExposureAutoMode(handle, ref pstValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="nValue"></param>
        /// <returns></returns>
        // Token: 0x060000A4 RID: 164 RVA: 0x00002A5F File Offset: 0x00000C5F
        public int MV_CC_SetExposureAutoMode_NET(uint nValue)
        {
            return MV_CC_SetExposureAutoMode(handle, nValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="pstValue"></param>
        /// <returns></returns>
        // Token: 0x060000A5 RID: 165 RVA: 0x00002A6D File Offset: 0x00000C6D
        public int MV_CC_GetTriggerMode_NET(ref MVCC_ENUMVALUE pstValue)
        {
            return MV_CC_GetTriggerMode(handle, ref pstValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="nValue"></param>
        /// <returns></returns>
        // Token: 0x060000A6 RID: 166 RVA: 0x00002A7B File Offset: 0x00000C7B
        public int MV_CC_SetTriggerMode_NET(uint nValue)
        {
            return MV_CC_SetTriggerMode(handle, nValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="pstValue"></param>
        /// <returns></returns>
        // Token: 0x060000A7 RID: 167 RVA: 0x00002A89 File Offset: 0x00000C89
        public int MV_CC_GetTriggerDelay_NET(ref MVCC_FLOATVALUE pstValue)
        {
            return MV_CC_GetTriggerDelay(handle, ref pstValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="fValue"></param>
        /// <returns></returns>
        // Token: 0x060000A8 RID: 168 RVA: 0x00002A97 File Offset: 0x00000C97
        public int MV_CC_SetTriggerDelay_NET(float fValue)
        {
            return MV_CC_SetTriggerDelay(handle, fValue);
        }

        /// <summary>
        /// 
        /// </summary>
        public int MV_CC_GetTriggerSource_NET(ref MVCC_ENUMVALUE pstValue)
        {
            return MV_CC_GetTriggerSource(handle, ref pstValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="nValue"></param>
        /// <returns></returns>
        // Token: 0x060000AA RID: 170 RVA: 0x00002AB3 File Offset: 0x00000CB3
        public int MV_CC_SetTriggerSource_NET(uint nValue)
        {
            return MV_CC_SetTriggerSource(handle, nValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <returns></returns>
        // Token: 0x060000AB RID: 171 RVA: 0x00002AC1 File Offset: 0x00000CC1
        public int MV_CC_TriggerSoftwareExecute_NET()
        {
            return MV_CC_TriggerSoftwareExecute(handle);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="pstValue"></param>
        /// <returns></returns>
        // Token: 0x060000AC RID: 172 RVA: 0x00002ACE File Offset: 0x00000CCE
        public int MV_CC_GetGammaSelector_NET(ref MVCC_ENUMVALUE pstValue)
        {
            return MV_CC_GetGammaSelector(handle, ref pstValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="nValue"></param>
        /// <returns></returns>
        // Token: 0x060000AD RID: 173 RVA: 0x00002ADC File Offset: 0x00000CDC
        public int MV_CC_SetGammaSelector_NET(uint nValue)
        {
            return MV_CC_SetGammaSelector(handle, nValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="pstValue"></param>
        /// <returns></returns>
        // Token: 0x060000AE RID: 174 RVA: 0x00002AEA File Offset: 0x00000CEA
        public int MV_CC_GetGamma_NET(ref MVCC_FLOATVALUE pstValue)
        {
            return MV_CC_GetGamma(handle, ref pstValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="fValue"></param>
        /// <returns></returns>
        // Token: 0x060000AF RID: 175 RVA: 0x00002AF8 File Offset: 0x00000CF8
        public int MV_CC_SetGamma_NET(float fValue)
        {
            return MV_CC_SetGamma(handle, fValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="pstValue"></param>
        /// <returns></returns>
        // Token: 0x060000B0 RID: 176 RVA: 0x00002B06 File Offset: 0x00000D06
        public int MV_CC_GetSharpness_NET(ref MVCC_INTVALUE pstValue)
        {
            return MV_CC_GetSharpness(handle, ref pstValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="nValue"></param>
        /// <returns></returns>
        // Token: 0x060000B1 RID: 177 RVA: 0x00002B14 File Offset: 0x00000D14
        public int MV_CC_SetSharpness_NET(uint nValue)
        {
            return MV_CC_SetSharpness(handle, nValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="pstValue"></param>
        /// <returns></returns>
        // Token: 0x060000B2 RID: 178 RVA: 0x00002B22 File Offset: 0x00000D22
        public int MV_CC_GetHue_NET(ref MVCC_INTVALUE pstValue)
        {
            return MV_CC_GetHue(handle, ref pstValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="nValue"></param>
        /// <returns></returns>
        // Token: 0x060000B3 RID: 179 RVA: 0x00002B30 File Offset: 0x00000D30
        public int MV_CC_SetHue_NET(uint nValue)
        {
            return MV_CC_SetHue(handle, nValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="pstValue"></param>
        /// <returns></returns>
        // Token: 0x060000B4 RID: 180 RVA: 0x00002B3E File Offset: 0x00000D3E
        public int MV_CC_GetSaturation_NET(ref MVCC_INTVALUE pstValue)
        {
            return MV_CC_GetSaturation(handle, ref pstValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="nValue"></param>
        /// <returns></returns>
        // Token: 0x060000B5 RID: 181 RVA: 0x00002B4C File Offset: 0x00000D4C
        public int MV_CC_SetSaturation_NET(uint nValue)
        {
            return MV_CC_SetSaturation(handle, nValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="pstValue"></param>
        /// <returns></returns>
        // Token: 0x060000B6 RID: 182 RVA: 0x00002B5A File Offset: 0x00000D5A
        public int MV_CC_GetBalanceWhiteAuto_NET(ref MVCC_ENUMVALUE pstValue)
        {
            return MV_CC_GetBalanceWhiteAuto(handle, ref pstValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="nValue"></param>
        /// <returns></returns>
        // Token: 0x060000B7 RID: 183 RVA: 0x00002B68 File Offset: 0x00000D68
        public int MV_CC_SetBalanceWhiteAuto_NET(uint nValue)
        {
            return MV_CC_SetBalanceWhiteAuto(handle, nValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="pstValue"></param>
        /// <returns></returns>
        // Token: 0x060000B8 RID: 184 RVA: 0x00002B76 File Offset: 0x00000D76
        public int MV_CC_GetBalanceRatioRed_NET(ref MVCC_INTVALUE pstValue)
        {
            return MV_CC_GetBalanceRatioRed(handle, ref pstValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="nValue"></param>
        /// <returns></returns>
        // Token: 0x060000B9 RID: 185 RVA: 0x00002B84 File Offset: 0x00000D84
        public int MV_CC_SetBalanceRatioRed_NET(uint nValue)
        {
            return MV_CC_SetBalanceRatioRed(handle, nValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="pstValue"></param>
        /// <returns></returns>
        // Token: 0x060000BA RID: 186 RVA: 0x00002B92 File Offset: 0x00000D92
        public int MV_CC_GetBalanceRatioGreen_NET(ref MVCC_INTVALUE pstValue)
        {
            return MV_CC_GetBalanceRatioGreen(handle, ref pstValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="nValue"></param>
        /// <returns></returns>
        // Token: 0x060000BB RID: 187 RVA: 0x00002BA0 File Offset: 0x00000DA0
        public int MV_CC_SetBalanceRatioGreen_NET(uint nValue)
        {
            return MV_CC_SetBalanceRatioGreen(handle, nValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="pstValue"></param>
        /// <returns></returns>
        // Token: 0x060000BC RID: 188 RVA: 0x00002BAE File Offset: 0x00000DAE
        public int MV_CC_GetBalanceRatioBlue_NET(ref MVCC_INTVALUE pstValue)
        {
            return MV_CC_GetBalanceRatioBlue(handle, ref pstValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="nValue"></param>
        /// <returns></returns>
        // Token: 0x060000BD RID: 189 RVA: 0x00002BBC File Offset: 0x00000DBC
        public int MV_CC_SetBalanceRatioBlue_NET(uint nValue)
        {
            return MV_CC_SetBalanceRatioBlue(handle, nValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="pstValue"></param>
        /// <returns></returns>
        // Token: 0x060000BE RID: 190 RVA: 0x00002BCA File Offset: 0x00000DCA
        public int MV_CC_GetDeviceUserID_NET(ref MVCC_STRINGVALUE pstValue)
        {
            return MV_CC_GetDeviceUserID(handle, ref pstValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="chValue"></param>
        /// <returns></returns>
        // Token: 0x060000BF RID: 191 RVA: 0x00002BD8 File Offset: 0x00000DD8
        public int MV_CC_SetDeviceUserID_NET(string chValue)
        {
            return MV_CC_SetDeviceUserID(handle, chValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="pstValue"></param>
        /// <returns></returns>
        // Token: 0x060000C0 RID: 192 RVA: 0x00002BE6 File Offset: 0x00000DE6
        public int MV_CC_GetBurstFrameCount_NET(ref MVCC_INTVALUE pstValue)
        {
            return MV_CC_GetBurstFrameCount(handle, ref pstValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="nValue"></param>
        /// <returns></returns>
        // Token: 0x060000C1 RID: 193 RVA: 0x00002BF4 File Offset: 0x00000DF4
        public int MV_CC_SetBurstFrameCount_NET(uint nValue)
        {
            return MV_CC_SetBurstFrameCount(handle, nValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="pstValue"></param>
        /// <returns></returns>
        // Token: 0x060000C2 RID: 194 RVA: 0x00002C02 File Offset: 0x00000E02
        public int MV_CC_GetAcquisitionLineRate_NET(ref MVCC_INTVALUE pstValue)
        {
            return MV_CC_GetAcquisitionLineRate(handle, ref pstValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="nValue"></param>
        /// <returns></returns>
        // Token: 0x060000C3 RID: 195 RVA: 0x00002C10 File Offset: 0x00000E10
        public int MV_CC_SetAcquisitionLineRate_NET(uint nValue)
        {
            return MV_CC_SetAcquisitionLineRate(handle, nValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="pstValue"></param>
        /// <returns></returns>
        // Token: 0x060000C4 RID: 196 RVA: 0x00002C1E File Offset: 0x00000E1E
        public int MV_CC_GetHeartBeatTimeout_NET(ref MVCC_INTVALUE pstValue)
        {
            return MV_CC_GetHeartBeatTimeout(handle, ref pstValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="nValue"></param>
        /// <returns></returns>
        // Token: 0x060000C5 RID: 197 RVA: 0x00002C2C File Offset: 0x00000E2C
        public int MV_CC_SetHeartBeatTimeout_NET(uint nValue)
        {
            return MV_CC_SetHeartBeatTimeout(handle, nValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="pstValue"></param>
        /// <returns></returns>
        // Token: 0x060000C6 RID: 198 RVA: 0x00002C3A File Offset: 0x00000E3A
        public int MV_GIGE_GetGevSCPSPacketSize_NET(ref MVCC_INTVALUE pstValue)
        {
            return MV_GIGE_GetGevSCPSPacketSize(handle, ref pstValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="nValue"></param>
        /// <returns></returns>
        // Token: 0x060000C7 RID: 199 RVA: 0x00002C48 File Offset: 0x00000E48
        public int MV_GIGE_SetGevSCPSPacketSize_NET(uint nValue)
        {
            return MV_GIGE_SetGevSCPSPacketSize(handle, nValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="pstValue"></param>
        /// <returns></returns>
        // Token: 0x060000C8 RID: 200 RVA: 0x00002C56 File Offset: 0x00000E56
        public int MV_GIGE_GetGevSCPD_NET(ref MVCC_INTVALUE pstValue)
        {
            return MV_GIGE_GetGevSCPD(handle, ref pstValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="nValue"></param>
        /// <returns></returns>
        // Token: 0x060000C9 RID: 201 RVA: 0x00002C64 File Offset: 0x00000E64
        public int MV_GIGE_SetGevSCPD_NET(uint nValue)
        {
            return MV_GIGE_SetGevSCPD(handle, nValue);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="pnIP"></param>
        /// <returns></returns>
        // Token: 0x060000CA RID: 202 RVA: 0x00002C72 File Offset: 0x00000E72
        public int MV_GIGE_GetGevSCDA_NET(ref uint pnIP)
        {
            return MV_GIGE_GetGevSCDA(handle, ref pnIP);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="nIP"></param>
        /// <returns></returns>
        // Token: 0x060000CB RID: 203 RVA: 0x00002C80 File Offset: 0x00000E80
        public int MV_GIGE_SetGevSCDA_NET(uint nIP)
        {
            return MV_GIGE_SetGevSCDA(handle, nIP);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="pnPort"></param>
        /// <returns></returns>
        // Token: 0x060000CC RID: 204 RVA: 0x00002C8E File Offset: 0x00000E8E
        public int MV_GIGE_GetGevSCSP_NET(ref uint pnPort)
        {
            return MV_GIGE_GetGevSCSP(handle, ref pnPort);
        }

        /// <summary>
        /// This interface is replaced by general interface
        /// </summary>
        /// <param name="nPort"></param>
        /// <returns></returns>
        // Token: 0x060000CD RID: 205 RVA: 0x00002C9C File Offset: 0x00000E9C
        public int MV_GIGE_SetGevSCSP_NET(uint nPort)
        {
            return MV_GIGE_SetGevSCSP(handle, nPort);
        }

        /// <summary>
        /// Get Camera Handle
        /// </summary>
        /// <returns></returns>
        // Token: 0x060000CE RID: 206 RVA: 0x00002CAA File Offset: 0x00000EAA
        public IntPtr GetCameraHandle()
        {
            return handle;
        }

        /// <summary>
        /// Byte array to struct
        /// </summary>
        /// <param name="bytes">Byte array</param>
        /// <param name="type">Struct type</param>
        /// <returns>Struct object</returns>
        // Token: 0x060000CF RID: 207 RVA: 0x00002CB4 File Offset: 0x00000EB4
        public static object ByteToStruct(byte[] bytes, Type type)
        {
            int num = Marshal.SizeOf(type);
            if (num > bytes.Length)
            {
                return null;
            }
            IntPtr intPtr = Marshal.AllocHGlobal(num);
            Marshal.Copy(bytes, 0, intPtr, num);
            object result = Marshal.PtrToStructure(intPtr, type);
            Marshal.FreeHGlobal(intPtr);
            return result;
        }

        // Token: 0x060000D0 RID: 208
        [DllImport("MvCameraControl.dll")]
        private static extern uint MV_CC_GetSDKVersion();

        // Token: 0x060000D1 RID: 209
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_CC_EnumerateTls();

        // Token: 0x060000D2 RID: 210
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_CC_EnumDevices(uint nTLayerType, ref MV_CC_DEVICE_INFO_LIST stDevList);

        // Token: 0x060000D3 RID: 211
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_CC_EnumDevicesEx(uint nTLayerType, ref MV_CC_DEVICE_INFO_LIST stDevList, string pManufacturerName);

        // Token: 0x060000D4 RID: 212
        [DllImport("MvCameraControl.dll")]
        private static extern bool MV_CC_IsDeviceAccessible(ref MV_CC_DEVICE_INFO stDevInfo, uint nAccessMode);

        // Token: 0x060000D5 RID: 213
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_CC_SetSDKLogPath(string pSDKLogPath);

        // Token: 0x060000D6 RID: 214
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_CC_CreateHandle(ref IntPtr handle, ref MV_CC_DEVICE_INFO stDevInfo);

        // Token: 0x060000D7 RID: 215
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_CC_CreateHandleWithoutLog(ref IntPtr handle, ref MV_CC_DEVICE_INFO stDevInfo);

        // Token: 0x060000D8 RID: 216
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_CC_DestroyHandle(IntPtr handle);

        // Token: 0x060000D9 RID: 217
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_CC_OpenDevice(IntPtr handle, uint nAccessMode, ushort nSwitchoverKey);

        // Token: 0x060000DA RID: 218
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_CC_CloseDevice(IntPtr handle);

        // Token: 0x060000DB RID: 219
        [DllImport("MvCameraControl.dll")]
        private static extern bool MV_CC_IsDeviceConnected(IntPtr handle);

        // Token: 0x060000DC RID: 220
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_CC_RegisterImageCallBackEx(IntPtr handle, cbOutputExdelegate cbOutput, IntPtr pUser);

        // Token: 0x060000DD RID: 221
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_CC_RegisterImageCallBackForRGB(IntPtr handle, cbOutputExdelegate cbOutput, IntPtr pUser);

        // Token: 0x060000DE RID: 222
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_CC_RegisterImageCallBackForBGR(IntPtr handle, cbOutputExdelegate cbOutput, IntPtr pUser);

        // Token: 0x060000DF RID: 223
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_CC_StartGrabbing(IntPtr handle);

        // Token: 0x060000E0 RID: 224
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_CC_StopGrabbing(IntPtr handle);

        // Token: 0x060000E1 RID: 225
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_CC_GetImageForRGB(IntPtr handle, IntPtr pData, uint nDataSize, ref MV_FRAME_OUT_INFO_EX pFrameInfo, int nMsec);

        // Token: 0x060000E2 RID: 226
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_CC_GetImageForBGR(IntPtr handle, IntPtr pData, uint nDataSize, ref MV_FRAME_OUT_INFO_EX pFrameInfo, int nMsec);

        // Token: 0x060000E3 RID: 227
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_CC_GetImageBuffer(IntPtr handle, ref MV_FRAME_OUT pFrame, int nMsec);

        // Token: 0x060000E4 RID: 228
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_CC_FreeImageBuffer(IntPtr handle, ref MV_FRAME_OUT pFrame);

        // Token: 0x060000E5 RID: 229
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_CC_GetOneFrameTimeout(IntPtr handle, IntPtr pData, uint nDataSize, ref MV_FRAME_OUT_INFO_EX pFrameInfo, int nMsec);

        // Token: 0x060000E6 RID: 230
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_CC_ClearImageBuffer(IntPtr handle);

        // Token: 0x060000E7 RID: 231
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_CC_Display(IntPtr handle, IntPtr hWnd);

        // Token: 0x060000E8 RID: 232
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_CC_DisplayOneFrame(IntPtr handle, ref MV_DISPLAY_FRAME_INFO pDisplayInfo);

        // Token: 0x060000E9 RID: 233
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_CC_SetImageNodeNum(IntPtr handle, uint nNum);

        // Token: 0x060000EA RID: 234
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_CC_SetGrabStrategy(IntPtr handle, MV_GRAB_STRATEGY enGrabStrategy);

        // Token: 0x060000EB RID: 235
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_CC_SetOutputQueueSize(IntPtr handle, uint nOutputQueueSize);

        // Token: 0x060000EC RID: 236
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_CC_GetImageInfo(IntPtr handle, ref MV_IMAGE_BASIC_INFO pstInfo);

        // Token: 0x060000ED RID: 237
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_CC_GetDeviceInfo(IntPtr handle, ref MV_CC_DEVICE_INFO pstDevInfo);

        // Token: 0x060000EE RID: 238
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_CC_GetAllMatchInfo(IntPtr handle, ref MV_ALL_MATCH_INFO pstInfo);

        // Token: 0x060000EF RID: 239
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_CC_GetIntValue(IntPtr handle, string strValue, ref MVCC_INTVALUE pIntValue);

        // Token: 0x060000F0 RID: 240
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_CC_GetIntValueEx(IntPtr handle, string strValue, ref MVCC_INTVALUE_EX pIntValue);

        // Token: 0x060000F1 RID: 241
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_CC_SetIntValue(IntPtr handle, string strValue, uint nValue);

        // Token: 0x060000F2 RID: 242
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_CC_SetIntValueEx(IntPtr handle, string strValue, long nValue);

        // Token: 0x060000F3 RID: 243
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_CC_GetEnumValue(IntPtr handle, string strValue, ref MVCC_ENUMVALUE pEnumValue);

        // Token: 0x060000F4 RID: 244
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_CC_SetEnumValue(IntPtr handle, string strValue, uint nValue);

        // Token: 0x060000F5 RID: 245
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_CC_SetEnumValueByString(IntPtr handle, string strValue, string sValue);

        // Token: 0x060000F6 RID: 246
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_CC_GetFloatValue(IntPtr handle, string strValue, ref MVCC_FLOATVALUE pFloatValue);

        // Token: 0x060000F7 RID: 247
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_CC_SetFloatValue(IntPtr handle, string strValue, float fValue);

        // Token: 0x060000F8 RID: 248
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_CC_GetBoolValue(IntPtr handle, string strValue, ref bool pBoolValue);

        // Token: 0x060000F9 RID: 249
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_CC_SetBoolValue(IntPtr handle, string strValue, bool bValue);

        // Token: 0x060000FA RID: 250
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_CC_GetStringValue(IntPtr handle, string strKey, ref MVCC_STRINGVALUE pStringValue);

        // Token: 0x060000FB RID: 251
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_CC_SetStringValue(IntPtr handle, string strKey, string sValue);

        // Token: 0x060000FC RID: 252
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_CC_SetCommandValue(IntPtr handle, string strValue);

        // Token: 0x060000FD RID: 253
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_CC_InvalidateNodes(IntPtr handle);

        // Token: 0x060000FE RID: 254
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_CC_GetWidth(IntPtr handle, ref MVCC_INTVALUE pstValue);

        // Token: 0x060000FF RID: 255
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_CC_SetWidth(IntPtr handle, uint nValue);

        // Token: 0x06000100 RID: 256
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_CC_GetHeight(IntPtr handle, ref MVCC_INTVALUE pstValue);

        // Token: 0x06000101 RID: 257
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_CC_SetHeight(IntPtr handle, uint nValue);

        // Token: 0x06000102 RID: 258
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_CC_GetAOIoffsetX(IntPtr handle, ref MVCC_INTVALUE pstValue);

        // Token: 0x06000103 RID: 259
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_CC_SetAOIoffsetX(IntPtr handle, uint nValue);

        // Token: 0x06000104 RID: 260
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_CC_GetAOIoffsetY(IntPtr handle, ref MVCC_INTVALUE pstValue);

        // Token: 0x06000105 RID: 261
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_CC_SetAOIoffsetY(IntPtr handle, uint nValue);

        // Token: 0x06000106 RID: 262
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_CC_GetAutoExposureTimeLower(IntPtr handle, ref MVCC_INTVALUE pstValue);

        // Token: 0x06000107 RID: 263
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_CC_SetAutoExposureTimeLower(IntPtr handle, uint nValue);

        // Token: 0x06000108 RID: 264
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_CC_GetAutoExposureTimeUpper(IntPtr handle, ref MVCC_INTVALUE pstValue);

        // Token: 0x06000109 RID: 265
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_CC_SetAutoExposureTimeUpper(IntPtr handle, uint nValue);

        // Token: 0x0600010A RID: 266
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_CC_GetBrightness(IntPtr handle, ref MVCC_INTVALUE pstValue);

        // Token: 0x0600010B RID: 267
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_CC_SetBrightness(IntPtr handle, uint nValue);

        // Token: 0x0600010C RID: 268
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_CC_GetFrameRate(IntPtr handle, ref MVCC_FLOATVALUE pstValue);

        // Token: 0x0600010D RID: 269
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_CC_SetFrameRate(IntPtr handle, float fValue);

        // Token: 0x0600010E RID: 270
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_CC_GetGain(IntPtr handle, ref MVCC_FLOATVALUE pstValue);

        // Token: 0x0600010F RID: 271
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_CC_SetGain(IntPtr handle, float fValue);

        // Token: 0x06000110 RID: 272
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_CC_GetExposureTime(IntPtr handle, ref MVCC_FLOATVALUE pstValue);

        // Token: 0x06000111 RID: 273
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_CC_SetExposureTime(IntPtr handle, float fValue);

        // Token: 0x06000112 RID: 274
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_CC_GetPixelFormat(IntPtr handle, ref MVCC_ENUMVALUE pstValue);

        // Token: 0x06000113 RID: 275
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_CC_SetPixelFormat(IntPtr handle, uint nValue);

        // Token: 0x06000114 RID: 276
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_CC_GetAcquisitionMode(IntPtr handle, ref MVCC_ENUMVALUE pstValue);

        // Token: 0x06000115 RID: 277
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_CC_SetAcquisitionMode(IntPtr handle, uint nValue);

        // Token: 0x06000116 RID: 278
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_CC_GetGainMode(IntPtr handle, ref MVCC_ENUMVALUE pstValue);

        // Token: 0x06000117 RID: 279
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_CC_SetGainMode(IntPtr handle, uint nValue);

        // Token: 0x06000118 RID: 280
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_CC_GetExposureAutoMode(IntPtr handle, ref MVCC_ENUMVALUE pstValue);

        // Token: 0x06000119 RID: 281
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_CC_SetExposureAutoMode(IntPtr handle, uint nValue);

        // Token: 0x0600011A RID: 282
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_CC_GetTriggerMode(IntPtr handle, ref MVCC_ENUMVALUE pstValue);

        // Token: 0x0600011B RID: 283
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_CC_SetTriggerMode(IntPtr handle, uint nValue);

        // Token: 0x0600011C RID: 284
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_CC_GetTriggerDelay(IntPtr handle, ref MVCC_FLOATVALUE pstValue);

        // Token: 0x0600011D RID: 285
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_CC_SetTriggerDelay(IntPtr handle, float fValue);

        // Token: 0x0600011E RID: 286
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_CC_GetTriggerSource(IntPtr handle, ref MVCC_ENUMVALUE pstValue);

        // Token: 0x0600011F RID: 287
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_CC_SetTriggerSource(IntPtr handle, uint nValue);

        // Token: 0x06000120 RID: 288
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_CC_TriggerSoftwareExecute(IntPtr handle);

        // Token: 0x06000121 RID: 289
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_CC_GetGammaSelector(IntPtr handle, ref MVCC_ENUMVALUE pstValue);

        // Token: 0x06000122 RID: 290
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_CC_SetGammaSelector(IntPtr handle, uint nValue);

        // Token: 0x06000123 RID: 291
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_CC_GetGamma(IntPtr handle, ref MVCC_FLOATVALUE pstValue);

        // Token: 0x06000124 RID: 292
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_CC_SetGamma(IntPtr handle, float fValue);

        // Token: 0x06000125 RID: 293
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_CC_GetSharpness(IntPtr handle, ref MVCC_INTVALUE pstValue);

        // Token: 0x06000126 RID: 294
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_CC_SetSharpness(IntPtr handle, uint nValue);

        // Token: 0x06000127 RID: 295
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_CC_GetHue(IntPtr handle, ref MVCC_INTVALUE pstValue);

        // Token: 0x06000128 RID: 296
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_CC_SetHue(IntPtr handle, uint nValue);

        // Token: 0x06000129 RID: 297
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_CC_GetSaturation(IntPtr handle, ref MVCC_INTVALUE pstValue);

        // Token: 0x0600012A RID: 298
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_CC_SetSaturation(IntPtr handle, uint nValue);

        // Token: 0x0600012B RID: 299
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_CC_GetBalanceWhiteAuto(IntPtr handle, ref MVCC_ENUMVALUE pstValue);

        // Token: 0x0600012C RID: 300
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_CC_SetBalanceWhiteAuto(IntPtr handle, uint nValue);

        // Token: 0x0600012D RID: 301
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_CC_GetBalanceRatioRed(IntPtr handle, ref MVCC_INTVALUE pstValue);

        // Token: 0x0600012E RID: 302
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_CC_SetBalanceRatioRed(IntPtr handle, uint nValue);

        // Token: 0x0600012F RID: 303
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_CC_GetBalanceRatioGreen(IntPtr handle, ref MVCC_INTVALUE pstValue);

        // Token: 0x06000130 RID: 304
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_CC_SetBalanceRatioGreen(IntPtr handle, uint nValue);

        // Token: 0x06000131 RID: 305
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_CC_GetBalanceRatioBlue(IntPtr handle, ref MVCC_INTVALUE pstValue);

        // Token: 0x06000132 RID: 306
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_CC_SetBalanceRatioBlue(IntPtr handle, uint nValue);

        // Token: 0x06000133 RID: 307
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_CC_GetDeviceUserID(IntPtr handle, ref MVCC_STRINGVALUE pstValue);

        // Token: 0x06000134 RID: 308
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_CC_SetDeviceUserID(IntPtr handle, string chValue);

        // Token: 0x06000135 RID: 309
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_CC_GetBurstFrameCount(IntPtr handle, ref MVCC_INTVALUE pstValue);

        // Token: 0x06000136 RID: 310
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_CC_SetBurstFrameCount(IntPtr handle, uint nValue);

        // Token: 0x06000137 RID: 311
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_CC_GetAcquisitionLineRate(IntPtr handle, ref MVCC_INTVALUE pstValue);

        // Token: 0x06000138 RID: 312
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_CC_SetAcquisitionLineRate(IntPtr handle, uint nValue);

        // Token: 0x06000139 RID: 313
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_CC_GetHeartBeatTimeout(IntPtr handle, ref MVCC_INTVALUE pstValue);

        // Token: 0x0600013A RID: 314
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_CC_SetHeartBeatTimeout(IntPtr handle, uint nValue);

        // Token: 0x0600013B RID: 315
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_CC_LocalUpgrade(IntPtr handle, string pFilePathName);

        // Token: 0x0600013C RID: 316
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_CC_GetUpgradeProcess(IntPtr handle, ref uint pnProcess);

        // Token: 0x0600013D RID: 317
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_CC_GetOptimalPacketSize(IntPtr handle);

        // Token: 0x0600013E RID: 318
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_CC_ReadMemory(IntPtr handle, IntPtr pBuffer, long nAddress, long nLength);

        // Token: 0x0600013F RID: 319
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_CC_WriteMemory(IntPtr handle, IntPtr pBuffer, long nAddress, long nLength);

        // Token: 0x06000140 RID: 320
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_CC_RegisterExceptionCallBack(IntPtr handle, cbExceptiondelegate cbException, IntPtr pUser);

        // Token: 0x06000141 RID: 321
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_CC_RegisterEventCallBack(IntPtr handle, cbEventdelegate cbEvent, IntPtr pUser);

        // Token: 0x06000142 RID: 322
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_CC_RegisterAllEventCallBack(IntPtr handle, cbEventdelegateEx cbEvent, IntPtr pUser);

        // Token: 0x06000143 RID: 323
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_CC_RegisterEventCallBackEx(IntPtr handle, string pEventName, cbEventdelegateEx cbEvent, IntPtr pUser);

        // Token: 0x06000144 RID: 324
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_GIGE_ForceIpEx(IntPtr handle, uint nIP, uint nSubNetMask, uint nDefaultGateWay);

        // Token: 0x06000145 RID: 325
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_GIGE_SetIpConfig(IntPtr handle, uint nType);

        // Token: 0x06000146 RID: 326
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_GIGE_SetNetTransMode(IntPtr handle, uint nType);

        // Token: 0x06000147 RID: 327
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_GIGE_GetNetTransInfo(IntPtr handle, ref MV_NETTRANS_INFO pstInfo);

        // Token: 0x06000148 RID: 328
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_GIGE_SetDiscoveryMode(uint nMode);

        // Token: 0x06000149 RID: 329
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_GIGE_SetGvspTimeout(IntPtr handle, uint nMillisec);

        // Token: 0x0600014A RID: 330
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_GIGE_GetGvspTimeout(IntPtr handle, ref uint pMillisec);

        // Token: 0x0600014B RID: 331
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_GIGE_SetGvcpTimeout(IntPtr handle, uint nMillisec);

        // Token: 0x0600014C RID: 332
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_GIGE_GetGvcpTimeout(IntPtr handle, ref uint pMillisec);

        // Token: 0x0600014D RID: 333
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_GIGE_SetRetryGvcpTimes(IntPtr handle, uint nRetryGvcpTimes);

        // Token: 0x0600014E RID: 334
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_GIGE_GetRetryGvcpTimes(IntPtr handle, ref uint pRetryGvcpTimes);

        // Token: 0x0600014F RID: 335
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_GIGE_SetResend(IntPtr handle, uint bEnable, uint nMaxResendPercent, uint nResendTimeout);

        // Token: 0x06000150 RID: 336
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_GIGE_SetResendMaxRetryTimes(IntPtr handle, uint nRetryTimes);

        // Token: 0x06000151 RID: 337
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_GIGE_GetResendMaxRetryTimes(IntPtr handle, ref uint pnRetryTimes);

        // Token: 0x06000152 RID: 338
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_GIGE_SetResendTimeInterval(IntPtr handle, uint nMillisec);

        // Token: 0x06000153 RID: 339
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_GIGE_GetResendTimeInterval(IntPtr handle, ref uint pnMillisec);

        // Token: 0x06000154 RID: 340
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_GIGE_GetGevSCPSPacketSize(IntPtr handle, ref MVCC_INTVALUE pstValue);

        // Token: 0x06000155 RID: 341
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_GIGE_SetGevSCPSPacketSize(IntPtr handle, uint nValue);

        // Token: 0x06000156 RID: 342
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_GIGE_GetGevSCPD(IntPtr handle, ref MVCC_INTVALUE pstValue);

        // Token: 0x06000157 RID: 343
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_GIGE_SetGevSCPD(IntPtr handle, uint nValue);

        // Token: 0x06000158 RID: 344
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_GIGE_GetGevSCDA(IntPtr handle, ref uint pnIP);

        // Token: 0x06000159 RID: 345
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_GIGE_SetGevSCDA(IntPtr handle, uint nIP);

        // Token: 0x0600015A RID: 346
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_GIGE_GetGevSCSP(IntPtr handle, ref uint pnPort);

        // Token: 0x0600015B RID: 347
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_GIGE_SetGevSCSP(IntPtr handle, uint nPort);

        // Token: 0x0600015C RID: 348
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_GIGE_SetTransmissionType(IntPtr handle, ref MV_CC_TRANSMISSION_TYPE pstTransmissionType);

        // Token: 0x0600015D RID: 349
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_GIGE_IssueActionCommand(ref MV_ACTION_CMD_INFO pstActionCmdInfo, ref MV_ACTION_CMD_RESULT_LIST pstActionCmdResults);

        // Token: 0x0600015E RID: 350
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_GIGE_GetMulticastStatus(ref MV_CC_DEVICE_INFO pstDevInfo, ref bool pStatus);

        // Token: 0x0600015F RID: 351
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CAML_SetDeviceBauderate")]
        private static extern int MV_CAML_SetDeviceBaudrate(IntPtr handle, uint nBaudrate);

        // Token: 0x06000160 RID: 352
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CAML_GetDeviceBauderate")]
        private static extern int MV_CAML_GetDeviceBaudrate(IntPtr handle, ref uint pnCurrentBaudrate);

        // Token: 0x06000161 RID: 353
        [DllImport("MvCameraControl.dll", EntryPoint = "MV_CAML_GetSupportBauderates")]
        private static extern int MV_CAML_GetSupportBaudrates(IntPtr handle, ref uint pnBaudrateAblity);

        // Token: 0x06000162 RID: 354
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_CAML_SetGenCPTimeOut(IntPtr handle, uint nMillisec);

        // Token: 0x06000163 RID: 355
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_USB_SetTransferSize(IntPtr handle, uint nTransferSize);

        // Token: 0x06000164 RID: 356
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_USB_GetTransferSize(IntPtr handle, ref uint pTransferSize);

        // Token: 0x06000165 RID: 357
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_USB_SetTransferWays(IntPtr handle, uint nTransferWays);

        // Token: 0x06000166 RID: 358
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_USB_GetTransferWays(IntPtr handle, ref uint pTransferWays);

        // Token: 0x06000167 RID: 359
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_CC_EnumInterfacesByGenTL(ref MV_GENTL_IF_INFO_LIST pstIFInfoList, string sGenTLPath);

        // Token: 0x06000168 RID: 360
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_CC_EnumDevicesByGenTL(ref MV_GENTL_IF_INFO stIFInfo, ref MV_GENTL_DEV_INFO_LIST pstDevList);

        // Token: 0x06000169 RID: 361
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_CC_CreateHandleByGenTL(ref IntPtr handle, ref MV_GENTL_DEV_INFO stDevInfo);

        // Token: 0x0600016A RID: 362
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_XML_GetGenICamXML(IntPtr handle, IntPtr pData, uint nDataSize, ref uint pnDataLen);

        // Token: 0x0600016B RID: 363
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_XML_GetNodeAccessMode(IntPtr handle, string pstrName, ref MV_XML_AccessMode pAccessMode);

        // Token: 0x0600016C RID: 364
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_XML_GetNodeInterfaceType(IntPtr handle, string pstrName, ref MV_XML_InterfaceType pInterfaceType);

        // Token: 0x0600016D RID: 365
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_XML_GetRootNode(IntPtr handle, ref MV_XML_NODE_FEATURE pstNode);

        // Token: 0x0600016E RID: 366
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_XML_GetChildren(IntPtr handle, ref MV_XML_NODE_FEATURE pstNode, IntPtr pstNodesList);

        // Token: 0x0600016F RID: 367
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_XML_GetChildren(IntPtr handle, ref MV_XML_NODE_FEATURE pstNode, ref MV_XML_NODES_LIST pstNodesList);

        // Token: 0x06000170 RID: 368
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_XML_GetNodeFeature(IntPtr handle, ref MV_XML_NODE_FEATURE pstNode, IntPtr pstFeature);

        // Token: 0x06000171 RID: 369
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_XML_UpdateNodeFeature(IntPtr handle, MV_XML_InterfaceType enType, IntPtr pstFeature);

        // Token: 0x06000172 RID: 370
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_XML_RegisterUpdateCallBack(IntPtr handle, cbXmlUpdatedelegate cbXmlUpdate, IntPtr pUser);

        // Token: 0x06000173 RID: 371
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_CC_SaveImageEx2(IntPtr handle, ref MV_SAVE_IMAGE_PARAM_EX stSaveParam);

        // Token: 0x06000174 RID: 372
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_CC_ConvertPixelType(IntPtr handle, ref MV_PIXEL_CONVERT_PARAM pstCvtParam);

        // Token: 0x06000175 RID: 373
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_CC_SetBayerCvtQuality(IntPtr handle, uint BayerCvtQuality);

        // Token: 0x06000176 RID: 374
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_CC_SetBayerGammaValue(IntPtr handle, float fBayerGammaValue);

        // Token: 0x06000177 RID: 375
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_CC_SetBayerGammaParam(IntPtr handle, ref MV_CC_GAMMA_PARAM pstGammaParam);

        // Token: 0x06000178 RID: 376
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_CC_SetBayerCCMParam(IntPtr handle, ref MV_CC_CCM_PARAM pstCCMParam);

        // Token: 0x06000179 RID: 377
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_CC_SetBayerCCMParamEx(IntPtr handle, ref MV_CC_CCM_PARAM_EX pstCCMParam);

        // Token: 0x0600017A RID: 378
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_CC_SetBayerCLUTParam(IntPtr handle, ref MV_CC_CLUT_PARAM pstCLUTParam);

        // Token: 0x0600017B RID: 379
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_CC_ImageContrast(IntPtr handle, ref MV_CC_CONTRAST_PARAM pstContrastParam);

        // Token: 0x0600017C RID: 380
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_CC_ImageSharpen(IntPtr handle, ref MV_CC_SHARPEN_PARAM pstSharpenParam);

        // Token: 0x0600017D RID: 381
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_CC_ColorCorrect(IntPtr handle, ref MV_CC_COLOR_CORRECT_PARAM pstColorCorrectParam);

        // Token: 0x0600017E RID: 382
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_CC_NoiseEstimate(IntPtr handle, ref MV_CC_NOISE_ESTIMATE_PARAM pstNoiseEstimateParam);

        // Token: 0x0600017F RID: 383
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_CC_SpatialDenoise(IntPtr handle, ref MV_CC_SPATIAL_DENOISE_PARAM pstSpatialDenoiseParam);

        // Token: 0x06000180 RID: 384
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_CC_LSCCalib(IntPtr handle, ref MV_CC_LSC_CALIB_PARAM pstLSCCalibParam);

        // Token: 0x06000181 RID: 385
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_CC_LSCCorrect(IntPtr handle, ref MV_CC_LSC_CORRECT_PARAM pstLSCCorrectParam);

        // Token: 0x06000182 RID: 386
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_CC_HB_Decode(IntPtr handle, ref MV_CC_HB_DECODE_PARAM pstDecodeParam);

        // Token: 0x06000183 RID: 387
        [DllImport("MvCameraControl.dll")]
        private static extern IntPtr MV_CC_GetTlProxy(IntPtr handle);

        // Token: 0x06000184 RID: 388
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_CC_FeatureSave(IntPtr handle, string pFileName);

        // Token: 0x06000185 RID: 389
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_CC_FeatureLoad(IntPtr handle, string pFileName);

        // Token: 0x06000186 RID: 390
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_CC_FileAccessRead(IntPtr handle, ref MV_CC_FILE_ACCESS pstFileAccess);

        // Token: 0x06000187 RID: 391
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_CC_FileAccessWrite(IntPtr handle, ref MV_CC_FILE_ACCESS pstFileAccess);

        // Token: 0x06000188 RID: 392
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_CC_GetFileAccessProgress(IntPtr handle, ref MV_CC_FILE_ACCESS_PROGRESS pstFileAccessProgress);

        // Token: 0x06000189 RID: 393
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_CC_StartRecord(IntPtr handle, ref MV_CC_RECORD_PARAM pstRecordParam);

        // Token: 0x0600018A RID: 394
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_CC_InputOneFrame(IntPtr handle, ref MV_CC_INPUT_FRAME_INFO pstInputFrameInfo);

        // Token: 0x0600018B RID: 395
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_CC_StopRecord(IntPtr handle);

        // Token: 0x0600018C RID: 396
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_CC_SaveImageToFile(IntPtr handle, ref MV_SAVE_IMG_TO_FILE_PARAM pstSaveFileParam);

        // Token: 0x0600018D RID: 397
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_CC_SavePointCloudData(IntPtr handle, ref MV_SAVE_POINT_CLOUD_PARAM pstPointDataParam);

        // Token: 0x0600018E RID: 398
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_CC_RotateImage(IntPtr handle, ref MV_CC_ROTATE_IMAGE_PARAM pstRotateParam);

        // Token: 0x0600018F RID: 399
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_CC_FlipImage(IntPtr handle, ref MV_CC_FLIP_IMAGE_PARAM pstFlipParam);

        // Token: 0x06000190 RID: 400
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_CC_GetOneFrame(IntPtr handle, IntPtr pData, uint nDataSize, ref MV_FRAME_OUT_INFO pFrameInfo);

        // Token: 0x06000191 RID: 401
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_CC_GetOneFrameEx(IntPtr handle, IntPtr pData, uint nDataSize, ref MV_FRAME_OUT_INFO_EX pFrameInfo);

        // Token: 0x06000192 RID: 402
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_CC_RegisterImageCallBack(IntPtr handle, cbOutputdelegate cbOutput, IntPtr pUser);

        // Token: 0x06000193 RID: 403
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_CC_SaveImage(ref MV_SAVE_IMAGE_PARAM stSaveParam);

        // Token: 0x06000194 RID: 404
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_GIGE_ForceIp(IntPtr handle, uint nIP);

        // Token: 0x06000195 RID: 405
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_CC_BayerNoiseEstimate(IntPtr handle, ref MV_CC_BAYER_NOISE_ESTIMATE_PARAM pstNoiseEstimateParam);

        // Token: 0x06000196 RID: 406
        [DllImport("MvCameraControl.dll")]
        private static extern int MV_CC_BayerSpatialDenoise(IntPtr handle, ref MV_CC_BAYER_SPATIAL_DENOISE_PARAM pstSpatialDenoiseParam);

        /// <summary>
        /// 
        /// </summary>
        public const int MV_UNKNOW_DEVICE = 0;

        /// <summary>Successed, no error</summary>
        /// <summary/>
        public const int MV_OK = 0;

        /// <summary>GigE Device</summary>
        /// <summary/>
        public const int MV_GIGE_DEVICE = 1;

        /// <summary>1394-a/b Device</summary>
        /// <summary/>
        public const int MV_1394_DEVICE = 2;

        /// <summary>USB3.0 Device</summary>
        /// <summary/>
        public const int MV_USB_DEVICE = 4;

        /// <summary>CameraLink Device</summary>
        /// <summary/>
        public const int MV_CAMERALINK_DEVICE = 8;

        /// <summary>处理正确</summary>
        public const int MV_ALG_OK = 0;

        /// <summary>
        /// ch:信息结构体的最大缓存 | en: Max buffer size of information structs
        /// </summary>
        /// <summary/>
        public const int INFO_MAX_BUFFER_SIZE = 64;

        /// <summary>
        /// 
        /// </summary>
        public const int MV_MAX_DEVICE_NUM = 256;

        /// <summary>
        /// ch:最大Interface数量 | en:Max num of interfaces
        /// </summary>
        /// <summary/>
        public const int MV_MAX_GENTL_IF_NUM = 256;

        /// <summary>
        /// ch:最大GenTL设备数量 | en:Max num of GenTL devices
        /// </summary>
        /// <summary/>
        public const int MV_MAX_GENTL_DEV_NUM = 256;

        /// <summary>
        /// 
        /// </summary>
        public const int MV_IP_CFG_STATIC = 83886080;

        /// <summary>
        /// 
        /// </summary>
        public const int MV_IP_CFG_DHCP = 100663296;

        /// <summary>
        /// 
        /// </summary>
        public const int MV_IP_CFG_LLA = 67108864;

        /// <summary>
        /// 
        /// </summary>
        public const int MV_NET_TRANS_DRIVER = 1;

        /// <summary>
        /// 
        /// </summary>
        public const int MV_NET_TRANS_SOCKET = 2;

        /// <summary>
        /// 
        /// </summary>
        public const int MV_CAML_BAUDRATE_9600 = 1;

        /// <summary>
        /// 
        /// </summary>
        public const int MV_CAML_BAUDRATE_19200 = 2;

        /// <summary>
        /// 
        /// </summary>
        public const int MV_CAML_BAUDRATE_38400 = 4;

        /// <summary>
        /// 
        /// </summary>
        public const int MV_CAML_BAUDRATE_57600 = 8;

        /// <summary>
        /// 
        /// </summary>
        public const int MV_CAML_BAUDRATE_115200 = 16;

        /// <summary>
        /// 
        /// </summary>
        public const int MV_CAML_BAUDRATE_230400 = 32;

        /// <summary>
        /// 
        /// </summary>
        public const int MV_CAML_BAUDRATE_460800 = 64;

        /// <summary>
        /// 
        /// </summary>
        public const int MV_CAML_BAUDRATE_921600 = 128;

        /// <summary>
        /// 
        /// </summary>
        public const int MV_CAML_BAUDRATE_AUTOMAX = 1073741824;

        /// <summary>
        /// 
        /// </summary>
        public const int MV_MATCH_TYPE_NET_DETECT = 1;

        /// <summary>
        /// 
        /// </summary>
        public const int MV_MATCH_TYPE_USB_DETECT = 2;

        /// <summary>
        /// 
        /// </summary>
        public const int MV_MAX_XML_DISC_STRLEN_C = 512;

        /// <summary>
        /// 
        /// </summary>
        public const int MV_MAX_XML_NODE_STRLEN_C = 64;

        /// <summary>
        /// 
        /// </summary>
        public const int MV_MAX_XML_NODE_NUM_C = 128;

        /// <summary>
        /// 
        /// </summary>
        public const int MV_MAX_XML_SYMBOLIC_NUM = 64;

        /// <summary>
        /// 
        /// </summary>
        public const int MV_MAX_XML_STRVALUE_STRLEN_C = 64;

        /// <summary>
        /// 
        /// </summary>
        public const int MV_MAX_XML_PARENTS_NUM = 8;

        /// <summary>
        /// 
        /// </summary>
        public const int MV_MAX_XML_SYMBOLIC_STRLEN_C = 64;

        /// <summary>
        /// 
        /// </summary>
        public const int MV_EXCEPTION_DEV_DISCONNECT = 32769;

        /// <summary/>
        public const int MV_EXCEPTION_VERSION_CHECK = 32770;

        /// <summary/>
        public const int MV_ACCESS_Exclusive = 1;

        /// <summary/>
        public const int MV_ACCESS_ExclusiveWithSwitch = 2;

        /// <summary/>
        public const int MV_ACCESS_Control = 3;

        /// <summary/>
        public const int MV_ACCESS_ControlWithSwitch = 4;

        /// <summary/>
        public const int MV_ACCESS_ControlSwitchEnable = 5;

        /// <summary/>
        public const int MV_ACCESS_ControlSwitchEnableWithKey = 6;

        /// <summary/>
        public const int MV_ACCESS_Monitor = 7;

        /// <summary/>
        public const int MAX_EVENT_NAME_SIZE = 128;

        /// <summary/>
        private IntPtr handle;

        /// <summary>
        /// Grab callback
        /// </summary>
        /// <param name="pData">Image data</param>
        /// <param name="pFrameInfo">Frame info</param>
        /// <param name="pUser">User defined variable</param>
        // Token: 0x02000003 RID: 3
        // (Invoke) Token: 0x06000198 RID: 408
        public delegate void cbOutputdelegate(IntPtr pData, ref MV_FRAME_OUT_INFO pFrameInfo, IntPtr pUser);

        /// <summary>
        /// Grab callback
        /// </summary>
        /// <param name="pData">Image data</param>
        /// <param name="pFrameInfo">Frame info</param>
        /// <param name="pUser">User defined variable</param>
        // Token: 0x02000004 RID: 4
        // (Invoke) Token: 0x0600019C RID: 412
        public delegate void cbOutputExdelegate(IntPtr pData, ref MV_FRAME_OUT_INFO_EX pFrameInfo, IntPtr pUser);

        /// <summary>
        /// Xml Update callback(Interfaces not recommended)
        /// </summary>
        /// <param name="enType">Node type</param>
        /// <param name="pstFeature">Current node feature structure</param>
        /// <param name="pstNodesList">Nodes list</param>
        /// <param name="pUser">User defined variable</param>
        // Token: 0x02000005 RID: 5
        // (Invoke) Token: 0x060001A0 RID: 416
        public delegate void cbXmlUpdatedelegate(MV_XML_InterfaceType enType, IntPtr pstFeature, ref MV_XML_NODES_LIST pstNodesList, IntPtr pUser);

        /// <summary>
        /// Exception callback
        /// </summary>
        /// <param name="nMsgType">Msg type</param>
        /// <param name="pUser">User defined variable</param>
        // Token: 0x02000006 RID: 6
        // (Invoke) Token: 0x060001A4 RID: 420
        public delegate void cbExceptiondelegate(uint nMsgType, IntPtr pUser);

        /// <summary>
        /// Event callback (Interfaces not recommended)
        /// </summary>
        /// <param name="nUserDefinedId">User defined ID</param>
        /// <param name="pUser">User defined variable</param>
        // Token: 0x02000007 RID: 7
        // (Invoke) Token: 0x060001A8 RID: 424
        public delegate void cbEventdelegate(uint nUserDefinedId, IntPtr pUser);

        /// <summary>
        /// Event callback
        /// </summary>
        /// <param name="pEventInfo">Event Info</param>
        /// <param name="pUser">User defined variable</param>
        // Token: 0x02000008 RID: 8
        // (Invoke) Token: 0x060001AC RID: 428
        public delegate void cbEventdelegateEx(ref MV_EVENT_OUT_INFO pEventInfo, IntPtr pUser);

        /// <summary>
        /// ch: GigE设备信息 | en: GigE device information
        /// </summary>
        // Token: 0x02000009 RID: 9
        public struct MV_GIGE_DEVICE_INFO_EX
        {
            /// <summary/>
            public uint nIpCfgOption;

            /// <summary/>
            public uint nIpCfgCurrent;

            /// <summary/>
            public uint nCurrentIp;

            /// <summary/>
            public uint nCurrentSubNetMask;

            /// <summary/>
            public uint nDefultGateWay;

            /// <summary/>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string chManufacturerName;

            /// <summary/>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string chModelName;

            /// <summary/>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string chDeviceVersion;

            /// <summary/>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 48)]
            public string chManufacturerSpecificInfo;

            /// <summary/>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
            public string chSerialNumber;

            /// <summary/>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public byte[] chUserDefinedName;

            /// <summary/>
            public uint nNetExport;

            /// <summary/>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public uint[] nReserved;
        }

        // Token: 0x0200000A RID: 10
        public struct MV_GIGE_DEVICE_INFO
        {
            /// <summary/>
            public uint nIpCfgOption;

            /// <summary/>
            public uint nIpCfgCurrent;

            /// <summary/>
            public uint nCurrentIp;

            /// <summary/>
            public uint nCurrentSubNetMask;

            /// <summary/>
            public uint nDefultGateWay;

            /// <summary/>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string chManufacturerName;

            /// <summary/>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string chModelName;

            /// <summary/>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string chDeviceVersion;

            /// <summary/>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 48)]
            public string chManufacturerSpecificInfo;

            /// <summary/>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
            public string chSerialNumber;

            /// <summary/>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
            public string chUserDefinedName;

            /// <summary/>
            public uint nNetExport;

            /// <summary/>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public uint[] nReserved;
        }

        /// <summary>
        /// ch:USB3 设备信息 | en:USB3 device information
        /// </summary>
        // Token: 0x0200000B RID: 11
        public struct MV_USB3_DEVICE_INFO_EX
        {
            /// <summary/>
            public byte CrtlInEndPoint;

            /// <summary/>
            public byte CrtlOutEndPoint;

            /// <summary/>
            public byte StreamEndPoint;

            /// <summary/>
            public byte EventEndPoint;

            /// <summary/>
            public ushort idVendor;

            /// <summary/>
            public ushort idProduct;

            /// <summary/>
            public uint nDeviceNumber;

            /// <summary/>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string chDeviceGUID;

            /// <summary/>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string chVendorName;

            /// <summary/>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string chModelName;

            /// <summary/>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string chFamilyName;

            /// <summary/>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string chDeviceVersion;

            /// <summary/>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string chManufacturerName;

            /// <summary/>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string chSerialNumber;

            /// <summary/>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
            public byte[] chUserDefinedName;

            /// <summary/>
            public uint nbcdUSB;

            /// <summary/>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public uint[] nReserved;
        }

        // Token: 0x0200000C RID: 12
        public struct MV_USB3_DEVICE_INFO
        {
            /// <summary/>
            public byte CrtlInEndPoint;

            /// <summary/>
            public byte CrtlOutEndPoint;

            /// <summary/>
            public byte StreamEndPoint;

            /// <summary/>
            public byte EventEndPoint;

            /// <summary/>
            public ushort idVendor;

            /// <summary/>
            public ushort idProduct;

            /// <summary/>
            public uint nDeviceNumber;

            /// <summary/>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string chDeviceGUID;

            /// <summary/>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string chVendorName;

            /// <summary/>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string chModelName;

            /// <summary/>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string chFamilyName;

            /// <summary/>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string chDeviceVersion;

            /// <summary/>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string chManufacturerName;

            /// <summary/>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string chSerialNumber;

            /// <summary/>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string chUserDefinedName;

            /// <summary/>
            public uint nbcdUSB;

            /// <summary/>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public uint[] nReserved;
        }

        /// <summary>
        /// ch:CamLink设备信息 | en:CamLink device information
        /// </summary>
        // Token: 0x0200000D RID: 13
        public struct MV_CamL_DEV_INFO
        {
            /// <summary/>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string chPortID;

            /// <summary/>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string chModelName;

            /// <summary/>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string chFamilyName;

            /// <summary/>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string chDeviceVersion;

            /// <summary/>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string chManufacturerName;

            /// <summary/>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string chSerialNumber;

            /// <summary/>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 38)]
            public uint[] nReserved;
        }

        /// <summary>
        /// ch:设备信息 | en:Device information
        /// </summary>
        // Token: 0x0200000E RID: 14
        public struct MV_CC_DEVICE_INFO
        {
            /// <summary/>
            public ushort nMajorVer;

            /// <summary/>
            public ushort nMinorVer;

            /// <summary/>
            public uint nMacAddrHigh;

            /// MAC 地址
            /// <summary/>
            public uint nMacAddrLow;

            /// <summary/>
            public uint nTLayerType;

            /// <summary/>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public uint[] nReserved;

            /// <summary/>
            public MV_CC_DEVICE_INFO.SPECIAL_INFO SpecialInfo;

            /// <summary>
            /// ch:特定类型的设备信息 | en:Special devcie information
            /// </summary>
            // Token: 0x0200000F RID: 15
            [StructLayout(LayoutKind.Explicit, Size = 540)]
            public struct SPECIAL_INFO
            {
                /// <summary/>
                [FieldOffset(0)]
                [MarshalAs(UnmanagedType.ByValArray, SizeConst = 216)]
                public byte[] stGigEInfo;

                /// <summary/>
                [FieldOffset(0)]
                [MarshalAs(UnmanagedType.ByValArray, SizeConst = 536)]
                public byte[] stCamLInfo;

                /// <summary/>
                [FieldOffset(0)]
                [MarshalAs(UnmanagedType.ByValArray, SizeConst = 540)]
                public byte[] stUsb3VInfo;
            }
        }

        // Token: 0x02000010 RID: 16
        public struct MV_CC_DEVICE_INFO_LIST
        {
            /// <summary/>
            public uint nDeviceNum;

            /// <summary/>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
            public IntPtr[] pDeviceInfo;
        }

        /// <summary>
        /// ch:通过GenTL枚举到的Interface信息 | en:Interface Information with GenTL
        /// </summary>
        // Token: 0x02000011 RID: 17
        public struct MV_GENTL_IF_INFO
        {
            /// <summary/>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string chInterfaceID;

            /// <summary/>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string chTLType;

            /// <summary/>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string chDisplayName;

            /// <summary/>
            public uint nCtiIndex;

            /// <summary/>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public uint[] nReserved;
        }

        /// <summary>
        /// ch:通过GenTL枚举到的设备信息列表 | en:Interface Information List with GenTL
        /// </summary>
        // Token: 0x02000012 RID: 18
        public struct MV_GENTL_IF_INFO_LIST
        {
            /// <summary/>
            public uint nInterfaceNum;

            /// <summary/>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
            public IntPtr[] pIFInfo;
        }

        /// <summary>
        /// ch:通过GenTL枚举到的设备信息 | en:Device Information discovered by with GenTL
        /// </summary>
        // Token: 0x02000013 RID: 19
        public struct MV_GENTL_DEV_INFO
        {
            /// <summary/>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string chInterfaceID;

            /// <summary/>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string chDeviceID;

            /// <summary/>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string chVendorName;

            /// <summary/>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string chModelName;

            /// <summary/>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string chTLType;

            /// <summary/>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string chDisplayName;

            /// <summary/>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string chUserDefinedName;

            /// <summary/>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string chSerialNumber;

            /// <summary/>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string chDeviceVersion;

            /// <summary/>
            public uint nCtiIndex;

            /// <summary/>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public uint[] nReserved;
        }

        /// <summary>
        /// ch:GenTL设备列表 | en:GenTL devices list
        /// </summary>
        // Token: 0x02000014 RID: 20
        public struct MV_GENTL_DEV_INFO_LIST
        {
            /// <summary/>
            public uint nDeviceNum;

            /// <summary/>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
            public IntPtr[] pDeviceInfo;
        }

        // Token: 0x02000015 RID: 21
        public struct MV_NETTRANS_INFO
        {
            /// <summary/>
            public long nReviceDataSize;

            /// <summary/>
            public int nThrowFrameCount;

            /// <summary/>
            public uint nNetRecvFrameCount;

            /// <summary/>
            public long nRequestResendPacketCount;

            /// <summary/>
            public long nResendPacketCount;
        }

        // Token: 0x02000016 RID: 22
        public struct MV_FRAME_OUT_INFO
        {
            /// <summary/>
            public ushort nWidth;

            /// <summary/>
            public ushort nHeight;

            /// <summary/>
            public MvGvspPixelType enPixelType;

            /// <summary/>
            public uint nFrameNum;

            /// <summary/>
            public uint nDevTimeStampHigh;

            /// <summary/>
            public uint nDevTimeStampLow;

            /// <summary/>
            public uint nReserved0;

            /// <summary/>
            public long nHostTimeStamp;

            /// <summary/>
            public uint nFrameLen;

            /// <summary/>
            public uint nLostPacket;

            /// <summary/>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public uint[] nReserved;
        }

        // Token: 0x02000017 RID: 23
        public struct MV_CHUNK_DATA_CONTENT
        {
            /// <summary/>
            public IntPtr pChunkData;

            /// <summary/>
            public uint nChunkID;

            /// <summary/>
            public uint nChunkLen;

            /// <summary/>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public uint[] nReserved;
        }

        // Token: 0x02000018 RID: 24
        public struct MV_FRAME_OUT_INFO_EX
        {
            /// <summary/>
            public ushort nWidth;

            /// <summary/>
            public ushort nHeight;

            /// <summary/>
            public MvGvspPixelType enPixelType;

            /// <summary/>
            public uint nFrameNum;

            /// <summary/>
            public uint nDevTimeStampHigh;

            /// <summary/>
            public uint nDevTimeStampLow;

            /// <summary/>
            public uint nReserved0;

            /// <summary/>
            public long nHostTimeStamp;

            /// <summary/>
            public uint nFrameLen;

            /// <summary/>
            public uint nSecondCount;

            /// <summary/>
            public uint nCycleCount;

            /// <summary/>
            public uint nCycleOffset;

            /// <summary/>
            public float fGain;

            /// <summary/>
            public float fExposureTime;

            /// <summary/>
            public uint nAverageBrightness;

            /// <summary/>
            public uint nRed;

            /// <summary/>
            public uint nGreen;

            /// <summary/>
            public uint nBlue;

            /// <summary/>
            public uint nFrameCounter;

            /// <summary/>
            public uint nTriggerIndex;

            /// <summary/>
            public uint nInput;

            /// <summary/>
            public uint nOutput;

            /// <summary/>
            public ushort nOffsetX;

            /// <summary/>
            public ushort nOffsetY;

            /// <summary/>
            public ushort nChunkWidth;

            /// <summary/>
            public ushort nChunkHeight;

            /// <summary/>
            public uint nLostPacket;

            /// <summary/>
            public uint nUnparsedChunkNum;

            /// <summary/>
            public MV_FRAME_OUT_INFO_EX.UNPARSED_CHUNK_LIST UnparsedChunkList;

            /// <summary/>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 36)]
            public uint[] nReserved;

            // Token: 0x02000019 RID: 25
            [StructLayout(LayoutKind.Explicit)]
            public struct UNPARSED_CHUNK_LIST
            {
                /// <summary/>
                [FieldOffset(0)]
                public IntPtr pUnparsedChunkContent;

                /// <summary/>
                [FieldOffset(0)]
                public long nAligning;
            }
        }

        /// <summary/>
        public struct MV_FRAME_OUT
        {
            /// <summary/>
            public IntPtr pBufAddr;

            /// <summary/>
            public MV_FRAME_OUT_INFO_EX stFrameInfo;

            /// <summary/>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public uint[] nReserved;
        }

        /// <summary/>
        public enum MV_GRAB_STRATEGY
        {
            /// <summary/>
            MV_GrabStrategy_OneByOne,
            /// <summary/>
            MV_GrabStrategy_LatestImagesOnly,
            /// <summary/>
            MV_GrabStrategy_LatestImages,
            /// <summary/>
            MV_GrabStrategy_UpcomingImage
        }

        /// <summary/>
        public struct MV_DISPLAY_FRAME_INFO
        {
            /// <summary/>
            public IntPtr hWnd;

            /// <summary/>
            public IntPtr pData;

            /// <summary/>
            public uint nDataLen;

            /// <summary/>
            public ushort nWidth;

            /// <summary/>
            public ushort nHeight;

            /// <summary/>
            public MvGvspPixelType enPixelType;

            /// <summary/>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public uint[] nReserved;
        }

        /// <summary/>
        public enum MV_SAVE_IAMGE_TYPE
        {
            /// <summary/>
            MV_Image_Undefined,
            /// <summary/>
            MV_Image_Bmp,
            /// <summary/>
            MV_Image_Jpeg,
            /// <summary/>
            MV_Image_Png,
            /// <summary/>
            MV_Image_Tif
        }

        /// <summary/>
        public struct MV_SAVE_POINT_CLOUD_PARAM
        {
            /// <summary/>
            public uint nLinePntNum;

            /// <summary/>
            public uint nLineNum;

            /// <summary/>
            public MvGvspPixelType enSrcPixelType;

            /// <summary/>
            public IntPtr pSrcData;

            /// <summary/>
            public uint nSrcDataLen;

            /// <summary/>
            public IntPtr pDstBuf;

            /// <summary/>
            public uint nDstBufSize;

            /// <summary/>
            public uint nDstBufLen;

            /// <summary/>
            public MV_SAVE_POINT_CLOUD_FILE_TYPE enPointCloudFileType;

            /// <summary/>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public uint[] nRes;
        }

        /// <summary/>
        public struct MV_SAVE_IMAGE_PARAM
        {
            /// <summary/>
            public IntPtr pData;

            /// <summary/>
            public uint nDataLen;

            /// <summary/>
            public MvGvspPixelType enPixelType;

            /// <summary/>
            public ushort nWidth;

            /// <summary/>
            public ushort nHeight;

            /// <summary/>
            public IntPtr pImageBuffer;

            /// <summary/>
            public uint nImageLen;

            /// <summary/>
            public uint nBufferSize;

            /// <summary/>
            public MV_SAVE_IAMGE_TYPE enImageType;
        }

        /// <summary/>
        public struct MV_SAVE_IMAGE_PARAM_EX
        {
            /// <summary/>
            public IntPtr pData;

            /// <summary/>
            public uint nDataLen;

            /// <summary/>
            public MvGvspPixelType enPixelType;

            /// <summary/>
            public ushort nWidth;

            /// <summary/>
            public ushort nHeight;

            /// <summary/>
            public IntPtr pImageBuffer;

            /// <summary/>
            public uint nImageLen;

            /// <summary/>
            public uint nBufferSize;

            /// <summary/>
            public MV_SAVE_IAMGE_TYPE enImageType;

            /// <summary/>
            public uint nJpgQuality;

            /// <summary/>
            public uint iMethodValue;

            /// <summary/>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public uint[] nReserved;
        }

        /// <summary/>
        public struct MV_SAVE_IMG_TO_FILE_PARAM
        {
            /// <summary/>
            public MvGvspPixelType enPixelType;

            /// <summary/>
            public IntPtr pData;

            /// <summary/>
            public uint nDataLen;

            /// <summary/>
            public ushort nWidth;

            /// <summary/>
            public ushort nHeight;

            /// <summary/>
            public MV_SAVE_IAMGE_TYPE enImageType;

            /// <summary/>
            public uint nQuality;

            /// <summary/>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string pImagePath;

            /// <summary/>
            public uint iMethodValue;

            /// <summary/>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public uint[] nRes;
        }

        /// <summary/>
        public enum MV_IMG_ROTATION_ANGLE
        {
            /// <summary/>
            MV_IMAGE_ROTATE_90 = 1,
            /// <summary/>
            MV_IMAGE_ROTATE_180,
            /// <summary/>
            MV_IMAGE_ROTATE_270
        }

        /// <summary/>
        public struct MV_CC_ROTATE_IMAGE_PARAM
        {
            /// <summary/>
            public MvGvspPixelType enPixelType;

            /// <summary/>
            public uint nWidth;

            /// <summary/>
            public uint nHeight;

            /// <summary/>
            public IntPtr pSrcData;

            /// <summary/>
            public uint nSrcDataLen;

            /// <summary/>
            public IntPtr pDstBuf;

            /// <summary/>
            public uint nDstBufLen;

            /// <summary/>
            public uint nDstBufSize;

            /// <summary/>
            public MV_IMG_ROTATION_ANGLE enRotationAngle;

            /// <summary/>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public uint[] nRes;
        }

        /// <summary/>
        public enum MV_IMG_FLIP_TYPE
        {
            /// <summary/>
            MV_FLIP_VERTICAL = 1,
            /// <summary/>
            MV_FLIP_HORIZONTAL
        }

        /// <summary/>
        public struct MV_CC_FLIP_IMAGE_PARAM
        {
            /// <summary/>
            public MvGvspPixelType enPixelType;

            /// <summary/>
            public uint nWidth;

            /// <summary/>
            public uint nHeight;

            /// <summary/>
            public IntPtr pSrcData;

            /// <summary/>
            public uint nSrcDataLen;

            /// <summary/>
            public IntPtr pDstBuf;

            /// <summary/>
            public uint nDstBufLen;

            /// <summary/>
            public uint nDstBufSize;

            /// <summary/>
            public MV_IMG_FLIP_TYPE enFlipType;

            /// <summary/>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public uint[] nRes;
        }

        /// <summary/>
        public struct MV_PIXEL_CONVERT_PARAM
        {
            /// <summary/>
            public ushort nWidth;

            /// <summary/>
            public ushort nHeight;

            /// <summary/>
            public MvGvspPixelType enSrcPixelType;

            /// <summary/>
            public IntPtr pSrcData;

            /// <summary/>
            public uint nSrcDataLen;

            /// <summary/>
            public MvGvspPixelType enDstPixelType;

            /// <summary/>
            public IntPtr pDstBuffer;

            /// <summary/>
            public uint nDstLen;

            /// <summary/>
            public uint nDstBufferSize;

            /// <summary/>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public uint[] nRes;
        }

        /// <summary/>
        public enum MV_CC_GAMMA_TYPE
        {
            /// <summary/>
            MV_CC_GAMMA_TYPE_NONE,
            /// <summary/>
            MV_CC_GAMMA_TYPE_VALUE,
            /// <summary/>
            MV_CC_GAMMA_TYPE_USER_CURVE,
            /// <summary/>
            MV_CC_GAMMA_TYPE_LRGB2SRGB,
            /// <summary/>
            MV_CC_GAMMA_TYPE_SRGB2LRGB
        }

        /// <summary/>
        public struct MV_CC_GAMMA_PARAM
        {
            /// <summary/>
            public MV_CC_GAMMA_TYPE enGammaType;

            /// <summary/>
            public float fGammaValue;

            /// <summary/>
            public IntPtr pGammaCurveBuf;

            /// <summary/>
            public uint nGammaCurveBufLen;

            /// <summary/>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public uint[] nRes;
        }

        /// <summary/>
        public struct MV_CC_CCM_PARAM
        {
            /// <summary/>
            public bool bCCMEnable;

            /// <summary/>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)]
            public int[] nCCMat;

            /// <summary/>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public uint[] nRes;
        }

        /// <summary/>
        public struct MV_CC_CCM_PARAM_EX
        {
            /// <summary/>
            public bool bCCMEnable;

            /// <summary/>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)]
            public int[] nCCMat;

            /// <summary/>
            public uint nCCMScale;

            /// <summary/>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public uint[] nRes;
        }

        /// <summary/>
        public struct MV_CC_CLUT_PARAM
        {
            /// <summary/>
            public bool bCLUTEnable;

            /// <summary/>
            public uint nCLUTScale;

            /// <summary/>
            public uint nCLUTSize;

            /// <summary/>
            public IntPtr pCLUTBuf;

            /// <summary/>
            public uint nCLUTBufLen;

            /// <summary/>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public uint[] nRes;
        }

        /// <summary/>
        public struct MV_CC_CONTRAST_PARAM
        {
            /// <summary/>
            public uint nWidth;

            /// <summary/>
            public uint nHeight;

            /// <summary/>
            public IntPtr pSrcBuf;

            /// <summary/>
            public uint nSrcBufLen;

            /// <summary/>
            public MvGvspPixelType enPixelType;

            /// <summary/>
            public IntPtr pDstBuf;

            /// <summary/>
            public uint nDstBufSize;

            /// <summary/>
            public uint nDstBufLen;

            /// <summary/>
            public uint nContrastFactor;

            /// <summary/>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public uint[] nRes;
        }

        /// <summary/>
        public struct MV_CC_SHARPEN_PARAM
        {
            /// <summary/>
            public uint nWidth;

            /// <summary/>
            public uint nHeight;

            /// <summary/>
            public IntPtr pSrcBuf;

            /// <summary/>
            public uint nSrcBufLen;

            /// <summary/>
            public MvGvspPixelType enPixelType;

            /// <summary/>
            public IntPtr pDstBuf;

            /// <summary/>
            public uint nDstBufSize;

            /// <summary/>
            public uint nDstBufLen;

            /// <summary/>
            public uint nSharpenAmount;

            /// <summary/>
            public uint nSharpenRadius;

            /// <summary/>
            public uint nSharpenThreshold;

            /// <summary/>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public uint[] nRes;
        }

        /// <summary/>
        public struct MV_CC_COLOR_CORRECT_PARAM
        {
            /// <summary/>
            public uint nWidth;

            /// <summary/>
            public uint nHeight;

            /// <summary/>
            public IntPtr pSrcBuf;

            /// <summary/>
            public uint nSrcBufLen;

            /// <summary/>
            public MvGvspPixelType enPixelType;

            /// <summary/>
            public IntPtr pDstBuf;

            /// <summary/>
            public uint nDstBufSize;

            /// <summary/>
            public uint nDstBufLen;

            /// <summary/>
            public uint nImageBit;

            /// <summary/>
            public MV_CC_GAMMA_PARAM stGammaParam;

            /// <summary/>
            public MV_CC_CCM_PARAM_EX stCCMParam;

            /// <summary/>
            public MV_CC_CLUT_PARAM stCLUTParam;

            /// <summary/>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public uint[] nRes;
        }

        /// <summary/>
        public struct MV_CC_RECT_I
        {
            /// <summary/>
            public uint nX;

            /// <summary/>
            public uint nY;

            /// <summary/>
            public uint nWidth;

            /// <summary/>
            public uint nHeight;
        }

        /// <summary/>
        public struct MV_CC_NOISE_ESTIMATE_PARAM
        {
            /// <summary/>
            public uint nWidth;

            /// <summary/>
            public uint nHeight;

            /// <summary/>
            public MvGvspPixelType enPixelType;

            /// <summary/>
            public IntPtr pSrcBuf;

            /// <summary/>
            public uint nSrcBufLen;

            /// <summary/>
            public IntPtr pstROIRect;

            /// <summary/>
            public uint nROINum;

            /// <summary/>
            public uint nNoiseThreshold;

            /// <summary/>
            public IntPtr pNoiseProfile;

            /// <summary/>
            public uint nNoiseProfileSize;

            /// <summary/>
            public uint nNoiseProfileLen;

            /// <summary/>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public uint[] nRes;
        }

        /// <summary/>
        public struct MV_CC_SPATIAL_DENOISE_PARAM
        {
            /// <summary/>
            public uint nWidth;

            /// <summary/>
            public uint nHeight;

            /// <summary/>
            public MvGvspPixelType enPixelType;

            /// <summary/>
            public IntPtr pSrcBuf;

            /// <summary/>
            public uint nSrcBufLen;

            /// <summary/>
            public IntPtr pDstBuf;

            /// <summary/>
            public uint nDstBufSize;

            /// <summary/>
            public uint nDstBufLen;

            /// <summary/>
            public IntPtr pNoiseProfile;

            /// <summary/>
            public uint nNoiseProfileLen;

            /// <summary/>
            public uint nBayerDenoiseStrength;

            /// <summary/>
            public uint nBayerSharpenStrength;

            /// <summary/>
            public uint nBayerNoiseCorrect;

            /// <summary/>
            public uint nNoiseCorrectLum;

            /// <summary/>
            public uint nNoiseCorrectChrom;

            /// <summary/>
            public uint nStrengthLum;

            /// <summary/>
            public uint nStrengthChrom;

            /// <summary/>
            public uint nStrengthSharpen;

            /// <summary/>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public uint[] nRes;
        }

        /// <summary/>
        public struct MV_CC_LSC_CALIB_PARAM
        {
            /// <summary/>
            public uint nWidth;

            /// <summary/>
            public uint nHeight;

            /// <summary/>
            public MvGvspPixelType enPixelType;

            /// <summary/>
            public IntPtr pSrcBuf;

            /// <summary/>
            public uint nSrcBufLen;

            /// <summary/>
            public IntPtr pCalibBuf;

            /// <summary/>
            public uint nCalibBufSize;

            /// <summary/>
            public uint nCalibBufLen;

            /// <summary/>
            public uint nSecNumW;

            /// <summary/>
            public uint nSecNumH;

            /// <summary/>
            public uint nPadCoef;

            /// <summary/>
            public uint nCalibMethod;

            /// <summary/>
            public uint nTargetGray;

            /// <summary/>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public uint[] nRes;
        }

        /// <summary/>
        public struct MV_CC_LSC_CORRECT_PARAM
        {
            /// <summary/>
            public uint nWidth;

            /// <summary/>
            public uint nHeight;

            /// <summary/>
            public MvGvspPixelType enPixelType;

            /// <summary/>
            public IntPtr pSrcBuf;

            /// <summary/>
            public uint nSrcBufLen;

            /// <summary/>
            public IntPtr pDstBuf;

            /// <summary/>
            public uint nDstBufSize;

            /// <summary/>
            public uint nDstBufLen;

            /// <summary/>
            public IntPtr pCalibBuf;

            /// <summary/>
            public uint nCalibBufLen;

            /// <summary/>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public uint[] nRes;
        }

        /// <summary/>
        public enum MV_CC_BAYER_NOISE_FEATURE_TYPE
        {
            /// <summary/>
            MV_CC_BAYER_NOISE_FEATURE_TYPE_INVALID,
            /// <summary/>
            MV_CC_BAYER_NOISE_FEATURE_TYPE_PROFILE,
            /// <summary/>
            MV_CC_BAYER_NOISE_FEATURE_TYPE_LEVEL,
            /// <summary/>
            MV_CC_BAYER_NOISE_FEATURE_TYPE_DEFAULT = 2
        }

        /// <summary/>
        public struct MV_CC_BAYER_NOISE_PROFILE_INFO
        {
            /// <summary/>
            public uint nVersion;

            /// <summary/>
            public MV_CC_BAYER_NOISE_FEATURE_TYPE enNoiseFeatureType;

            /// <summary/>
            public MvGvspPixelType enPixelType;

            /// <summary/>
            public int nNoiseLevel;

            /// <summary/>
            public uint nCurvePointNum;

            /// <summary/>
            public IntPtr nNoiseCurve;

            /// <summary/>
            public IntPtr nLumCurve;

            /// <summary/>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public uint[] nRes;
        }

        /// <summary/>
        public struct MV_CC_BAYER_NOISE_ESTIMATE_PARAM
        {
            /// <summary/>
            public uint nWidth;

            /// <summary/>
            public uint nHeight;

            /// <summary/>
            public MvGvspPixelType enPixelType;

            /// <summary/>
            public IntPtr pSrcData;

            /// <summary/>
            public uint nSrcDataLen;

            /// <summary/>
            public uint nNoiseThreshold;

            /// <summary/>
            public IntPtr pCurveBuf;

            /// <summary/>
            public MV_CC_BAYER_NOISE_PROFILE_INFO stNoiseProfile;

            /// <summary/>
            public uint nThreadNum;

            /// <summary/>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public uint[] nRes;
        }

        /// <summary/>
        public struct MV_CC_BAYER_SPATIAL_DENOISE_PARAM
        {
            /// <summary/>
            public uint nWidth;

            /// <summary/>
            public uint nHeight;

            /// <summary/>
            public MvGvspPixelType enPixelType;

            /// <summary/>
            public IntPtr pSrcData;

            /// <summary/>
            public uint nSrcDataLen;

            /// <summary/>
            public IntPtr pDstBuf;

            /// <summary/>
            public uint nDstBufSize;

            /// <summary/>
            public uint nDstBufLen;

            /// <summary/>
            public MV_CC_BAYER_NOISE_PROFILE_INFO stNoiseProfile;

            /// <summary/>
            public uint nDenoiseStrength;

            /// <summary/>
            public uint nSharpenStrength;

            /// <summary/>
            public uint nNoiseCorrect;

            /// <summary/>
            public uint nThreadNum;

            /// <summary/>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public uint[] nRes;
        }

        /// <summary/>
        public struct MV_CC_FRAME_SPEC_INFO
        {
            /// <summary/>
            public uint nSecondCount;

            /// <summary/>
            public uint nCycleCount;

            /// <summary/>
            public uint nCycleOffset;

            /// <summary/>
            public float fGain;

            /// <summary/>
            public float fExposureTime;

            /// <summary/>
            public uint nAverageBrightness;

            /// <summary/>
            public uint nRed;

            /// <summary/>
            public uint nGreen;

            /// <summary/>
            public uint nBlue;

            /// <summary/>
            public uint nFrameCounter;

            /// <summary/>
            public uint nTriggerIndex;

            /// <summary/>
            public uint nInput;

            /// <summary/>
            public uint nOutput;

            /// <summary/>
            public ushort nOffsetX;

            /// <summary/>
            public ushort nOffsetY;

            /// <summary/>
            public ushort nFrameWidth;

            /// <summary/>
            public ushort nFrameHeight;

            /// <summary/>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public uint[] nRes;
        }

        /// <summary/>
        public struct MV_CC_HB_DECODE_PARAM
        {
            /// <summary/>
            public IntPtr pSrcBuf;

            /// <summary/>
            public uint nSrcLen;

            /// <summary/>
            public uint nWidth;

            /// <summary/>
            public uint nHeight;

            /// <summary/>
            public IntPtr pDstBuf;

            /// <summary/>
            public uint nDstBufSize;

            /// <summary/>
            public uint nDstBufLen;

            /// <summary/>
            public MvGvspPixelType enDstPixelType;

            /// <summary/>
            public MV_CC_FRAME_SPEC_INFO stFrameSpecInfo;

            /// <summary/>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public uint[] nRes;
        }

        // Token: 0x0200003A RID: 58
        public enum MV_RECORD_FORMAT_TYPE
        {
            /// <summary/>
            MV_FormatType_Undefined,
            /// <summary/>
            MV_FormatType_AVI
        }

        // Token: 0x0200003B RID: 59
        public enum MV_SAVE_POINT_CLOUD_FILE_TYPE
        {
            /// <summary/>
            MV_PointCloudFile_Undefined,
            /// <summary/>
            MV_PointCloudFile_PLY,
            /// <summary/>
            MV_PointCloudFile_CSV,
            /// <summary/>
            MV_PointCloudFile_OBJ
        }

        // Token: 0x0200003C RID: 60
        public struct MV_CC_RECORD_PARAM
        {
            /// <summary/>
            public MvGvspPixelType enPixelType;

            /// <summary/>
            public ushort nWidth;

            /// <summary/>
            public ushort nHeight;

            /// <summary/>
            public float fFrameRate;

            /// <summary/>
            public uint nBitRate;

            /// <summary/>
            public MV_RECORD_FORMAT_TYPE enRecordFmtType;

            /// <summary/>
            public string strFilePath;

            /// <summary/>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public uint[] nRes;
        }

        // Token: 0x0200003D RID: 61
        public struct MV_CC_INPUT_FRAME_INFO
        {
            /// <summary/>
            public IntPtr pData;

            /// <summary/>
            public uint nDataLen;

            /// <summary/>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public uint[] nRes;
        }

        // Token: 0x0200003E RID: 62
        public enum MV_CAM_ACQUISITION_MODE
        {
            /// <summary/>
            MV_ACQ_MODE_SINGLE,
            /// <summary/>
            MV_ACQ_MODE_MUTLI,
            /// <summary/>
            MV_ACQ_MODE_CONTINUOUS
        }

        // Token: 0x0200003F RID: 63
        public enum MV_CAM_GAIN_MODE
        {
            /// <summary/>
            MV_GAIN_MODE_OFF,
            /// <summary/>
            MV_GAIN_MODE_ONCE,
            /// <summary/>
            MV_GAIN_MODE_CONTINUOUS
        }

        // Token: 0x02000040 RID: 64
        public enum MV_CAM_EXPOSURE_MODE
        {
            /// <summary/>
            MV_EXPOSURE_MODE_TIMED,
            /// <summary/>
            MV_EXPOSURE_MODE_TRIGGER_WIDTH
        }

        // Token: 0x02000041 RID: 65
        public enum MV_CAM_EXPOSURE_AUTO_MODE
        {
            /// <summary/>
            MV_EXPOSURE_AUTO_MODE_OFF,
            /// <summary/>
            MV_EXPOSURE_AUTO_MODE_ONCE,
            /// <summary/>
            MV_EXPOSURE_AUTO_MODE_CONTINUOUS
        }

        // Token: 0x02000042 RID: 66
        public enum MV_CAM_TRIGGER_MODE
        {
            /// <summary/>
            MV_TRIGGER_MODE_OFF,
            /// <summary/>
            MV_TRIGGER_MODE_ON
        }

        // Token: 0x02000043 RID: 67
        public enum MV_CAM_GAMMA_SELECTOR
        {
            /// <summary/>
            MV_GAMMA_SELECTOR_USER = 1,
            /// <summary/>
            MV_GAMMA_SELECTOR_SRGB
        }

        // Token: 0x02000044 RID: 68
        public enum MV_CAM_BALANCEWHITE_AUTO
        {
            /// <summary/>
            MV_BALANCEWHITE_AUTO_OFF,
            /// <summary/>
            MV_BALANCEWHITE_AUTO_ONCE = 2,
            /// <summary/>
            MV_BALANCEWHITE_AUTO_CONTINUOUS = 1
        }

        // Token: 0x02000045 RID: 69
        public enum MV_CAM_TRIGGER_SOURCE
        {
            /// <summary/>
            MV_TRIGGER_SOURCE_LINE0,
            /// <summary/>
            MV_TRIGGER_SOURCE_LINE1,
            /// <summary/>
            MV_TRIGGER_SOURCE_LINE2,
            /// <summary/>
            MV_TRIGGER_SOURCE_LINE3,
            /// <summary/>
            MV_TRIGGER_SOURCE_COUNTER0,
            /// <summary/>
            MV_TRIGGER_SOURCE_SOFTWARE = 7,
            /// <summary/>
            MV_TRIGGER_SOURCE_FrequencyConverter
        }

        // Token: 0x02000046 RID: 70
        public enum MV_GIGE_TRANSMISSION_TYPE
        {
            /// <summary/>
            MV_GIGE_TRANSTYPE_UNICAST,
            /// <summary/>
            MV_GIGE_TRANSTYPE_MULTICAST,
            /// <summary/>
            MV_GIGE_TRANSTYPE_LIMITEDBROADCAST,
            /// <summary/>
            MV_GIGE_TRANSTYPE_SUBNETBROADCAST,
            /// <summary/>
            MV_GIGE_TRANSTYPE_CAMERADEFINED,
            /// <summary/>
            MV_GIGE_TRANSTYPE_UNICAST_DEFINED_PORT,
            /// <summary/>
            MV_GIGE_TRANSTYPE_UNICAST_WITHOUT_RECV = 65536,
            /// <summary/>
            MV_GIGE_TRANSTYPE_MULTICAST_WITHOUT_RECV
        }

        // Token: 0x02000047 RID: 71
        public struct MV_ALL_MATCH_INFO
        {
            /// <summary/>
            public uint nType;

            /// <summary/>
            public IntPtr pInfo;

            /// <summary/>
            public uint nInfoSize;
        }

        // Token: 0x02000048 RID: 72
        public struct MV_MATCH_INFO_NET_DETECT
        {
            /// <summary/>
            public long nReviceDataSize;

            /// <summary/>
            public long nLostPacketCount;

            /// <summary/>
            public uint nLostFrameCount;

            /// <summary/>
            public uint nNetRecvFrameCount;

            /// <summary/>
            public long nRequestResendPacketCount;

            /// <summary/>
            public long nResendPacketCount;
        }

        // Token: 0x02000049 RID: 73
        public struct MV_MATCH_INFO_USB_DETECT
        {
            /// <summary/>
            public long nReviceDataSize;

            /// <summary/>
            public uint nRevicedFrameCount;

            /// <summary/>
            public uint nErrorFrameCount;

            /// <summary/>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public uint[] nReserved;
        }

        // Token: 0x0200004A RID: 74
        public struct MV_IMAGE_BASIC_INFO
        {
            /// <summary/>
            public ushort nWidthValue;

            /// <summary/>
            public ushort nWidthMin;

            /// <summary/>
            public uint nWidthMax;

            /// <summary/>
            public uint nWidthInc;

            /// <summary/>
            public uint nHeightValue;

            /// <summary/>
            public uint nHeightMin;

            /// <summary/>
            public uint nHeightMax;

            /// <summary/>
            public uint nHeightInc;

            /// <summary/>
            public float fFrameRateValue;

            /// <summary/>
            public float fFrameRateMin;

            /// <summary/>
            public float fFrameRateMax;

            /// <summary/>
            public uint enPixelType;

            /// <summary/>
            public uint nSupportedPixelFmtNum;

            /// <summary/>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
            public uint[] enPixelList;

            /// <summary/>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public uint[] nReserved;
        }

        // Token: 0x0200004B RID: 75
        public enum MV_XML_InterfaceType
        {
            /// <summary/>
            IFT_IValue,
            /// <summary/>
            IFT_IBase,
            /// <summary/>
            IFT_IInteger,
            /// <summary/>
            IFT_IBoolean,
            /// <summary/>
            IFT_ICommand,
            /// <summary/>
            IFT_IFloat,
            /// <summary/>
            IFT_IString,
            /// <summary/>
            IFT_IRegister,
            /// <summary/>
            IFT_ICategory,
            /// <summary/>
            IFT_IEnumeration,
            /// <summary/>
            IFT_IEnumEntry,
            /// <summary/>
            IFT_IPort
        }

        // Token: 0x0200004C RID: 76
        public enum MV_XML_AccessMode
        {
            /// <summary/>
            AM_NI,
            /// <summary/>
            AM_NA,
            /// <summary/>
            AM_WO,
            /// <summary/>
            AM_RO,
            /// <summary/>
            AM_RW,
            /// <summary/>
            AM_Undefined,
            /// <summary/>
            AM_CycleDetect
        }

        // Token: 0x0200004D RID: 77
        public enum MV_XML_Visibility
        {
            /// <summary/>
            V_Beginner,
            /// <summary/>
            V_Expert,
            /// <summary/>
            V_Guru,
            /// <summary/>
            V_Invisible,
            /// <summary/>
            V_Undefined = 99
        }

        // Token: 0x0200004E RID: 78
        public struct MV_EVENT_OUT_INFO
        {
            /// <summary/>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string EventName;

            /// <summary/>
            public ushort nEventID;

            /// <summary/>
            public ushort nStreamChannel;

            /// <summary/>
            public uint nBlockIdHigh;

            /// <summary/>
            public uint nBlockIdLow;

            /// <summary/>
            public uint nTimestampHigh;

            /// <summary/>
            public uint nTimestampLow;

            /// <summary/>
            public IntPtr pEventData;

            /// <summary/>
            public uint nEventDataSize;

            /// <summary/>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public uint[] nReserved;
        }

        // Token: 0x0200004F RID: 79
        public struct MV_CC_FILE_ACCESS
        {
            /// <summary/>
            public string pUserFileName;

            /// <summary/>
            public string pDevFileName;

            /// <summary/>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            public uint[] nReserved;
        }

        // Token: 0x02000050 RID: 80
        public struct MV_CC_FILE_ACCESS_PROGRESS
        {
            /// <summary/>
            public long nCompleted;

            /// <summary/>
            public long nTotal;

            /// <summary/>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public uint[] nReserved;
        }

        // Token: 0x02000051 RID: 81
        public struct MV_CC_TRANSMISSION_TYPE
        {
            /// <summary/>
            public MV_GIGE_TRANSMISSION_TYPE enTransmissionType;

            /// <summary/>
            public uint nDestIp;

            /// <summary/>
            public ushort nDestPort;

            /// <summary/>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            public uint[] nReserved;
        }

        // Token: 0x02000052 RID: 82
        public struct MV_ACTION_CMD_INFO
        {
            /// <summary/>
            public uint nDeviceKey;

            /// <summary/>
            public uint nGroupKey;

            /// <summary/>
            public uint nGroupMask;

            /// <summary/>
            public uint bActionTimeEnable;

            /// <summary/>
            public long nActionTime;

            /// <summary/>
            public string pBroadcastAddress;

            /// <summary/>
            public uint nTimeOut;

            /// <summary/>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public uint[] nReserved;
        }

        // Token: 0x02000053 RID: 83
        public struct MV_ACTION_CMD_RESULT
        {
            /// <summary/>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
            public string strDeviceAddress;

            /// <summary/>
            public int nStatus;

            /// <summary/>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public uint[] nReserved;
        }

        // Token: 0x02000054 RID: 84
        public struct MV_ACTION_CMD_RESULT_LIST
        {
            /// <summary/>
            public uint nNumResults;

            /// <summary/>
            public IntPtr pResults;
        }

        // Token: 0x02000055 RID: 85
        public struct MV_XML_NODE_FEATURE
        {
            /// <summary/>
            public MV_XML_InterfaceType enType;

            /// <summary/>
            public MV_XML_Visibility enVisivility;

            /// <summary/>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 512)]
            public string strDescription;

            /// <summary/>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string strDisplayName;

            /// <summary/>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string strName;

            /// <summary/>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 512)]
            public string strToolTip;

            /// <summary/>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public uint[] nReserved;
        }

        // Token: 0x02000056 RID: 86
        public struct MV_XML_NODES_LIST
        {
            /// <summary/>
            public uint nNodeNum;

            /// <summary/>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)]
            public MV_XML_NODE_FEATURE[] stNodes;
        }

        // Token: 0x02000057 RID: 87
        public struct MVCC_INTVALUE
        {
            /// <summary/>
            public uint nCurValue;

            /// <summary/>
            public uint nMax;

            /// <summary/>
            public uint nMin;

            /// <summary/>
            public uint nInc;

            /// <summary/>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public uint[] nReserved;
        }

        // Token: 0x02000058 RID: 88
        public struct MVCC_INTVALUE_EX
        {
            /// <summary/>
            public long nCurValue;

            /// <summary/>
            public long nMax;

            /// <summary/>
            public long nMin;

            /// <summary/>
            public long nInc;

            /// <summary/>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public uint[] nReserved;
        }

        // Token: 0x02000059 RID: 89
        public struct MVCC_FLOATVALUE
        {
            /// <summary/>
            public float fCurValue;

            /// <summary/>
            public float fMax;

            /// <summary/>
            public float fMin;

            /// <summary/>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public uint[] nReserved;
        }

        // Token: 0x0200005A RID: 90
        public struct MVCC_ENUMVALUE
        {
            /// <summary/>
            public uint nCurValue;

            /// <summary/>
            public uint nSupportedNum;

            /// <summary/>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
            public uint[] nSupportValue;

            /// <summary/>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public uint[] nReserved;
        }

        // Token: 0x0200005B RID: 91
        public struct MVCC_STRINGVALUE
        {
            /// <summary/>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string chCurValue;

            /// <summary/>
            public long nMaxLength;

            /// <summary/>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public uint[] nReserved;
        }

        // Token: 0x0200005C RID: 92
        public struct MV_XML_FEATURE_Integer
        {
            /// <summary/>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string strName;

            /// <summary/>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string strDisplayName;

            /// <summary/>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 512)]
            public string strDescription;

            /// <summary/>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 512)]
            public string strToolTip;

            /// <summary/>
            public MV_XML_Visibility enVisivility;

            /// <summary/>
            public MV_XML_AccessMode enAccessMode;

            /// <summary/>
            public int bIsLocked;

            /// <summary/>
            public long nValue;

            /// <summary/>
            public long nMinValue;

            /// <summary/>
            public long nMaxValue;

            /// <summary/>
            public long nIncrement;

            /// <summary/>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public uint[] nReserved;
        }

        // Token: 0x0200005D RID: 93
        public struct MV_XML_FEATURE_Boolean
        {
            /// <summary/>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string strName;

            /// <summary/>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string strDisplayName;

            /// <summary/>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 512)]
            public string strDescription;

            /// <summary/>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 512)]
            public string strToolTip;

            /// <summary/>
            public MV_XML_Visibility enVisivility;

            /// <summary/>
            public MV_XML_AccessMode enAccessMode;

            /// <summary/>
            public int bIsLocked;

            /// <summary/>
            public bool bValue;

            /// <summary/>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public uint[] nReserved;
        }

        // Token: 0x0200005E RID: 94
        public struct MV_XML_FEATURE_Command
        {
            /// <summary/>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string strName;

            /// <summary/>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string strDisplayName;

            /// <summary/>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 512)]
            public string strDescription;

            /// <summary/>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 512)]
            public string strToolTip;

            /// <summary/>
            public MV_XML_Visibility enVisivility;

            /// <summary/>
            public MV_XML_AccessMode enAccessMode;

            /// <summary/>
            public int bIsLocked;

            /// <summary/>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public uint[] nReserved;
        }

        // Token: 0x0200005F RID: 95
        public struct MV_XML_FEATURE_Float
        {
            /// <summary/>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string strName;

            /// <summary/>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string strDisplayName;

            /// <summary/>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 512)]
            public string strDescription;

            /// <summary/>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 512)]
            public string strToolTip;

            /// <summary/>
            public MV_XML_Visibility enVisivility;

            /// <summary/>
            public MV_XML_AccessMode enAccessMode;

            /// <summary/>
            public int bIsLocked;

            /// <summary/>
            public double dfValue;

            /// <summary/>
            public double dfMinValue;

            /// <summary/>
            public double dfMaxValue;

            /// <summary/>
            public double dfIncrement;

            /// <summary/>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public uint[] nReserved;
        }

        // Token: 0x02000060 RID: 96
        public struct MV_XML_FEATURE_String
        {
            /// <summary/>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string strName;

            /// <summary/>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string strDisplayName;

            /// <summary/>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 512)]
            public string strDescription;

            /// <summary/>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 512)]
            public string strToolTip;

            /// <summary/>
            public MV_XML_Visibility enVisivility;

            /// <summary/>
            public MV_XML_AccessMode enAccessMode;

            /// <summary/>
            public int bIsLocked;

            /// <summary/>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string strValue;

            /// <summary/>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public uint[] nReserved;
        }

        // Token: 0x02000061 RID: 97
        public struct MV_XML_FEATURE_Register
        {
            /// <summary/>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string strName;

            /// <summary/>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string strDisplayName;

            /// <summary/>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 512)]
            public string strDescription;

            /// <summary/>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 512)]
            public string strToolTip;

            /// <summary/>
            public MV_XML_Visibility enVisivility;

            /// <summary/>
            public MV_XML_AccessMode enAccessMode;

            /// <summary/>
            public int bIsLocked;

            /// <summary/>
            public long nAddrValue;

            /// <summary/>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public uint[] nReserved;
        }

        // Token: 0x02000062 RID: 98
        public struct MV_XML_FEATURE_Category
        {
            /// <summary/>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 512)]
            public string strDescription;

            /// <summary/>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string strDisplayName;

            /// <summary/>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string strName;

            /// <summary/>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 512)]
            public string strToolTip;

            /// <summary/>
            public MV_XML_Visibility enVisivility;

            /// <summary/>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public uint[] nReserved;
        }

        // Token: 0x02000063 RID: 99
        public struct MV_XML_FEATURE_EnumEntry
        {
            /// <summary/>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string strName;

            /// <summary/>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string strDisplayName;

            /// <summary/>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 512)]
            public string strDescription;

            /// <summary/>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 512)]
            public string strToolTip;

            /// <summary/>
            public int bIsImplemented;

            /// <summary/>
            public int nParentsNum;

            /// <summary/>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public MV_XML_NODE_FEATURE[] stParentsList;

            /// <summary/>
            public MV_XML_Visibility enVisivility;

            /// <summary/>
            public long nValue;

            /// <summary/>
            public MV_XML_AccessMode enAccessMode;

            /// <summary/>
            public int bIsLocked;

            /// <summary/>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public uint[] nReserved;
        }

        // Token: 0x02000064 RID: 100
        public struct StrSymbolic
        {
            /// <summary/>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string str;
        }

        // Token: 0x02000065 RID: 101
        public struct MV_XML_FEATURE_Enumeration
        {
            /// <summary/>
            public MV_XML_Visibility enVisivility;

            /// <summary/>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 512)]
            public string strDescription;

            /// <summary/>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string strDisplayName;

            /// <summary/>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string strName;

            /// <summary/>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 512)]
            public string strToolTip;

            /// <summary/>
            public int nSymbolicNum;

            /// <summary/>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string strCurrentSymbolic;

            /// <summary/>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
            public StrSymbolic[] strSymbolic;

            /// <summary/>
            public MV_XML_AccessMode enAccessMode;

            /// <summary/>
            public int bIsLocked;

            /// <summary/>
            public long nValue;

            /// <summary/>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public uint[] nReserved;
        }

        // Token: 0x02000066 RID: 102
        public struct MV_XML_FEATURE_Port
        {
            /// <summary/>
            public MV_XML_Visibility enVisivility;

            /// <summary/>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 512)]
            public string strDescription;

            /// <summary/>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string strDisplayName;

            /// <summary/>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string strName;

            /// <summary/>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 512)]
            public string strToolTip;

            /// <summary/>
            public MV_XML_AccessMode enAccessMode;

            /// <summary/>
            public int bIsLocked;

            /// <summary/>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public uint[] nReserved;
        }

        // Token: 0x02000067 RID: 103
        public enum MvGvspPixelType
        {
            /// <summary/>
            PixelType_Gvsp_Undefined = -1,
            /// <summary/>
            PixelType_Gvsp_Mono1p = 16842807,
            /// <summary/>
            PixelType_Gvsp_Mono2p = 16908344,
            /// <summary/>
            PixelType_Gvsp_Mono4p = 17039417,
            /// <summary/>
            PixelType_Gvsp_Mono8 = 17301505,
            /// <summary/>
            PixelType_Gvsp_Mono8_Signed,
            /// <summary/>
            PixelType_Gvsp_Mono10 = 17825795,
            /// <summary/>
            PixelType_Gvsp_Mono10_Packed = 17563652,
            /// <summary/>
            PixelType_Gvsp_Mono12 = 17825797,
            /// <summary/>
            PixelType_Gvsp_Mono12_Packed = 17563654,
            /// <summary/>
            PixelType_Gvsp_Mono14 = 17825829,
            /// <summary/>
            PixelType_Gvsp_Mono16 = 17825799,
            /// <summary/>
            PixelType_Gvsp_BayerGR8 = 17301512,
            /// <summary/>
            PixelType_Gvsp_BayerRG8,
            /// <summary/>
            PixelType_Gvsp_BayerGB8,
            /// <summary/>
            PixelType_Gvsp_BayerBG8,
            /// <summary/>
            PixelType_Gvsp_BayerGR10 = 17825804,
            /// <summary/>
            PixelType_Gvsp_BayerRG10,
            /// <summary/>
            PixelType_Gvsp_BayerGB10,
            /// <summary/>
            PixelType_Gvsp_BayerBG10,
            /// <summary/>
            PixelType_Gvsp_BayerGR12,
            /// <summary/>
            PixelType_Gvsp_BayerRG12,
            /// <summary/>
            PixelType_Gvsp_BayerGB12,
            /// <summary/>
            PixelType_Gvsp_BayerBG12,
            /// <summary/>
            PixelType_Gvsp_BayerGR10_Packed = 17563686,
            /// <summary/>
            PixelType_Gvsp_BayerRG10_Packed,
            /// <summary/>
            PixelType_Gvsp_BayerGB10_Packed,
            /// <summary/>
            PixelType_Gvsp_BayerBG10_Packed,
            /// <summary/>
            PixelType_Gvsp_BayerGR12_Packed,
            /// <summary/>
            PixelType_Gvsp_BayerRG12_Packed,
            /// <summary/>
            PixelType_Gvsp_BayerGB12_Packed,
            /// <summary/>
            PixelType_Gvsp_BayerBG12_Packed,
            /// <summary/>
            PixelType_Gvsp_BayerGR16 = 17825838,
            /// <summary/>
            PixelType_Gvsp_BayerRG16,
            /// <summary/>
            PixelType_Gvsp_BayerGB16,
            /// <summary/>
            PixelType_Gvsp_BayerBG16,
            /// <summary/>
            PixelType_Gvsp_RGB8_Packed = 35127316,
            /// <summary/>
            PixelType_Gvsp_BGR8_Packed,
            /// <summary/>
            PixelType_Gvsp_RGBA8_Packed = 35651606,
            /// <summary/>
            PixelType_Gvsp_BGRA8_Packed,
            /// <summary/>
            PixelType_Gvsp_RGB10_Packed = 36700184,
            /// <summary/>
            PixelType_Gvsp_BGR10_Packed,
            /// <summary/>
            PixelType_Gvsp_RGB12_Packed,
            /// <summary/>
            PixelType_Gvsp_BGR12_Packed,
            /// <summary/>
            PixelType_Gvsp_RGB16_Packed = 36700211,
            /// <summary/>
            PixelType_Gvsp_RGB10V1_Packed = 35651612,
            /// <summary/>
            PixelType_Gvsp_RGB10V2_Packed,
            /// <summary/>
            PixelType_Gvsp_RGB12V1_Packed = 35913780,
            /// <summary/>
            PixelType_Gvsp_RGB565_Packed = 34603061,
            /// <summary/>
            PixelType_Gvsp_BGR565_Packed,
            /// <summary/>
            PixelType_Gvsp_YUV411_Packed = 34340894,
            /// <summary/>
            PixelType_Gvsp_YUV422_Packed = 34603039,
            /// <summary/>
            PixelType_Gvsp_YUV422_YUYV_Packed = 34603058,
            /// <summary/>
            PixelType_Gvsp_YUV444_Packed = 35127328,
            /// <summary/>
            PixelType_Gvsp_YCBCR8_CBYCR = 35127354,
            /// <summary/>
            PixelType_Gvsp_YCBCR422_8 = 34603067,
            /// <summary/>
            PixelType_Gvsp_YCBCR422_8_CBYCRY = 34603075,
            /// <summary/>
            PixelType_Gvsp_YCBCR411_8_CBYYCRYY = 34340924,
            /// <summary/>
            PixelType_Gvsp_YCBCR601_8_CBYCR = 35127357,
            /// <summary/>
            PixelType_Gvsp_YCBCR601_422_8 = 34603070,
            /// <summary/>
            PixelType_Gvsp_YCBCR601_422_8_CBYCRY = 34603076,
            /// <summary/>
            PixelType_Gvsp_YCBCR601_411_8_CBYYCRYY = 34340927,
            /// <summary/>
            PixelType_Gvsp_YCBCR709_8_CBYCR = 35127360,
            /// <summary/>
            PixelType_Gvsp_YCBCR709_422_8 = 34603073,
            /// <summary/>
            PixelType_Gvsp_YCBCR709_422_8_CBYCRY = 34603077,
            /// <summary/>
            PixelType_Gvsp_YCBCR709_411_8_CBYYCRYY = 34340930,
            /// <summary/>
            PixelType_Gvsp_RGB8_Planar = 35127329,
            /// <summary/>
            PixelType_Gvsp_RGB10_Planar = 36700194,
            /// <summary/>
            PixelType_Gvsp_RGB12_Planar,
            /// <summary/>
            PixelType_Gvsp_RGB16_Planar,
            /// <summary/>
            PixelType_Gvsp_Jpeg = -2145910783,
            /// <summary/>
            PixelType_Gvsp_Coord3D_ABC32f = 39846080,
            /// <summary/>
            PixelType_Gvsp_Coord3D_ABC32f_Planar,
            /// <summary/>
            PixelType_Gvsp_Coord3D_AC32f = 37748930,
            /// <summary/>
            PixelType_Gvsp_COORD3D_DEPTH_PLUS_MASK = -2112094207,
            /// <summary/>
            PixelType_Gvsp_Coord3D_ABC32 = -2107625471,
            /// <summary/>
            PixelType_Gvsp_Coord3D_AB32f = -2109722622,
            /// <summary/>
            PixelType_Gvsp_Coord3D_AB32,
            /// <summary/>
            PixelType_Gvsp_Coord3D_AC32f_Planar = 37748931,
            /// <summary/>
            PixelType_Gvsp_Coord3D_AC32 = -2109722620,
            /// <summary/>
            PixelType_Gvsp_Coord3D_A32f = 18874557,
            /// <summary/>
            PixelType_Gvsp_Coord3D_A32 = -2128596987,
            /// <summary/>
            PixelType_Gvsp_Coord3D_C32f = 18874559,
            /// <summary/>
            PixelType_Gvsp_Coord3D_C32 = -2128596986,
            /// <summary/>
            PixelType_Gvsp_Coord3D_ABC16 = 36700345,
            /// <summary/>
            PixelType_Gvsp_Coord3D_C16 = 17825976,
            /// <summary/>
            PixelType_Gvsp_HB_Mono8 = -2130182143,
            /// <summary/>
            PixelType_Gvsp_HB_Mono10 = -2129657853,
            /// <summary/>
            PixelType_Gvsp_HB_Mono10_Packed = -2129919996,
            /// <summary/>
            PixelType_Gvsp_HB_Mono12 = -2129657851,
            /// <summary/>
            PixelType_Gvsp_HB_Mono12_Packed = -2129919994,
            /// <summary/>
            PixelType_Gvsp_HB_Mono16 = -2129657849,
            /// <summary/>
            PixelType_Gvsp_HB_BayerGR8 = -2130182136,
            /// <summary/>
            PixelType_Gvsp_HB_BayerRG8,
            /// <summary/>
            PixelType_Gvsp_HB_BayerGB8,
            /// <summary/>
            PixelType_Gvsp_HB_BayerBG8,
            /// <summary/>
            PixelType_Gvsp_HB_BayerGR10 = -2129657844,
            /// <summary/>
            PixelType_Gvsp_HB_BayerRG10,
            /// <summary/>
            PixelType_Gvsp_HB_BayerGB10,
            /// <summary/>
            PixelType_Gvsp_HB_BayerBG10,
            /// <summary/>
            PixelType_Gvsp_HB_BayerGR12,
            /// <summary/>
            PixelType_Gvsp_HB_BayerRG12,
            /// <summary/>
            PixelType_Gvsp_HB_BayerGB12,
            /// <summary/>
            PixelType_Gvsp_HB_BayerBG12,
            /// <summary/>
            PixelType_Gvsp_HB_BayerGR10_Packed = -2129919962,
            /// <summary/>
            PixelType_Gvsp_HB_BayerRG10_Packed,
            /// <summary/>
            PixelType_Gvsp_HB_BayerGB10_Packed,
            /// <summary/>
            PixelType_Gvsp_HB_BayerBG10_Packed,
            /// <summary/>
            PixelType_Gvsp_HB_BayerGR12_Packed,
            /// <summary/>
            PixelType_Gvsp_HB_BayerRG12_Packed,
            /// <summary/>
            PixelType_Gvsp_HB_BayerGB12_Packed,
            /// <summary/>
            PixelType_Gvsp_HB_BayerBG12_Packed,
            /// <summary/>
            PixelType_Gvsp_HB_YUV422_Packed = -2112880609,
            /// <summary/>
            PixelType_Gvsp_HB_YUV422_YUYV_Packed = -2112880590,
            /// <summary/>
            PixelType_Gvsp_HB_RGB8_Packed = -2112356332,
            /// <summary/>
            PixelType_Gvsp_HB_BGR8_Packed,
            /// <summary/>
            PixelType_Gvsp_HB_RGBA8_Packed = -2111832042,
            /// <summary/>
            PixelType_Gvsp_HB_BGRA8_Packed
        }
    }
}
