using System.ComponentModel;

namespace GeneralTool.CoreLibrary.MVS
{
    /// <summary>
    /// 
    /// </summary>
    public class Errors
    {


        /// <summary>错误或无效句柄</summary>
        [Description("错误或无效句柄")]
        public const int MV_E_HANDLE = -2147483648;

        /// <summary>Not supported function</summary>
        [Description("不支持的方法")]
        public const int MV_E_SUPPORT = -2147483647;

        /// <summary>Buffer overflow</summary>
        [Description("内存溢出")]
        public const int MV_E_BUFOVER = -2147483646;

        /// <summary>函数调用顺序错误</summary>
        [Description("函数调用顺序错误")]
        public const int MV_E_CALLORDER = -2147483645;

        /// <summary>参数不正确</summary>
        [Description("参数不正确")]
        public const int MV_E_PARAMETER = -2147483644;

        /// <summary>应用资源失败</summary>
        [Description("应用资源失败")]
        public const int MV_E_RESOURCE = -2147483642;

        /// <summary>无数据</summary>
        [Description("无数据")]
        public const int MV_E_NODATA = -2147483641;

        /// <summary>前提条件错误，或运行环境已更改</summary>
        [Description("前提条件错误，或运行环境已更改")]
        public const int MV_E_PRECONDITION = -2147483640;

        /// <summary>版本不匹配</summary>
        [Description("版本不匹配")]
        public const int MV_E_VERSION = -2147483639;

        /// <summary>内存不足</summary>
        [Description("内存不足")]
        public const int MV_E_NOENOUGH_BUF = -2147483638;

        /// <summary>图像不正常，可能是由于数据包丢失导致图像不完整</summary>
        [Description("图像不正常，可能是由于数据包丢失导致图像不完整")]
        public const int MV_E_ABNORMAL_IMAGE = -2147483637;

        /// <summary>加载库失败</summary>
        [Description("加载库失败")]
        public const int MV_E_LOAD_LIBRARY = -2147483636;

        /// <summary>无可用缓冲区</summary>
        [Description("无可用缓冲区")] public const int MV_E_NOOUTBUF = -2147483635;

        /// <summary>加密错误</summary>
        [Description("加密错误")] public const int MV_E_ENCRYPT = -2147483634;

        /// <summary>未知错误</summary>
        [Description("未知错误")] public const int MV_E_UNKNOW = -2147483393;

        /// <summary>一般错误</summary>
        [Description("一般错误")] public const int MV_E_GC_GENERIC = -2147483392;

        /// <summary>非法参数</summary>
        [Description("非法参数")] public const int MV_E_GC_ARGUMENT = -2147483391;

        /// <summary>该值超出范围</summary>
        [Description("该值超出范围")] public const int MV_E_GC_RANGE = -2147483390;

        /// <summary>所有物</summary>
        [Description("属性错误")] public const int MV_E_GC_PROPERTY = -2147483389;

        /// <summary>Running environment error</summary>
        [Description("运行环境错误")] public const int MV_E_GC_RUNTIME = -2147483388;

        /// <summary>逻辑错误</summary>
        [Description("逻辑错误")] public const int MV_E_GC_LOGICAL = -2147483387;

        /// <summary>节点访问条件错误</summary>
        [Description("节点访问条件错误")] public const int MV_E_GC_ACCESS = -2147483386;

        /// <summary>Timeout</summary>
        [Description("超时")] public const int MV_E_GC_TIMEOUT = -2147483385;

        /// <summary>转换异常</summary>
        [Description("转换异常")] public const int MV_E_GC_DYNAMICCAST = -2147483384;

        /// <summary>Gen ICam未知错误</summary>
        [Description("Gen ICam未知错误")] public const int MV_E_GC_UNKNOW = -2147483137;

        /// <summary>设备不支持该命令</summary>
        [Description("设备不支持该命令")] public const int MV_E_NOT_IMPLEMENTED = -2147483136;

        /// <summary>正在访问的目标地址不存在</summary>
        [Description("正在访问的目标地址不存在")] public const int MV_E_INVALID_ADDRESS = -2147483135;

        /// <summary>目标地址不可写</summary>
        [Description("目标地址不可写")] public const int MV_E_WRITE_PROTECT = -2147483134;

