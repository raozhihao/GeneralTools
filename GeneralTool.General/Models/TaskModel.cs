using GeneralTool.General.WPFHelper;
using System.Collections.ObjectModel;

namespace GeneralTool.General.Models
{
    /// <summary>
    /// 任务类型
    /// </summary>
    public class TaskModel : BaseNotifyModel
    {
        private string explanation;
        /// <summary>
        /// 任务类注解
        /// </summary>
        public string Explanation { get => this.explanation; set => this.RegisterProperty(ref this.explanation, value); }

        private ObservableCollection<DoTaskModel> doTaskModels = new ObservableCollection<DoTaskModel>();
        /// <summary>
        /// 任务类集合
        /// </summary>
        public ObservableCollection<DoTaskModel> DoTaskModels { get => this.doTaskModels; set => this.RegisterProperty(ref this.doTaskModels, value); }

        private int selectedIndex = 0;
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

        private DoTaskModel selectedItem;
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

        private bool hasItems = false;
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

    }

    /// <summary>
    /// 任务
    /// </summary>
    public class DoTaskModel : BaseNotifyModel
    {
        private string url;
        /// <summary>
        /// Url
        /// </summary>
        public string Url { get => this.url; set => this.RegisterProperty(ref this.url, value); }
        private DoTaskParameterItem parameterItem;
        /// <summary>
        /// 任务
        /// </summary>
        public DoTaskParameterItem DoTaskParameterItem { get => this.parameterItem; set => this.RegisterProperty(ref this.parameterItem, value); }
    }
}
