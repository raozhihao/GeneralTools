using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;

using GeneralTool.CoreLibrary.WPFHelper.DiagramDesigner.Controls;
using GeneralTool.CoreLibrary.WPFHelper.DiagramDesigner.Models;

namespace GeneralTool.CoreLibrary.WPFHelper.DiagramDesigner.Common
{
    public class HistoryManger
    {
        public List<HistoryModel> HistoryModels { get; set; } = new List<HistoryModel>();

        public int CurrentIndex { get; set; } = -1;

        private readonly DesignerCanvas designerCanvas;
        public HistoryManger(DesignerCanvas canvas)
        {
            designerCanvas = canvas;
        }

        public void AddHistoryModel(HistoryModel historyModel)
        {
            HistoryModel first = HistoryModels.FirstOrDefault(f => f.Item == historyModel.Item);
            if (first != null) _ = HistoryModels.Remove(first);
            HistoryModels.Add(historyModel);
        }


        public void Undo()
        {
            HistoryModel current = null;

            if (CurrentIndex == -1)
            {
                if (HistoryModels.Count == 0)
                {
                    CurrentIndex = -1;
                    return;
                }
                else if (HistoryModels.Count > 1)
                {
                    CurrentIndex = HistoryModels.Count - 1;
                    current = HistoryModels[CurrentIndex];
                }

            }
            else if (CurrentIndex == 0)
            {
                current = null;
            }
            else if (CurrentIndex > 0)
            {
                CurrentIndex--;
                current = HistoryModels[CurrentIndex];
                if (current.Item.IsStart)
                {
                    CurrentIndex++;
                    return;
                }
            }

            if (current == null) return;


            ReLoad(current);
        }

        private void ReLoad(HistoryModel current)
        {

            switch (current.HistoryType)
            {
                case HistoryType.Add:
                    designerCanvas.RemoveItem(current.Item);
                    break;
                case HistoryType.Delete:
                    //重新添加进去
                    System.Windows.DependencyObject p = VisualTreeHelper.GetParent(current.Item);
                    if (p != null)
                    {
                        _ = HistoryModels.Remove(current);
                        return;
                    }
                    designerCanvas.AddItem(current.Item, false);
                    break;
            }
        }

        public void Next()
        {
            //获取当前的
            if (CurrentIndex < 0) return;

            if (CurrentIndex >= HistoryModels.Count)
            {
                CurrentIndex = HistoryModels.Count;
                return;
            }

            //当前项
            HistoryModel current = HistoryModels[CurrentIndex];

            CurrentIndex++;
            ReLoad(current);
        }

        public void Clear()
        {
            HistoryModels.Clear();
        }
    }
}
