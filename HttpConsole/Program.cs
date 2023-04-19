﻿using System;

using GeneralTool.General.Interfaces;
using GeneralTool.General.TaskLib;

using HttpConsole.Inis;

using Newtonsoft.Json;

using GeneralTool.General.Extensions;
using System.Data;

namespace HttpConsole
{
    internal class Program
    {
        static void Main(string[] args)
        {
            #region 值转换

            //var point = new Point(1, 33) + "";
            //var converter = new StringConverter();
            //object result = null;
            // result= converter.ConvertBack(point, typeof(Point), null, null);

            //枚举
            //result = converter.ConvertBack(FileMode.Truncate+"", typeof(FileMode), null, null);
            //result = converter.ConvertBack(((int)FileMode.Truncate) + "", typeof(FileMode), null, null);

            // 基元类型
            var arr = new object[] { "aaa", 1, 3.2d, 3.2f, (byte)12, (long)2222, (short)33 };
            //foreach (var item in arr)
            //{
            //    result = converter.Convert(item, null, null, null);
            //    result = converter.ConvertBack(result, item.GetType(), null, null);
            //}

            //result = converter.Convert(arr, null, null, null);
            //result = converter.ConvertBack(result, arr.GetType(), null, null);

            //自定义结构
            //result = converter.ConvertBack(new MyStruct() { X = 33, Y = 45 } + "", typeof(MyStruct), null, null);

            //var st = new MyStruct() { X = 33, Y = 45 };
            //var json = converter.Convert(st, null, null, null);
            //result = converter.ConvertBack(json, typeof(MyStruct), null, null);

            //decimal d = 0.222m;
            //result = converter.ConvertSimpleType(d, typeof(double));
            #endregion

            #region Task

            //var httpStation = new HttpServerStation(null, new MyJson());

            //var manager = new TaskManager(null, null, httpStation);
            //var task = new TaskLib();
            //manager.Open("127.0.0.1", 8878, task);
            //manager.GetInterfaces();
            //Console.ReadKey();

            #endregion


            //var p = new Point(1, 2);
            //var str = new StringConverter().Convert(p, null, null, null);
            //str = new StringConverter().ConvertSimpleType(str + "", p.GetType());

            //var arr1 = new int[] { 1, 2, 3 };
            //var reslt = arr1.FomartDatas();


            #region IniTest

            //var node = new WindowNode("Window");
            //var s = node.Str.Value;
            //node.Str.Value = "bbc";

            #endregion


            var table = new DataTable();
            table.Columns.Add("Id", typeof(int));
            table.Columns.Add("Name", typeof(string));
            table.Columns.Add("M1", typeof(M1));

            table.Rows.Add(1, "aaa",new M1() { Checked=true});
            table.Rows.Add(1, "bbb");

            var list=table.ToList<Model>();

            var t2= list.ToDataTable();
        }
    }

    public class Model
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public M1 M1 { get; set; }
    }

    public class M1
    {
        public bool Checked { get; set; }
    }

    public class MyJson : IJsonConvert
    {
        public T DeserializeObject<T>(string value)
        {
            return JsonConvert.DeserializeObject<T>(value);
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
    public struct MyStruct
    {
        public int X { get; set; }
        public int Y { get; set; }
    }
}
