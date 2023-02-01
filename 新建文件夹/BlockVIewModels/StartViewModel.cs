using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GeneralTool.General.Models;

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
