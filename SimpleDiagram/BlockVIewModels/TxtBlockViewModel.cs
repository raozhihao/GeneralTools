using System.ComponentModel;
using System.Threading.Tasks;

using GeneralTool.CoreLibrary.Models;

using SimpleDiagram.Models;

namespace SimpleDiagram.BlockVIewModels
{
    [Description("文本块")]
    public class TxtBlockViewModel : TrueFalseBaseBlockViewModel
    {
        public TxtModel Txt { get; set; }

        public override Task<bool> ExecuteImp(BaseBlockViewModel prevModel, ExcuteCancelTokenSource token)
        {
            this.Log.Info($"执行文本:[{this.Txt?.Txt}]");
            return Task.FromResult(true);
        }

    }
}
