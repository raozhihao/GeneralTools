using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GeneralTool.CoreLibrary.Attributes;
using GeneralTool.CoreLibrary.TaskLib;

namespace TaskServerUI.Commons
{
    [Route(nameof(Startup)+"/")]
    public class Startup : BaseTaskInvoke, IDisposable
    {
       
        private bool disposedValue;


        public Startup()
        {
          
        }

      

        [Route(nameof(Test), "Test")]
        public string Test()
        {
            return "This is a test method";
        }

        [Route(nameof(Open), "开启相机")]
        public void Open([WaterMark("相机下标,如果小于0,则使用ip查找")] int index = 0, [WaterMark("相机ip")] string ip = null)
        {
           
        }

        /// <summary>
        /// 关闭设备关闭流
        /// </summary>
        [Route(nameof(CloseDevice), "关闭设备")]
        public void CloseDevice()
        {
           
        }


        /// <summary>
        /// 开始拍摄
        /// </summary>
        /// <param name="frameRate">设置的帧率</param>
        /// <param name="grabSeconds">拍摄的时间秒</param>
        [Route(nameof(StartCapture), "开始拍摄")]
        public void StartCapture(int frameRate, int grabSeconds)
        {
            
        }

        /// <summary>
        /// 停止采集
        /// </summary>
        [Route(nameof(StopDevice), "停止采集数据流")]
        public void StopDevice()
        {
           
        }

        [Route(nameof(SetTriggerMode), "设置触发模式")]
        public void SetTriggerMode([WaterMark("True为软触发")] bool mode = true)
        {
          
        }

        [Route(nameof(SaveSingleImage), "拍摄单张图片并保存")]
        public string SaveSingleImage([WaterMark("是否恢复最大画幅")] bool maxRoi = true)
        {
            return "";
        }

        [Route(nameof(SetRoi), "设置画幅")]
        public void SetRoi([WaterMark("X偏移")] int x, [WaterMark("Y偏移")] int y, [WaterMark("宽度")] int width, [WaterMark("高度")] int height)
        {
           
        }

        [Route(nameof(SetMaxROI), "设置最大ROI")]
        public void SetMaxROI()
        {
            
        }

        /// <summary>
        /// 当还在拍摄时进行异常招聘
        /// </summary>
        private void ThrowIfCpaturing()
        {
           
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (disposing)
                {
                    // TODO: 释放托管状态(托管对象)
                   
                }

                // TODO: 释放未托管的资源(未托管的对象)并重写终结器
                // TODO: 将大型字段设置为 null
                this.disposedValue = true;
            }
        }

        // TODO: 仅当“Dispose(bool disposing)”拥有用于释放未托管资源的代码时才替代终结器
        ~Startup()
        {
            // 不要更改此代码。请将清理代码放入“Dispose(bool disposing)”方法中
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            // 不要更改此代码。请将清理代码放入“Dispose(bool disposing)”方法中
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
