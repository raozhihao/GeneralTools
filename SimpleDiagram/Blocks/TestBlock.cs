using GeneralTool.CoreLibrary.WPFHelper.DiagramDesigner.Models;

using SimpleDiagram.BlockVIewModels;

namespace SimpleDiagram.Blocks
{
    public class TestBlock : BaseBlock
    {
        public override BaseBlockViewModel BlockViewModel { get; set; }
        public override void OnCreateInCanvas()
        {
            base.OnCreateInCanvas();
            ConnectorThumbs[Direction.Right].Visibility =
              ConnectorThumbs[Direction.Left].Visibility =
              ConnectorThumbs[Direction.Top].Visibility =
              ConnectorThumbs[Direction.Bottom].Visibility =
               System.Windows.Visibility.Collapsed;
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
