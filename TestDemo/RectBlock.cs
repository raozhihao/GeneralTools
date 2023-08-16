using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GeneralTool.CoreLibrary.WPFHelper.DiagramDesigner.Controls;
using GeneralTool.CoreLibrary.WPFHelper.DiagramDesigner.Models;

namespace TestDemo
{
    public class RectBlock : BlockItem
    {
        protected override void OnCreate()
        {
            base.OnCreate();
            this.ConnectorThumbs[Direction.Right].Visibility =
              this.ConnectorThumbs[Direction.Left].Visibility =
              this.ConnectorThumbs[Direction.Top].Visibility =
              this.ConnectorThumbs[Direction.Bottom].Visibility =
               System.Windows.Visibility.Collapsed;
        }

    }
}