        /// <summary>No permission</summary>
        [Description("没有访问权限")] public const int MV_E_ACCESS_DENIED = -2147483133;

        /// <summary>设备正忙，或网络已断开</summary>
        [Description("设备正忙，或网络已断开")] public const int MV_E_BUSY = -2147483132;

        /// <summary>网络数据包错误</summary>
        [Description("网络数据包错误")] public const int MV_E_PACKET = -2147483131;

        /// <summary>Network error</summary>
        [Description("网络错误")] public const int MV_E_NETER = -2147483130;

        /// <summary>设备IP冲突</summary>
        [Description("设备IP冲突")] public const int MV_E_IP_CONFLICT = -2147483103;

        /// <summary>读取USB错误</summary>
        [Description("读取USB错误")] public const int MV_E_USB_READ = -2147482880;

        /// <summary>Writing USB error</summary>
        [Description("写入USB错误")] public const int MV_E_USB_WRITE = -2147482879;

        /// <summary>设备异常</summary>
        [Description("设备异常")] public const int MV_E_USB_DEVICE = -2147482878;

        /// <summary>Gen ICam错误</summary>
        [Description("Gen ICam错误")] public const int MV_E_USB_GENICAM = -2147482877;

        /// <summary>带宽不足，此错误代码是新添加的</summary>
        [Description("带宽不足")] public const int MV_E_USB_BANDWIDTH = -2147482876;

        /// <summary>驱动程序不匹配或未安装驱动器</summary>
        [Description("驱动程序不匹配或未安装驱动器")] public const int MV_E_USB_DRIVER = -2147482875;

        /// <summary>USB未知错误</summary>
        [Description("USB未知错误")] public const int MV_E_USB_UNKNOW = -2147482625;

        /// <summary>固件不匹配</summary>
        [Description("固件不匹配")] public const int MV_E_UPG_FILE_MISMATCH = -2147482624;

        /// <summary>固件语言不匹配</summary>
        [Description("固件语言不匹配")] public const int MV_E_UPG_LANGUSGE_MISMATCH = -2147482623;

        /// <summary>升级冲突（设备升级过程中重复升级请求）</summary>
        [Description("升级冲突（设备升级过程中重复升级请求）")] public const int MV_E_UPG_CONFLICT = -2147482622;

        /// <summary>升级过程中出现摄像头内部错误</summary>
        [Description("升级过程中出现摄像头内部错误")] public const int MV_E_UPG_INNER_ERR = -2147482621;

        /// <summary>升级过程中出现未知错误</summary>
        [Description("升级过程中出现未知错误")] public const int MV_E_UPG_UNKNOW = -2147482369;



        /// <summary>不确定类型错误</summary>
        [Description("不确定类型错误")] public const int MV_ALG_ERR = 268435456;

        /// <summary>能力集中存在无效参数</summary>
        [Description("能力集中存在无效参数")] public const int MV_ALG_E_ABILITY_ARG = 268435457;

        /// <summary>内存地址为空</summary>
        [Description("内存地址为空")] public const int MV_ALG_E_MEM_NULL = 268435458;

        /// <summary>内存对齐不满足要求</summary>
        [Description("内存对齐不满足要求")] public const int MV_ALG_E_MEM_ALIGN = 268435459;

        /// <summary>内存空间大小不够</summary>
        [Description("内存空间大小不够")] public const int MV_ALG_E_MEM_LACK = 268435460;

        /// <summary>内存空间大小不满足对齐要求</summary>
        [Description("内存空间大小不满足对齐要求")] public const int MV_ALG_E_MEM_SIZE_ALIGN = 268435461;

        /// <summary>内存地址不满足对齐要求</summary>
        [Description("内存地址不满足对齐要求")] public const int MV_ALG_E_MEM_ADDR_ALIGN = 268435462;

        /// <summary>图像格式不正确或者不支持</summary>
        [Description("图像格式不正确或者不支持")] public const int MV_ALG_E_IMG_FORMAT = 268435463;

        /// <summary>图像宽高不正确或者超出范围</summary>
        [Description("图像宽高不正确或者超出范围")] public const int MV_ALG_E_IMG_SIZE = 268435464;

        /// <summary>图像宽高与step参数不匹配</summary>
        [Description("图像宽高与step参数不匹配")] public const int MV_ALG_E_IMG_STEP = 268435465;

