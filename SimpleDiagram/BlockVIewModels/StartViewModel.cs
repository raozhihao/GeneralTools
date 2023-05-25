using System.ComponentModel;
using System.Threading.Tasks;

using GeneralTool.CoreLibrary.Models;

namespace SimpleDiagram.BlockVIewModels
{
    [Description("开始块")]
    public class StartViewModel : BaseBlockViewModel
    {

        public override Task<bool> ExecuteImp(BaseBlockViewModel prevModel, ExcuteCancelTokenSource token)
        {
            return Task.FromResult(true);
        }

    }
}
