using GeneralTool.CoreLibrary.WPFHelper.DiagramDesigner.Models;

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
