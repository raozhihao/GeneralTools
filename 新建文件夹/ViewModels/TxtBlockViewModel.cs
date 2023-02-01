using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeneralTool.General.Models;

using SimpleDiagram.BlockVIewModels;
using SimpleDiagram.Models;

namespace SimpleDiagram.ViewModels
{
    [Description("文本块")]
    public class TxtBlockViewModel : TrueFalseBaseBlockViewModel
    {
        public TxtModel Txt { get; set; }
     
        public override Task<bool> ExecuteImp(BaseBlockViewModel prevModel, ExcuteCancelTokenSource token)
        {
            return Task.FromResult(true);
        }

    }
}
