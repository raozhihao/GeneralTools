using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GeneralTool.General.Models;
using GeneralTool.General.WPFHelper.DiagramDesigner.Models;

namespace SimpleDiagram.BlockVIewModels
{
    public abstract class TrueFalseBaseBlockViewModel : BaseBlockViewModel
    {
        public BaseBlockViewModel TrueViewModel { get; set; }
        public BaseBlockViewModel FalseViewModel { get; set; }

        public override void SetProperty(ConnectionDo connection, BaseBlockViewModel newxtModel)
        {
            var sourceDirection = connection.SourceDirection;
            if (sourceDirection == Direction.Bottom)
                this.TrueViewModel = newxtModel;
            if (sourceDirection == Direction.Right)
                this.FalseViewModel = newxtModel;
        }


    }
}
