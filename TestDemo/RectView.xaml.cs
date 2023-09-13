using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

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
            dc.RemoveItemsEvent += Dc_RemoveItemsEvent;
        }

        private bool Dc_RemoveItemsEvent(List<BlockItem> args)
        {
            foreach (BlockItem item in args)
            {
                BlockInfoModel tag = item.Tag as BlockInfoModel;
                item.PosChangedEvent -= Test_PosChangedEvent;
                _ = Blocks.Remove(tag.Key);
            }
            return true;
        }

        public void ChangeCanvasSize(double width, double height)
        {
            dc.Width = width;
            dc.Height = height;
        }

        public void AddBlock(BlockInfoModel model)
        {
            if (Blocks.ContainsKey(model.Key))
                throw new Exception("已经添加了相同的项");

            RectBlock test = new RectBlock()
            {
                ID = model.Key,
                Padding = new Thickness(15, 5, 15, 5),
                Header = model.Header,
                Background = System.Windows.Media.Brushes.DarkCyan,
                HeaderCornerRadius = new CornerRadius(0),
                ResizeVisibility = Visibility.Collapsed,
                ContentVisibility = Visibility.Visible,
                ContentRadius = new CornerRadius(0),
                AutoCornerRadius = false,
            };
            test.SetShowText(model.Content);
            Canvas.SetLeft(test, model.Bounds.Left);
            Canvas.SetTop(test, model.Bounds.Top);

            dc.AddItem(test, false);
            _ = test.ApplyTemplate();
            dc.AddTempelte(test);
            test.Apply();

            test.Width = model.Bounds.Width;
            test.Height = model.Bounds.Height;
            test.RotateAngle = 0;
            test.RotateCenterX = test.Width / 2;
            test.RotateCenterY = test.Height / 2;
            test.Tag = model;
            Blocks.Add(model.Key, model);
            test.PosChangedEvent += Test_PosChangedEvent;

        }

        private void Test_PosChangedEvent(BlockItem item, Rect obj)
        {
            BlockInfoModel model = item.Tag as BlockInfoModel;
            model.Bounds = obj;
        }

        public void RemoveBlock(BlockInfoModel model)
        {
            if (Blocks.TryGetValue(model.Key, out _))
            {
                RemoveBlock(model.Key);

            }
        }

        public void RemoveBlock(Guid key)
        {
            if (Blocks.TryGetValue(key, out _))
            {
                _ = Blocks.Remove(key);
                BlockItem first = dc.Children.OfType<BlockItem>().FirstOrDefault(f => f.ID == key);
                if (first != null)
                {
                    first.PosChangedEvent -= Test_PosChangedEvent;
                    first.Remove();
                }
            }
        }

        public void InvalidateBlocks()
        {
            List<BlockItem> collections = dc.Children.OfType<BlockItem>().ToList();
            foreach (KeyValuePair<Guid, BlockInfoModel> item in Blocks)
            {

                //查看当前项是否与其它项有堆叠
                foreach (KeyValuePair<Guid, BlockInfoModel> item1 in Blocks)
                {
                    BlockItem first = collections.FirstOrDefault(f => f.ID == item1.Key);
                    _ = first.GetVertexCoordinates();
                    if (item.Key == item1.Key)
                    {
                        continue;//自己与自己不计算
                    }

                    bool re = IsIntersection(item.Value.Bounds, item1.Value.Bounds);
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
            return bounds2.Contains(bounds1)
|| bounds2.Contains(bounds1.TopLeft)
|| bounds2.Contains(bounds1.TopRight) || bounds2.Contains(bounds1.BottomRight) || bounds2.Contains(bounds1.BottomLeft);
        }

        public void ShowOrHideZoom(Visibility visibility)
        {
            ZoomBoxControl.Visibility = visibility;
        }
    }
}
