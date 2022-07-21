using GeneralTool.General;
using GeneralTool.General.DataSetExtensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;

namespace ConsoleAppTest
{
    internal class Program
    {
        [STAThread]
        private static void Main(string[] args)
        {
            new SocketTest().Test();
            //// TestPostWcf();
            //TestTableConvert();

            //ProcessHelper.Run(@"C:\Demo\DemoProject\MaterialWpf\bin\Debug\MaterialWpf.exe", out var result);

            Console.WriteLine("完成");
            Console.ReadKey();
        }

        private static void TestTableConvert()
        {
            var table = InitTable();
            var doc = table.ToXmlDocument();
            var dt = doc.ToDataTable();
        }

        private static async void TestPostWcf()
        {
            var Client = new HttpClient();

            var table = InitTable();
            var xmlStr = table.ToXmlString();
            var json = JsonConvert.SerializeObject(new { table = xmlStr });


            var content = new StringContent(json);


            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            try
            {
                var resultConent = await Client.PostAsync("http://localhost:15500/ServiceAjax.svc/ParamterTable", content);

                var result = await resultConent.Content.ReadAsStringAsync();

            }
            catch (Exception ex)
            {
            }
        }

        private static DataTable InitTable()
        {
            var table = new DataTable("ta");
            table.Columns.Add("ID");
            table.Columns.Add("Name");
            for (int i = 0; i < 5; i++)
            {
                table.Rows.Add(i, i + 1);
            }

            table.AcceptChanges();
            table.Rows.Add(1, "2");
            table.Rows[0]["Name"] = "33";
            table.Rows[1].Delete();
            return table;
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
