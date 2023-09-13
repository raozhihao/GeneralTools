using System;
using System.Windows;

using GeneralTool.CoreLibrary.WPFHelper;

namespace TestDemo
{
    public class BlockInfoModel : BaseNotifyModel
    {
        private Guid key;
        public Guid Key
        {
            get => key;
            set => RegisterProperty(ref key, value);
        }

        private string header;
        public string Header
        {
            get => header;
            set => RegisterProperty(ref header, value);
        }

        private string content;
        public string Content
        {
            get => content;
            set => RegisterProperty(ref content, value);
        }

        private Rect bounds;
        public Rect Bounds
        {
            get => bounds;
            set => RegisterProperty(ref bounds, value);
        }
    }
}
