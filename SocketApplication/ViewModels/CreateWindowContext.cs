using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GeneralTool.CoreLibrary.WPFHelper;

using SocketApplication.Models;

namespace SocketApplication.ViewModels
{
    public class CreateWindowContext:BaseNotifyModel
    {
        private SocketModel socketModel=new SocketModel();
        public SocketModel SocketModel
        {
            get => this.socketModel; 
            set => this.RegisterProperty(ref this.socketModel, value); 
        }

        
    }
}
