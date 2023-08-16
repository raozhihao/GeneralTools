using System.Windows;

using SimpleDiagram.BlockVIewModels;
using SimpleDiagram.Models;
using SimpleDiagram.Windows;

namespace SimpleDiagram.Blocks
{
    public class TxtBlock : TrueFalseBaseBlock
    {
        public override BaseBlockViewModel BlockViewModel { get; set; } = new TxtBlockViewModel();

        public override WindowResult OpenWindow()
        {
            if (!(this.BlockViewModel is TxtBlockViewModel t))
            {
                return WindowResult.False;
            }
            var window = new TxtWindow(t.Txt?.Txt)
            {
                Owner = this.MainWindow
            };

            var re = window.ShowDialog();
            if (re.Value)
            {

                {
                    t.Txt = new TxtModel()
                    {
                        Txt = window.ResultTxt,
                        BlockId = t.BlockId,
                        ScriptId = this.LayoutId
                    };
                }
                this.SetContent();
            }
            return this.GetResult(re);
        }



        private void SetContent()
        {
            if (this.BlockViewModel is TxtBlockViewModel t)
            {
                this.SetShowText(t.Txt.Txt);
            }
        }

        protected override void OnDispose()
        {

        }

        public override void SetShow()
        {
            this.SetContent();
        }
    }
}
