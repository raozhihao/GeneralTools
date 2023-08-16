using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using GeneralTool.CoreLibrary.WPFHelper;

namespace TestDemo
{
    public class BlockInfoModel:BaseNotifyModel
    {
        private Guid key;
        public Guid Key
        {
            get => this.key;
            set=>this.RegisterProperty(ref this.key, value);
        }

        private string header;
        public string Header
        {
            get=> this.header;
            set=>this.RegisterProperty(ref this.header, value);
        }

        private string content;
        public string Content
        {
            get => this.content; 
            set => this.RegisterProperty(ref this.content, value);
        }

        private Rect bounds;
        public Rect Bounds
        {
            get => this.bounds; 
            set => this.RegisterProperty(ref this.bounds, value);
        }
    }
}
