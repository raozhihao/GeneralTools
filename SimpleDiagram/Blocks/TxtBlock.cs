using SimpleDiagram.BlockVIewModels;
using SimpleDiagram.Models;
using SimpleDiagram.Windows;

namespace SimpleDiagram.Blocks
{
    public class TxtBlock : TrueFalseBaseBlock
    {
        public override BaseBlockViewModel BlockViewModel { get; set; } = new TxtBlockViewModel();

        private string txt;
        public override WindowResult OpenWindow()
        {
            var window = new TxtWindow(txt)
            {
                Owner = this.MainWindow
            };

            var re = window.ShowDialog();
            if (re.Value)
            {
                if (this.BlockViewModel is TxtBlockViewModel t)
                {
                    t.Txt = new TxtModel()
                    {
                        Txt = window.ResultTxt,
                        BlockId = t.BlockId,
                        ScriptId = this.LayoutId
                    };
                    this.txt = window.ResultTxt;
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
    }
}
