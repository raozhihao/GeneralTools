using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using GeneralTool.General.Models;
using GeneralTool.General.WPFHelper;

namespace LangDemo
{
    public class MainViewModel : BaseNotifyModel
    {
        /// <summary>
        /// 语言列表
        /// </summary>
        public ObservableCollection<string> LangList { get; set; } = new ObservableCollection<string>();

        private int selectedIndex = 0;
        /// <summary>
        /// 当前选择的语言下标
        /// </summary>
        public int SelectedIndex { get => this.selectedIndex; set => this.RegisterProperty(ref this.selectedIndex, value); }

        public ObservableCollection<LogMessageInfo> Logs { get; set; } = new ObservableCollection<LogMessageInfo>();

        public MainViewModel()
        {
            //将资源中的语言文件加入到语言服务中
            var langResources = new Dictionary<string, ResourceDictionary>();
            langResources.Add("中文", new ResourceDictionary() { Source = new Uri("pack://application:,,,/LangDemo;component/LangResouce/Chinese.xaml") });
            langResources.Add("English", new ResourceDictionary() { Source = new Uri("pack://application:,,,/LangDemo;component/LangResouce/English.xaml") });
            langResources.Add("日本語", new ResourceDictionary() { Source = new Uri("pack://application:,,,/LangDemo;component/LangResouce/Japanese.xaml") });

            this.LangList.Add("中文");
            this.LangList.Add("English");
            this.LangList.Add("日本語");
            LangProvider.LangProviderInstance.AddLangResources(langResources);
            //加载语言
            this.ChangeLangMethod();
        }

        public ICommand ChangeLangCommand { get => new SimpleCommand(ChangeLangMethod); }

        private void ChangeLangMethod()
        {
            LangProvider.LangProviderInstance.ChangeLang(this.LangList[this.SelectedIndex]);
        }

        public ICommand OpenNewWindowCommand { get => new SimpleCommand(OpenNewWindowMethod); }

        private void OpenNewWindowMethod()
        {
            new NewWindow().Show();
        }

        public ICommand OutLogCommand { get => new SimpleCommand(OutLogMethod); }

        private void OutLogMethod()
        {
            //从日志文件中读取
            var log = LangProvider.LangProviderInstance.GetLangValue("Logs.SetLog");
            this.WriteLog(log);
            //格式化日志
            log = LangProvider.LangProviderInstance.GetLangValueFomart("Logs.FomartLog", "格式化1", "格式化2");
            this.WriteLog(log);
        }

        private void WriteLog(string Logs) => this.Logs.Add(new LogMessageInfo(Logs, GeneralTool.General.Enums.LogType.Info, ""));
    }
}
