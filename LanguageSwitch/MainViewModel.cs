using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Windows;

using GeneralTool.General.Enums;
using GeneralTool.General.Models;
using GeneralTool.General.WPFHelper;

namespace LanguageSwitch
{
    public class MainViewModel : BaseNotifyModel
    {
        public ObservableCollection<LogMessageInfo> LogMessages { get; set; } = new ObservableCollection<LogMessageInfo>();

        private ObservableCollection<string> languageInfos = new ObservableCollection<string>();
        public ObservableCollection<string> LanguageInfos
        {
            get => this.languageInfos;
            set => this.RegisterProperty(ref this.languageInfos, value);
        }

        private int selectedLangIndex = -1;
        public int SelectedLangIndex
        {
            get => this.selectedLangIndex;
            set
            {
                this.ChangedLang(value);
                this.RegisterProperty(ref this.selectedLangIndex, value);
            }
        }


        public string Text { get; set; }

        public SimpleCommand ExcptionCommand => new SimpleCommand(ExcptionMethod);

        private void ExcptionMethod()
        {
            try
            {
                var a = 0;
                var b = 1 / a;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ChangedLang(int value)
        {
            if (value < 0) return;

            if (this.LanguageInfos.Count - 1 < value) return;

            var key = this.LanguageInfos[value];
            LangProvider.LangProviderInstance.ChangeLang(key);

            //获取Logs
            var loadingStr = LangProvider.LangProviderInstance.GetLangValueFomart("Logs.ChangeLang", key);
            this.LogInfo(loadingStr);

            //更新区域语言
            var cul = LangProvider.LangProviderInstance.GetLangValue("CultureInfo");
            if (!string.IsNullOrWhiteSpace(cul))
            {
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(cul);
            }
        }

        public MainViewModel()
        {

        }

        public void Init()
        {
            //动态加载语言包,加载本地文件
            var dir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Asserts");

            var resourcesDic = new Dictionary<string, ResourceDictionary>();


            foreach (var item in Directory.GetFiles(dir, "*.xaml"))
            {

                var name = Path.GetFileNameWithoutExtension(item);
                //resourcesDic.Add(name, xamlDic);
                var r = new ResourceDictionary() { Source = new Uri(item, UriKind.Absolute) };
                resourcesDic.Add(name, r);
                this.LanguageInfos.Add(name);
            }

            LangProvider.LangProviderInstance.AddLangResources(resourcesDic);
            this.SelectedLangIndex = 0;


        }

        private void LogInfo(string loadingStr, LogType type = LogType.Info)
        {
            this.LogMessages.Add(new LogMessageInfo(loadingStr, type));
        }

        internal void Loaded()
        {
            var loadingStr = LangProvider.LangProviderInstance.GetLangValue("Logs.LoadingStartup");
            this.LogInfo(loadingStr);
        }
    }

    public class LanguageInfo
    {
        public LanguageInfo(string content, bool isChecked)
        {
            Content = content;
            IsChecked = isChecked;
        }

        public string Content { get; set; }

        public bool IsChecked { get; set; }


    }
}
