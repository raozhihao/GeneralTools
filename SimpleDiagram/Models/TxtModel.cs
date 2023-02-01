namespace SimpleDiagram.Models
{
    public class TxtModel : BaseData
    {
        private string txt = "";
        public string Txt
        {
            get => this.txt;
            set => this.RegisterProperty(ref this.txt, value);
        }

    }
}
