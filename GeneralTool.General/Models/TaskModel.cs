using GeneralTool.General.WPFHelper;
using System.Collections.ObjectModel;

namespace GeneralTool.General.Models
{
    /// <summary>
    /// 任务
    /// </summary>
    public class DoTaskModel : BaseNotifyModel
    {
        #region Private 字段

        private DoTaskParameterItem parameterItem;
        private string url;
        private string returnString;

        #endregion Private 字段

        #region Public 属性

        /// <summary>
        /// 任务
        /// </summary>
        public DoTaskParameterItem DoTaskParameterItem { get => this.parameterItem; set => this.RegisterProperty(ref this.parameterItem, value); }

        /// <summary>
        /// Url
        /// </summary>
        public string Url { get => this.url; set => this.RegisterProperty(ref this.url, value); }


        #endregion Public 属性
    }

    /// <summary>
    /// 任务类型
    /// </summary>
    public class TaskModel : BaseNotifyModel
    {
        #region Private 字段

        private ObservableCollection<DoTaskModel> doTaskModels = new ObservableCollection<DoTaskModel>();
        private string explanation;
        private bool hasItems = false;

        private int selectedIndex = 0;

        private DoTaskModel selectedItem;

        #endregion Private 字段

        #region Public 属性

        /// <summary>
        /// 任务类集合
        /// </summary>
        public ObservableCollection<DoTaskModel> DoTaskModels { get => this.doTaskModels; set => this.RegisterProperty(ref this.doTaskModels, value); }

        /// <summary>
        /// 任务类注解
        /// </summary>
        public string Explanation { get => this.explanation; set => this.RegisterProperty(ref this.explanation, value); }

        /// <summary>
        /// 集合中是否有元素
        /// </summary>
        public bool HasItems
        {
            get
            {
                this.hasItems = this.DoTaskModels.Count > 0;
                return this.hasItems;
            }

            set => this.RegisterProperty(ref this.hasItems, value);
        }

        /// <summary>
        /// 当前选择项
        /// </summary>
        public int SelectedIndex
        {
            get => this.selectedIndex;
            set
            {
                this.RegisterProperty(ref this.selectedIndex, value);
            }
        }

        /// <summary>
        /// 当前选择对象
        /// </summary>
        public DoTaskModel SelectedItem
        {
            get
            {
                if (this.HasItems && this.SelectedIndex > -1 && this.selectedItem == null)
                    this.selectedItem = this.DoTaskModels[this.SelectedIndex];
                return this.selectedItem;
            }
            set
            {
                this.RegisterProperty(ref selectedItem, value);
            }
        }

        #endregion Public 属性
    }
}