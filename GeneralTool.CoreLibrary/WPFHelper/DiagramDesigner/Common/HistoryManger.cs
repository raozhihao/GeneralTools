using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

using GeneralTool.CoreLibrary.WPFHelper.DiagramDesigner.Controls;
using GeneralTool.CoreLibrary.WPFHelper.DiagramDesigner.Models;

namespace GeneralTool.CoreLibrary.WPFHelper.DiagramDesigner.Common
{
    public class HistoryManger
    {
        public List<HistoryModel> HistoryModels { get; set; } = new List<HistoryModel>();

        public int CurrentIndex { get; set; } = -1;

        private DesignerCanvas designerCanvas;
        public HistoryManger(DesignerCanvas canvas)
        {
            this.designerCanvas = canvas;
        }

        public void AddHistoryModel(HistoryModel historyModel)
        {
            var first = this.HistoryModels.FirstOrDefault(f => f.Item == historyModel.Item);
            if (first != null) this.HistoryModels.Remove(first);
            this.HistoryModels.Add(historyModel);
        }


        public void Undo()
        {
            HistoryModel current = null;

            if (this.CurrentIndex == -1)
            {
                if (this.HistoryModels.Count == 0)
                {
                    this.CurrentIndex = -1;
                    return;
                }
                else if (this.HistoryModels.Count > 1)
                {
                    this.CurrentIndex = this.HistoryModels.Count - 1;
                    current = this.HistoryModels[this.CurrentIndex];
                }

            }
            else if (this.CurrentIndex == 0)
            {
                current = null;
            }
            else if (this.CurrentIndex > 0)
            {
                this.CurrentIndex--;
                current = this.HistoryModels[this.CurrentIndex];
                if (current.Item.IsStart)
                {
                    this.CurrentIndex++;
                    return;
                }
            }

            if (current == null) return;


            this.ReLoad(current);
        }

        private void ReLoad(HistoryModel current)
        {

            switch (current.HistoryType)
            {
                case HistoryType.Add:
                    this.designerCanvas.RemoveItem(current.Item);
                    break;
                case HistoryType.Delete:
                    //重新添加进去
                    var p = VisualTreeHelper.GetParent(current.Item);
                    if (p != null)
                    {
                        this.HistoryModels.Remove(current);
                        return;
                    }
                    this.designerCanvas.AddItem(current.Item, false);
                    break;
            }
        }

        public void Next()
        {
            //获取当前的
            if (this.CurrentIndex < 0) return;

            if (this.CurrentIndex >= this.HistoryModels.Count)
            {
                this.CurrentIndex = this.HistoryModels.Count;
                return;
            }

            //当前项
            var current = this.HistoryModels[this.CurrentIndex];

            this.CurrentIndex++;
            this.ReLoad(current);
        }

        public void Clear()
        {
            this.HistoryModels.Clear();
        }
    }
}
