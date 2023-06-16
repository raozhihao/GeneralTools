using System.Collections.ObjectModel;

using GeneralTool.CoreLibrary.WPFHelper;

namespace GeneralTool.CoreLibrary.Models
{
    /// <summary>
    /// 任务
    /// </summary>
    public class DoTaskModel : BaseNotifyModel
    {
        #region Private 字段

        private DoTaskParameterItem parameterItem;
        private string url;

        #endregion Private 字段

        #region Public 属性

        /// <summary>
        /// 任务
        /// </summary>
        public DoTaskParameterItem DoTaskParameterItem
        {
            get => parameterItem;
            set => RegisterProperty(ref parameterItem, value);
        }

        /// <summary>
        /// Url
        /// </summary>
        public string Url { get => url; set => RegisterProperty(ref url, value); }

        #endregion Public 属性
    }

    /// <summary>
    /// 任务类型
    /// </summary>
    public class TaskModel : BaseNotifyModel
    {
        /// <summary>
        /// 
        /// </summary>
        public TaskModel()
        {
        }

        /// <summary>
        /// 初始化语言库
        /// </summary>
        public void InitLangKey()
        {
            if (string.IsNullOrWhiteSpace(LangKey))
                return;
            //设置默认库
            if (!LangProvider.LangProviderInstance.DefaultResource.ContainsKey(LangKey))
            {
                LangProvider.LangProviderInstance.DefaultResource.Add(LangKey, defaultText);
            }

            LangProviderInstance_LangChanged(LangProvider.LangProviderInstance.CurrentResource);
            LangProvider.LangProviderInstance.LangChanged += LangProviderInstance_LangChanged;
        }
        private void LangProviderInstance_LangChanged(System.Windows.ResourceDictionary obj)
        {
            if (string.IsNullOrWhiteSpace(LangKey))
                return;

            string value = LangProvider.LangProviderInstance.GetLangValue(LangKey);

            Explanation = string.IsNullOrWhiteSpace(value) ? defaultText : value;
        }

        #region Private 字段

        private ObservableCollection<DoTaskModel> doTaskModels = new ObservableCollection<DoTaskModel>();
        private string explanation;
        private bool hasItems = false;

        private int selectedIndex = 0;

        private DoTaskModel selectedItem;
        private string langKey;
        private string defaultText;

        #endregion Private 字段

        #region Public 属性

        /// <summary>
        /// 任务类集合
        /// </summary>
        public ObservableCollection<DoTaskModel> DoTaskModels { get => doTaskModels; set => RegisterProperty(ref doTaskModels, value); }

        /// <summary>
        /// 任务类注解
        /// </summary>
        public string Explanation
        {
            get => explanation;
            set
            {
                if (string.IsNullOrWhiteSpace(defaultText))
                {
                    defaultText = value;
                }
                RegisterProperty(ref explanation, value);
            }
        }

        /// <summary>
        /// 语言Key
        /// </summary>
        public string LangKey
        {
            get => langKey;
            set => RegisterProperty(ref langKey, value);
        }

        /// <summary>
        /// 集合中是否有元素
        /// </summary>
        public bool HasItems
        {
            get
            {
                hasItems = DoTaskModels.Count > 0;
                return hasItems;
            }

            set => RegisterProperty(ref hasItems, value);
        }

        /// <summary>
        /// 当前选择项
        /// </summary>
        public int SelectedIndex
        {
            get => selectedIndex;
            set
            {
                RegisterProperty(ref selectedIndex, value);
            }
        }

        /// <summary>
        /// 当前选择对象
        /// </summary>
        public DoTaskModel SelectedItem
        {
            get
            {
                if (HasItems && SelectedIndex > -1 && selectedItem == null)
                    selectedItem = DoTaskModels[SelectedIndex];
                return selectedItem;
            }
            set
            {
                RegisterProperty(ref selectedItem, value);
            }
        }

        #endregion Public 属性
    }
}