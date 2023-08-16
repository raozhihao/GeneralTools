using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            this.ConnectorThumbs[Direction.Right].Visibility =
              this.ConnectorThumbs[Direction.Left].Visibility =
              this.ConnectorThumbs[Direction.Top].Visibility =
              this.ConnectorThumbs[Direction.Bottom].Visibility =
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
