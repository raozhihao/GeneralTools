using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Threading.Tasks;

using GeneralTool.CoreLibrary.Interfaces;
using GeneralTool.CoreLibrary.Models;
using GeneralTool.CoreLibrary.WPFHelper;
using GeneralTool.CoreLibrary.WPFHelper.DiagramDesigner.Models;

namespace SimpleDiagram.BlockVIewModels
{
    /// <summary>
    /// 基数据类
    /// </summary>
    public abstract class BaseBlockViewModel : BaseNotifyModel
    {
        /// <summary>
        /// 当前脚本/函数的ID
        /// </summary>
        public string ScriptId { get; set; }

        /// <summary>
        /// 当前块的ID
        /// </summary>
        public string BlockId { get; set; }
        public ILog Log { get; set; }
        public BaseBlockViewModel NextModel { get; set; }


        public virtual async Task<bool> Execute(BaseBlockViewModel prevModel, ExcuteCancelTokenSource token)
        {
            return await this.ExecuteImp(prevModel, token);
        }

        public abstract Task<bool> ExecuteImp(BaseBlockViewModel prevModel, ExcuteCancelTokenSource token);
        public List<BaseBlockViewModel> SinkBlockModels { get; private set; } = new List<BaseBlockViewModel>();
        public List<BaseBlockViewModel> SourceBlockModels { get; private set; } = new List<BaseBlockViewModel>();

        public void SetSinkModels(List<BaseBlockViewModel> sinkModels)
        {
            this.SinkBlockModels = sinkModels;
        }

        public event Action<bool> IsSelected;
        public virtual void Selected()
        {
            IsSelected?.Invoke(true);
        }


        public virtual void UnSelected()
        {
            IsSelected?.Invoke(false);
        }

        public event Action<bool> SetBreakBlockEvent;
        /// <summary>
        /// 设置为选择块
        /// </summary>
        public void SetBreakBlock()
        {
            SetBreakBlockEvent?.Invoke(true);
        }

        public void UnBreakBlock()
        {
            SetBreakBlockEvent?.Invoke(false);
        }

        public event Func<bool> IsBreakPointEvent;
        /// <summary>
        /// 是否属于断点
        /// </summary>
        public bool IsBreakPoint
        {
            get
            {
                if (this.IsBreakPointEvent != null)
                {
                    return this.IsBreakPointEvent();
                }
                return false;
            }
        }
        public string Description
        {
            get
            {
                var type = this.GetType();
                var desc = type.GetCustomAttribute<DescriptionAttribute>();
                if (desc == null)
                    return type.Name;
                else
                    return desc.Description;
            }
        }



        public virtual void SetProperty(ConnectionDo connection, BaseBlockViewModel newxtModel)
        {
        }

        public virtual void SelectData(string id)
        {

        }
    }
}
