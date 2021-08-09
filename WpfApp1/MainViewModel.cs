using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using GeneralTool.General.WPFHelper;

namespace WpfApp1
{
    class MainViewModel : BaseNotifyModel
    {
        private int a = 1;
        public int A { get => this.a; set => this.RegisterProperty(ref this.a, value); }

        private int b = 3;
        public int B
        {
            get => this.b; set => this.RegisterProperty(ref this.b, value);
        }

        public TextBlock TxtBlockName { get; set; }

        private int selectedIndex = 0;
        public int SelectedIndex
        {
            get => this.selectedIndex; set
            {
                this.RegisterProperty(ref this.selectedIndex, value);

                ChangeLangMethod();
            }
        }

        public Button CommandButton { get; set; }

        public ICommand TestCommand
        {
            get => new SimpleCommand(TestMethod);
        }

        private void TestMethod()
        {
            LangProvider.LangProviderInstance.GetLangValue("Log.MyLog");
            //ChangeLangMethod();
        }

        private void ChangeLangMethod()
        {
            var key = this.LangList[this.SelectedIndex];
            LangProvider.LangProviderInstance.ChangeLang(key);

            //获取当前语言文件中key对应的值
            Console.WriteLine(LangProvider.LangProviderInstance.GetLangValue("Text2"));
        }

        public void LangSelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            var key = e.AddedItems[0] + "";

            LangProvider.LangProviderInstance.ChangeLang(key);

            //var CurrentUICultureStr = LangProvider.LangProviderInstance.GetLangValue("CurrentUICulture");

            //var cultureInfo = new CultureInfo(CurrentUICultureStr);

            //Application.Current.MainWindow.Dispatcher.Thread.CurrentUICulture = cultureInfo;
        }

        public ObservableCollection<string> LangList { get; set; } = new ObservableCollection<string>();
        public MainViewModel()
        {
            //下拉框语言选择列表
            this.LangList.Add("中文");
            this.LangList.Add("English");
            //this.LangList.Add("日本語");
            //设置当前的语言包
            var dic = new Dictionary<string, ResourceDictionary>();
            //语言字典 ResourceDictionary 的 Source 的 Uri 必须为绝对路径
            dic.Add("English", new ResourceDictionary() { Source = new Uri($"pack://application:,,,/WpfApp1;component/en-US.xaml") });
            //dic.Add("日本語", new ResourceDictionary() { Source = new Uri($"pack://application:,,,/LangTest;component/Japanese.xaml") });
            // dic.Add("中文", new ResourceDictionary() { Source = new Uri($"pack://application:,,,/LangTest;component/Chinese.xaml") });
            LangProvider.LangProviderInstance.AddLangResources(dic);
            //此属性如果设置,则会默认创建出一个中文的资源包(并不存在),且此key应与语言列表中的某项key保持一致
            LangProvider.LangProviderInstance.DefaultLang = "中文";

            
            this.SelectedIndex = 1;

        }
    }
}