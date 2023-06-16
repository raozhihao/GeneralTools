﻿using System.Collections.Generic;

namespace GeneralTool.CoreLibrary.Models
{
    /// <summary>
    /// adb坐标系
    /// </summary>
    public struct Element
    {
        /// <summary>
        /// x
        /// </summary>
        public int X
        {
            get;
            set;
        }

        /// <summary>
        /// y
        /// </summary>
        public int Y
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public Dictionary<string, string> Attributes
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cx"></param>
        /// <param name="cy"></param>
        /// <param name="attr"></param>
        public Element(int cx, int cy, Dictionary<string, string> attr)
        {
            X = cx;
            Y = cy;
            Attributes = attr;
        }
    }
}
