using System.Linq;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;

using GeneralTool.CoreLibrary.WPFHelper.DiagramDesigner.Adorners;
using GeneralTool.CoreLibrary.WPFHelper.DiagramDesigner.Controls;
using GeneralTool.CoreLibrary.WPFHelper.DiagramDesigner.Models;

namespace GeneralTool.CoreLibrary.WPFHelper.DiagramDesigner.Thumbs
{
    /// <summary>
    /// 
    /// </summary>
    public class ConnectorThumb : Thumb
    {
        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty DirectionProperty = DependencyProperty.Register(nameof(Direction), typeof(Direction), typeof(ConnectorThumb));
        /// <summary>
        /// 
        /// </summary>
        public Direction Direction
        {
            get => (Direction)GetValue(DirectionProperty);
            set => SetValue(DirectionProperty, value);
        }

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty ConnectorTypeProperty = DependencyProperty.Register(nameof(ConnectorType), typeof(ConnectorType), typeof(ConnectorThumb));

        /// <summary>
        /// 
        /// </summary>
        public ConnectorType ConnectorType
        {
            get => (ConnectorType)GetValue(ConnectorTypeProperty);
            set => SetValue(ConnectorTypeProperty, value);
        }

        //public static readonly DependencyProperty SinkCountProperty = DependencyProperty.Register(nameof(SinkCount), typeof(int), typeof(ConnectorThumb), new PropertyMetadata(1));
        ///// <summary>
        ///// 可作为目标点的数量
        ///// </summary>
        //public int SinkCount
        //{
        //    get => (int)this.GetValue(SinkCountProperty);
        //    set => this.SetValue(SinkCountProperty, value);
        //}

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty SourceCountProperty = DependencyProperty.Register(nameof(SourceCount), typeof(int), typeof(ConnectorThumb), new PropertyMetadata(1));
        /// <summary>
        /// 可作为起点的数量
        /// </summary>
        public int SourceCount
        {
            get => (int)GetValue(SourceCountProperty);
            set => SetValue(SourceCountProperty, value);
        }

        /// <summary>
        /// 
        /// </summary>
        public ConnectorThumb()
        {
            Loaded += ConnectorThumb_Loaded;
        }

        /// <summary>
        /// 
        /// </summary>
        public BlockItem SourceBlock { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public AdornerLayer Layer { get; set; }
        private void ConnectorThumb_Loaded(object sender, RoutedEventArgs e)
        {
            SourceBlock = DataContext as BlockItem;
            if (SourceBlock != null)
                Layer = AdornerLayer.GetAdornerLayer(SourceBlock);
        }

        private bool isDrag;
        /// <summary>
        /// 
        /// </summary>
        public ConnectorAdorner Connector { get; set; }
        /// <summary>
        /// 
        /// </summary>
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            SinkBlock = null;
            if (SourceBlock == null || !SourceBlock.ParentCanvas.CanOperation)
            {
                return;
            }
            //读取当前可作为起点的数量
            int count = SourceBlock.ParentCanvas.Children.OfType<Connection>().Where(c => c.SourceThumb == this).Count();
            if (count > 0)
            {
                isDrag = false;
                return;
            }

            if (ConnectorType == ConnectorType.OnlySink)
            {
                isDrag = false;
                return;//仅作为目标点,不允许拉动
            }

            isDrag = true;
            if (isDrag && SourceBlock != null && Layer != null)
            {
                //创建一个Adorner
                Cursor = Cursors.Cross;
                Point startPoint = TranslatePoint(new Point(ActualWidth / 2, ActualHeight / 2), SourceBlock);
                Connector = new ConnectorAdorner(SourceBlock, startPoint, this);

                Layer.Add(Connector);
                e.Handled = true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (isDrag && SourceBlock != null && Layer != null)
            {
                //获取Canvas

                Point point = e.GetPosition(SourceBlock);
                Connector.End = point;
                e.Handled = true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public BlockItem SinkBlock { get; set; }
        /// <summary>
        /// 
        /// </summary>
        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonUp(e);

            Cursor = null;
            isDrag = false;
            if (Connector == null) return;
            DesignerCanvas canvas = SourceBlock.Parent as DesignerCanvas;
            if (SourceBlock != null && Layer != null)
            {
                //找到落点,如果找到就将落点的Item也一并找到,在其变换位置时,更新End坐标
                //获取Canvas
                ConnectorThumb destThumb = canvas?.HitConnectorItem(e);

                //判断目标点是否可作为终点使用
                if (destThumb != null && (destThumb.ConnectorType == ConnectorType.None || destThumb.ConnectorType == ConnectorType.OnlySink))
                {
                    //读取可连接的目标数量
                    // var count = this.SourceBlock.ParentCanvas.Children.OfType<Connection>().Where(c => c.SinkThumb == destThumb).Count();

                    //找到目标的点了
                    //if (!destThumb.SourceBlock.Equals(this.SourceBlock) && destThumb.SinkCount != count)

                    //if (destThumb.SourceBlock != null&&!destThumb.SourceBlock.Equals(this.SourceBlock)&&this.GetCanConnectThumbs(destThumb.Direction)&&!this.SourceBlock.SinkItems.Contains(destThumb.SourceBlock))
                    if (GetCanConnectThumbs(destThumb.Direction))
                    {
                        //一个源只能对应一个相同的目标点,两个块之间不能互连
                        //当前块是否已经拥有目标点了,以及当前目标点是否作为源时是否已经拥有当前块做为目标点
                        //if (!this.SourceBlock.ContainsSink(destThumb.SourceBlock) && !destThumb.SourceBlock.ContainsSink(this.SourceBlock))
                        //{
                        //获取目标点的中间点
                        Point destCenterPoint = new Point(destThumb.RenderSize.Width / 2, destThumb.RenderSize.Height / 2);
                        //中间点在canvas中的位置
                        Point canvasPoint = destThumb.TranslatePoint(destCenterPoint, canvas);
                        //相对于当前源的位置
                        Point thisPoint = canvas.TranslatePoint(canvasPoint, SourceBlock);
                        Connector.End = thisPoint;

                        // this.connector.DestItem = destThumb.SourceBlock;
                        Connector.DestThumb = destThumb;
                        // destThumb.SourceBlock = this.SourceBlock;
                        //在当前块的目标集合中添加目标块
                        SourceBlock.AddSinkItem(destThumb.SourceBlock);
                        //在当前目标块的源集合中添加当前块
                        // destThumb.SourceBlock.AddSourceItem(this.SourceBlock);

                        canvas.AddConnection(new Connection(this, destThumb));
                    }
                    //if (destThumb.SourceBlock==null&&!destThumb.SourceBlock.Equals(this.SourceBlock)&&this.GetCanConnectThumbs(destThumb.Direction)&&!this.SourceBlock.SinkItems.Contains(destThumb.SourceBlock))
                    //{

                    //    //  }
                    //}
                }
            }

            if (Connector != null)
                Layer.Remove(Connector);
            ReleaseMouseCapture();
        }

        /// <summary>
        /// 
        /// </summary>
        public bool GetCanConnectThumbs(Direction direction)
        {
            return directions == null || directions.Contains(direction);
        }

        private Direction[] directions;
        /// <summary>
        /// 设定当前能连接的目标点的方向类型
        /// </summary>
        /// <param name="directions"></param>
        public void SetCanSinkDirections(params Direction[] directions)
        {
            this.directions = directions;
        }
    }
}
