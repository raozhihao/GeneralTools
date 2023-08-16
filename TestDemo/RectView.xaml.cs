using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using GeneralTool.CoreLibrary.WPFHelper.DiagramDesigner.Controls;

namespace TestDemo
{
    /// <summary>
    /// RectView.xaml 的交互逻辑
    /// </summary>
    public partial class RectView : UserControl
    {
        public Dictionary<Guid, BlockInfoModel> Blocks { get; private set; } = new Dictionary<Guid, BlockInfoModel>();
        public RectView()
        {
            InitializeComponent();
            this.dc.RemoveItemsEvent += Dc_RemoveItemsEvent;
        }

        private bool Dc_RemoveItemsEvent(List<BlockItem> args)
        {
            foreach (var item in args)
            {
                var tag = item.Tag as BlockInfoModel;
                item.PosChangedEvent -= Test_PosChangedEvent;
                this.Blocks.Remove(tag.Key);
            }
            return true;
        }

        public void ChangeCanvasSize(double width, double height)
        {
            this.dc.Width = width;
            this.dc.Height = height;
        }

        public void AddBlock(BlockInfoModel model)
        {
            if (this.Blocks.ContainsKey(model.Key))
                throw new Exception("已经添加了相同的项");

            var test = new RectBlock()
            {
                ID = model.Key,
                Padding = new Thickness(15, 5, 15, 5),
                Header = model.Header,
                Background = System.Windows.Media.Brushes.DarkCyan,
                HeaderCornerRadius = new CornerRadius(0),
                ResizeVisibility = Visibility.Collapsed,
                ContentVisibility = Visibility.Visible,
                ContentRadius = new CornerRadius(0),
                AutoCornerRadius=false,
            };
            test.SetShowText(model.Content);
            Canvas.SetLeft(test, model.Bounds.Left);
            Canvas.SetTop(test, model.Bounds.Top);

            this.dc.AddItem(test, false);
            test.ApplyTemplate();
            this.dc.AddTempelte(test);
            test.Apply();

            test.Width = model.Bounds.Width;
            test.Height = model.Bounds.Height;
            test.RotateAngle = 0;
            test.RotateCenterX = test.Width / 2;
            test.RotateCenterY = test.Height / 2;
            test.Tag = model;
            this.Blocks.Add(model.Key, model);
            test.PosChangedEvent += Test_PosChangedEvent;

        }

        private void Test_PosChangedEvent(BlockItem item, Rect obj)
        {
            var model = item.Tag as BlockInfoModel;
            model.Bounds = obj;
        }

        public void RemoveBlock(BlockInfoModel model)
        {
            if (this.Blocks.TryGetValue(model.Key, out var test))
            {
                this.RemoveBlock(model.Key);

            }
        }

        public void RemoveBlock(Guid key)
        {
            if (this.Blocks.TryGetValue(key, out _))
            {
                this.Blocks.Remove(key);
                var first = this.dc.Children.OfType<BlockItem>().FirstOrDefault(f => f.ID == key);
                if (first != null)
                {
                    first.PosChangedEvent -= Test_PosChangedEvent;
                    first.Remove();
                }
            }
        }

        public void InvalidateBlocks()
        {
            var collections = this.dc.Children.OfType<BlockItem>().ToList();
            foreach (var item in this.Blocks)
            {
                
                //查看当前项是否与其它项有堆叠
                foreach (var item1 in this.Blocks)
                {
                    var first = collections.FirstOrDefault(f => f.ID == item1.Key);
                    first.GetVertexCoordinates();
                    if (item.Key == item1.Key)
                    {
                        continue;//自己与自己不计算
                    }


                    var re = this.IsIntersection(item.Value.Bounds, item1.Value.Bounds);
                    if (re)
                        throw new Exception($"{item.Value.Header} 与 {item1.Value.Header} 有堆叠");
                }
            }
        }

        private bool IsIntersection(Rect bounds1, Rect bounds2)
        {
            //计算四个顶点是否与另一个框有交集则行
            if (bounds1.Contains(bounds2))
                return true;

            if (bounds1.Contains(bounds2.TopLeft))
                return true;

            if (bounds1.Contains(bounds2.TopRight))
                return true;

            if (bounds1.Contains(bounds2.BottomRight))
                return true;

            if (bounds1.Contains(bounds2.BottomLeft))
                return true;

            //反向查询
            if (bounds2.Contains(bounds1))
                return true;

            if (bounds2.Contains(bounds1.TopLeft))
                return true;

            if (bounds2.Contains(bounds1.TopRight))
                return true;

            if (bounds2.Contains(bounds1.BottomRight))
                return true;

            if (bounds2.Contains(bounds1.BottomLeft))
                return true;

            return false;
        }

        public void ShowOrHideZoom(Visibility visibility)
        {
            this.ZoomBoxControl.Visibility = visibility;
        }
    }
}
