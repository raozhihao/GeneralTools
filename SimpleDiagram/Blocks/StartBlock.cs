
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
            ConnectorThumbs[Direction.Right].Visibility =
              ConnectorThumbs[Direction.Left].Visibility =
              ConnectorThumbs[Direction.Top].Visibility =
               System.Windows.Visibility.Collapsed;
            ConnectorThumbs[Direction.Bottom].ConnectorType = ConnectorType.OnlySource;
            IsStart = true;
            ContentRadius = new System.Windows.CornerRadius(20);
            CanResize = false;
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
