
using GeneralTool.CoreLibrary.WPFHelper.DiagramDesigner.Models;

using SimpleDiagram.BlockVIewModels;

namespace SimpleDiagram.Blocks
{
    public class StartBlock : BaseBlock
    {
        public override BaseBlockViewModel BlockViewModel { get; set; } = new StartViewModel();

        public override void OnCreateInCanvas()
        {
            base.OnCreateInCanvas();
            this.ConnectorThumbs[Direction.Right].Visibility =
              this.ConnectorThumbs[Direction.Left].Visibility =
              this.ConnectorThumbs[Direction.Top].Visibility =
               System.Windows.Visibility.Collapsed;
            this.ConnectorThumbs[Direction.Bottom].ConnectorType = ConnectorType.OnlySource;
            this.IsStart = true;
        }

        public override WindowResult OpenWindow()
        {
            return WindowResult.None;
        }

        public override void SetShow()
        {

        }

        protected override void OnDispose()
        {

        }
    }
}