        /// <summary>图像数据存储地址为空</summary>
        [Description("图像数据存储地址为空")] public const int MV_ALG_E_IMG_DATA_NULL = 268435466;

        /// <summary>设置或者获取参数类型不正确</summary>
        [Description("设置或者获取参数类型不正确")] public const int MV_ALG_E_CFG_TYPE = 268435467;

        /// <summary>设置或者获取参数的输入、输出结构体大小不正确</summary>
        [Description("设置或者获取参数的输入、输出结构体大小不正确")] public const int MV_ALG_E_CFG_SIZE = 268435468;

        /// <summary>处理类型不正确</summary>
        [Description("处理类型不正确")] public const int MV_ALG_E_PRC_TYPE = 268435469;

        /// <summary>处理时输入、输出参数大小不正确</summary>
        [Description("处理时输入、输出参数大小不正确")] public const int MV_ALG_E_PRC_SIZE = 268435470;

        /// <summary>子处理类型不正确</summary>
        [Description("子处理类型不正确")] public const int MV_ALG_E_FUNC_TYPE = 268435471;

        /// <summary>子处理时输入、输出参数大小不正确</summary>
        [Description("子处理时输入、输出参数大小不正确")] public const int MV_ALG_E_FUNC_SIZE = 268435472;

        /// <summary>index参数不正确</summary>
        [Description("index参数不正确")] public const int MV_ALG_E_PARAM_INDEX = 268435473;

        /// <summary>value参数不正确或者超出范围</summary>
        [Description("value参数不正确或者超出范围")] public const int MV_ALG_E_PARAM_VALUE = 268435474;

        /// <summary>param_num参数不正确</summary>
        [Description("param_num参数不正确")] public const int MV_ALG_E_PARAM_NUM = 268435475;

        /// <summary>函数参数指针为空</summary>
        [Description("函数参数指针为空")] public const int MV_ALG_E_NULL_PTR = 268435476;

        /// <summary>超过限定的最大内存</summary>
        [Description("超过限定的最大内存")] public const int MV_ALG_E_OVER_MAX_MEM = 268435477;

        /// <summary>回调函数出错</summary>
        [Description("回调函数出错")] public const int MV_ALG_E_CALL_BACK = 268435478;

        /// <summary>加密错误</summary>
        [Description("加密错误")] public const int MV_ALG_E_ENCRYPT = 268435479;

        /// <summary>算法库使用期限错误</summary>
        [Description("算法库使用期限错误")] public const int MV_ALG_E_EXPIRE = 268435480;

        /// <summary>参数范围不正确</summary>
        [Description("参数范围不正确")] public const int MV_ALG_E_BAD_ARG = 268435481;

        /// <summary>数据大小不正确</summary>
        [Description("数据大小不正确")] public const int MV_ALG_E_DATA_SIZE = 268435482;

        /// <summary>数据step不正确</summary>
        [Description("数据step不正确")] public const int MV_ALG_E_STEP = 268435483;

        /// <summary>cpu不支持优化代码中的指令集</summary>
        [Description("cpu不支持优化代码中的指令集")] public const int MV_ALG_E_CPUID = 268435484;

        /// <summary>警告</summary>
        [Description("警告")] public const int MV_ALG_WARNING = 268435485;

        /// <summary>算法库超时</summary>
        [Description("算法库超时")] public const int MV_ALG_E_TIME_OUT = 268435486;

        /// <summary>算法版本号出错</summary>
        [Description("算法版本号出错")] public const int MV_ALG_E_LIB_VERSION = 268435487;

        /// <summary>模型版本号出错</summary>
        [Description("模型版本号出错")] public const int MV_ALG_E_MODEL_VERSION = 268435488;

        /// <summary>GPU内存分配错误</summary>
        [Description("GPU内存分配错误")] public const int MV_ALG_E_GPU_MEM_ALLOC = 268435489;

        /// <summary>文件不存在</summary>
        [Description("文件不存在")] public const int MV_ALG_E_FILE_NON_EXIST = 268435490;

        /// <summary>字符串为空</summary>
        [Description("字符串为空")] public const int MV_ALG_E_NONE_STRING = 268435491;

