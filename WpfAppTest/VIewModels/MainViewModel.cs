using System;
using System.Windows;
using System.Windows.Input;
using GeneralTool.General.WPFHelper;
using GeneralTool.General.WPFHelper.Events;

namespace WpfAppTest.VIewModels
{
    class MainViewModel : BaseNotifyModel
    {
        private string resultText;
        public string ResultText { get => this.resultText; set => this.RegisterProperty(ref this.resultText, value); }

        public EventCommand<MouseEventArgs> MoveCommand { get; set; }
        public EventCommand<MouseButtonEventArgs> RightClickCommand { get; set; }
        public EventCommand<RoutedEventArgs> LoadCommand { get; set; }
        public EventCommand<MouseButtonEventArgs> TextClickCommand { get; set; }


        public MainViewModel()
        {
            this.MoveCommand = new EventCommand<MouseEventArgs>(DockPanelMouseMoveEventMethod);
            this.RightClickCommand = new EventCommand<MouseButtonEventArgs>(DockPanelMouseRightClickEventMethod);
            this.LoadCommand = new EventCommand<RoutedEventArgs>(WindowLoadedEventMethod);
            this.TextClickCommand = new EventCommand<MouseButtonEventArgs>(TextClickEventMethod);
        }

        private void TextClickEventMethod(object sender, MouseButtonEventArgs e)
        {
            this.TextClickCommand.RemoveEvent();
        }

        private void WindowLoadedEventMethod(object sender, RoutedEventArgs e)
        {
            (sender as Window).Title = "Window Loaded";
        }

        private void DockPanelMouseRightClickEventMethod(object sender, MouseButtonEventArgs e)
        {
            this.ResultText = $"{sender} | {e.RightButton} | args : {this.RightClickCommand.CommandParameter}";
        }

        private void DockPanelMouseMoveEventMethod(object sender, MouseEventArgs e)
        {

            this.ResultText = $"{sender} | {e.GetPosition((IInputElement)sender)} | args : {this.MoveCommand.CommandParameter}";
        }
    }
}
