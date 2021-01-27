using GeneralTool.General;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using GeneralTool.General.WebExtensioins;

namespace ConsoleAppTest
{
    internal class Program
    {
        [STAThread]
        private static void Main(string[] args)
        {
            var tu = PageHelper.Pages(8, 9, 7);


            for (int i = 1; i < 100; i++)
            {
                for (int j = i; j < 1000; j++)
                {
                    for (int k = 1; k < 100; k++)
                    {
                        var tt = PageHelper.Pages(i, j, k);

                        var count = tt.Item2 - tt.Item1 + 1;
                        bool re = count == k || count == j;
                        if (!re)
                        {
                            Console.WriteLine($"pageIndex:{i},pageSum:{j},pageCount:{k} == {re},result:{tt.Item1}--{tt.Item2}");

                        }
                        
                    }

                }
            }

            Console.WriteLine("完成");
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
