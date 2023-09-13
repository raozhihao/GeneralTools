using GeneralTool.CoreLibrary.WPFHelper.DiagramDesigner.Controls;
using GeneralTool.CoreLibrary.WPFHelper.DiagramDesigner.Models;

namespace TestDemo
{
    public class RectBlock : BlockItem
    {
        protected override void OnCreate()
        {
            base.OnCreate();
            ConnectorThumbs[Direction.Right].Visibility =
              ConnectorThumbs[Direction.Left].Visibility =
              ConnectorThumbs[Direction.Top].Visibility =
              ConnectorThumbs[Direction.Bottom].Visibility =
               System.Windows.Visibility.Collapsed;
        }

    }
}
