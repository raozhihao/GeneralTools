using System;
using System.Windows.Input;

using GeneralTool.General.Interfaces;

using GeneralTool.General.WPFHelper;

namespace SimpleDiagram.BlockVIewModels
{
    public abstract class BaseBlockWindowViewModel : BaseNotifyModel, IDisposable
    {
        private bool? dialogResult;
        public bool? DialogResult
        {
            get => this.dialogResult;
            set => this.RegisterProperty(ref this.dialogResult, value);
        }

        public ILog Log { get; set; }
        public ICommand OKCommand => new SimpleCommand(OKMethod);

        public virtual void OKMethod()
        {
            this.DialogResult = true;
        }

        public ICommand CancelCommand => new SimpleCommand(CancelMethod);

        public virtual void CancelMethod()
        {
            this.DialogResult = false;
        }

        public virtual void Dispose()
        {
            //this.ClearParameters();
        }


        public BaseBlockViewModel BaseViewModel { get; set; }
        #region 数据操作区

        ///// <summary>
        ///// 从数据库中将数据进行填充
        ///// </summary>
        ///// <param name="scriptId"></param>
        ///// <param name="blockId"></param>
        ///// <returns></returns>
        //public abstract bool SelectByDb(SqlSugarScope db, string scriptId, string blockId);

        #endregion
    }
}
