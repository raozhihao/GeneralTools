using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

using GeneralTool.CoreLibrary.Attributes;
using GeneralTool.CoreLibrary.DbHelper;
using GeneralTool.CoreLibrary.Extensions;
using GeneralTool.CoreLibrary.IniHelpers;
using GeneralTool.CoreLibrary.Interfaces;

using Newtonsoft.Json;

namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var window = new WindowNode(nameof(WindowNode));
            var m = window.Node.Value;

            m.Value = 0.55;
            m.Name = "update";
            window.Node.Value = m;
        }
    }

    public class WindowNode : Category
    {
        public WindowNode(string sectionName) : base(sectionName)
        {
            this.Node = new Node<Model>(this.SectionName, nameof(this.Node), new Model() { Id=1, Value=0.2}, true,"",true);
        }

       
        public Node<Model> Node { get; set; }
    }

    public class MyJsonConveter : IJsonConvert
    {
        public T DeserializeObject<T>(string value)
        {
            return (T)this.DeserializeObject(value, typeof(T));
        }

        public object DeserializeObject(string value, Type type)
        {
            return JsonConvert.DeserializeObject(value, type);
        }

        public string SerializeObject(object serverResponse)
        {
            return JsonConvert.SerializeObject(serverResponse);
        }
    }

    public class Model
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public TypeCode aypeCode { get; set; }
        public double Value { get; set; }


        // public Model Mc { get; set; }

        // public List<MC> Strings { get; set; }
    }

    public class MC
    {
        public int Id { get; set; }
        public string Abc { get; set; }
    }
}