        /// <summary>图像解码器错误</summary>
        [Description("图像解码器错误")] public const int MV_ALG_E_IMAGE_CODEC = 268435492;

        /// <summary>打开文件错误</summary>
        [Description("打开文件错误")] public const int MV_ALG_E_FILE_OPEN = 268435493;

        /// <summary>文件读取错误</summary>
        [Description("文件读取错误")] public const int MV_ALG_E_FILE_READ = 268435494;

        /// <summary>文件写错误</summary>
        [Description("文件写错误")] public const int MV_ALG_E_FILE_WRITE = 268435495;

        /// <summary>文件读取大小错误</summary>
        [Description("文件读取大小错误")] public const int MV_ALG_E_FILE_READ_SIZE = 268435496;

        /// <summary>文件类型错误</summary>
        [Description("文件类型错误")] public const int MV_ALG_E_FILE_TYPE = 268435497;

        /// <summary>模型类型错误</summary>
        [Description("模型类型错误")] public const int MV_ALG_E_MODEL_TYPE = 268435498;

        /// <summary>分配内存错误</summary>
        [Description("分配内存错误")] public const int MV_ALG_E_MALLOC_MEM = 268435499;

        /// <summary>线程绑核失败</summary>
        [Description("线程绑核失败")] public const int MV_ALG_E_BIND_CORE_FAILED = 268435500;

        /// <summary>噪声特性图像格式错误</summary>
        [Description("噪声特性图像格式错误")] public const int MV_ALG_E_DENOISE_NE_IMG_FORMAT = 272637953;

        /// <summary>噪声特性类型错误</summary>
        [Description("噪声特性类型错误")] public const int MV_ALG_E_DENOISE_NE_FEATURE_TYPE = 272637954;

        /// <summary>噪声特性个数错误</summary>
        [Description("噪声特性个数错误")] public const int MV_ALG_E_DENOISE_NE_PROFILE_NUM = 272637955;

        /// <summary>噪声特性增益个数错误</summary>
        [Description("噪声特性增益个数错误")] public const int MV_ALG_E_DENOISE_NE_GAIN_NUM = 272637956;

        /// <summary>噪声曲线增益值输入错误</summary>
        [Description("噪声曲线增益值输入错误")] public const int MV_ALG_E_DENOISE_NE_GAIN_VAL = 272637957;

        /// <summary>噪声曲线柱数错误</summary>
        [Description("噪声曲线柱数错误")] public const int MV_ALG_E_DENOISE_NE_BIN_NUM = 272637958;

        /// <summary>噪声估计初始化增益设置错误</summary>
        [Description("噪声估计初始化增益设置错误")] public const int MV_ALG_E_DENOISE_NE_INIT_GAIN = 272637959;

        /// <summary>噪声估计未初始化</summary>
        [Description("噪声估计未初始化")] public const int MV_ALG_E_DENOISE_NE_NOT_INIT = 272637960;

        /// <summary>颜色空间模式错误</summary>
        [Description("颜色空间模式错误")] public const int MV_ALG_E_DENOISE_COLOR_MODE = 272637961;

        /// <summary>图像ROI个数错误</summary>
        [Description("图像ROI个数错误")] public const int MV_ALG_E_DENOISE_ROI_NUM = 272637962;

        /// <summary>图像ROI原点错误</summary>
        [Description("图像ROI原点错误")] public const int MV_ALG_E_DENOISE_ROI_ORI_PT = 272637963;

        /// <summary>图像ROI大小错误</summary>
        [Description("图像ROI大小错误")] public const int MV_ALG_E_DENOISE_ROI_SIZE = 272637964;

        /// <summary>输入的相机增益不存在(增益个数已达上限)</summary>
        [Description("输入的相机增益不存在(增益个数已达上限)")] public const int MV_ALG_E_DENOISE_GAIN_NOT_EXIST = 272637965;

        /// <summary>输入的相机增益不在范围内</summary>
        [Description("输入的相机增益不在范围内")] public const int MV_ALG_E_DENOISE_GAIN_BEYOND_RANGE = 272637966;

        /// <summary>输入的噪声特性内存大小错误</summary>
        [Description("输入的噪声特性内存大小错误")] public const int MV_ALG_E_DENOISE_NP_BUF_SIZE = 272637967;
    }
}
