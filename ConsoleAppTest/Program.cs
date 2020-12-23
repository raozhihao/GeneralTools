using GeneralTool.General;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;

namespace ConsoleAppTest
{
    internal class Program
    {
        [STAThread]
        private static void Main(string[] args)
        {
            //RunFrm();
            Form f1 = new Form();

            //var m = GetMyStruct();
            //Sys(f1);
            //Console.WriteLine("1");



            var my = GetMyStruct();
            Custom(GetMyStruct());
            Console.WriteLine("2");




            Console.ReadKey();
        }

        static MyStruct GetMyStruct()
        {
            MyStruct my = new MyStruct();
            my.Dt = new DataTable("dt");
            my.Action = () => { Console.WriteLine("abc"); };
            my.Dt.Columns.Add("ID");
            my.Dt.Rows.Add(1);
            my.Dic = new Dictionary<int, int>() { { 1, 1 } };

            my.Set = new DataSet();
            my.Set.Tables.Add(my.Dt);

            return my;
        }


        static void Custom(object obj)
        {
            Stopwatch watch = new Stopwatch();

            watch.Start();
            GeneralTool.General.SerializeHelpers serialize = new GeneralTool.General.SerializeHelpers();

            byte[] bytes = obj.Serialize();
            var m = bytes.Desrialize();



            watch.Stop();
            Console.WriteLine("Custom " + watch.Elapsed);

        }

        static void Sys(object obj)
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();

            byte[] b = null;
            using (var stream = new MemoryStream())
            {
                var binary = new BinaryFormatter();
                binary.Serialize(stream, obj);
                b = stream.ToArray();
            }

            using (var stream = new MemoryStream(b))
            {
                var binary = new BinaryFormatter();
                obj = binary.Deserialize(stream);
            }



            watch.Stop();

            Console.WriteLine("Memory " + watch.Elapsed);
        }


        private static void RunFrm()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Form1());
        }
    }


    public class MyStruct
    {
        public int X { get; set; }
        public int Y { get; set; }



        public Action Action { get; set; }

        public DataTable Dt { get; set; }

        public DataSet Set { get; set; }

        public Dictionary<int, int> Dic { get; set; }

        public MyStruct mys { get; set; }

    }

}
