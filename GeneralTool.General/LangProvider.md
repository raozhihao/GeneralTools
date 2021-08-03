### GeneralTool.General 部分使用指南

#### WPF 语言管理

> 在项目中添加表示语言的字典文件,随意命名,例如添加一个English.xaml
```
<ResourceDictionary xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
                    xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
                    xmlns:sys="clr-namespace:System;assembly=mscorlib">
    <sys:String x:Key="Text1">Test1</sys:String>
    <sys:String x:Key="Text2">Test2</sys:String>

</ResourceDictionary>
```
> 在需要更改语言的代码位置如下使用,进行语言注册
```
 private void Init()
 {
     //下拉框语言选择列表
     this.LangList.Add("中文");
     this.LangList.Add("English");
     //this.LangList.Add("日本語");
     //设置当前的语言包
     var dic = new Dictionary<string, ResourceDictionary>();
     //语言字典 ResourceDictionary 的 Source 的 Uri 必须为绝对路径
     dic.Add("English", new ResourceDictionary() { Source = new Uri($"pack://application:,,,/LangTest;component/English.xaml") });
     //dic.Add("日本語", new ResourceDictionary() { Source = new Uri($"pack://application:,,,/LangTest;component/Japanese.xaml") });
     // dic.Add("中文", new ResourceDictionary() { Source = new Uri($"pack://application:,,,/LangTest;component/Chinese.xaml") });
     LangProvider.LangProviderInstance.AddLangResources(dic);
     //此属性如果设置,则会默认创建出一个中文的资源包(并不存在),且此key应与语言列表中的某项key保持一致
     LangProvider.LangProviderInstance.DefaultLang = "中文";
 }

 private void ChangeLangMethod()
 {
     var key = this.LangList[this.SelectedIndex];
     LangProvider.LangProviderInstance.ChangeLang(key);

     //获取当前语言文件中key对应的值
     Console.WriteLine(LangProvider.LangProviderInstance.GetLangValue("Text2"));
 }
```
##### 1.使用LangKey 与 LangBinding

xaml,导入命名空间:xmlns:e="clr-namespace:GeneralTool.General.WPFHelper.Extensions;assembly=GeneralTool.General"<br>
```
 <TextBlock Text="测试2"
            e:LangHelper.LangBinding="Text"
            e:LangHelper.LangKey="Text2"/>
```

##### 1.使用 LangMarkup 和 LangExtension

xaml,导入命名空间:xmlns:e="clr-namespace:GeneralTool.General.WPFHelper.Extensions;assembly=GeneralTool.General"<br>
```
 <TextBlock Text="测试2"
            e:LangHelper.LangMarkup="{e:Lang BindingProperty=Text,LangKey=Text2}" />
```





