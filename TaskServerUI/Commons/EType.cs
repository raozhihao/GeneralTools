using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskServerUI.Commons
{
    public enum EType
    {
        [Test("无")]
        None,
        [Test("一")]
        First,
        [Test("二")]
        Second,
        [Test("三")]
        Third,
    }

    public class TestAttribute : Attribute
    {
        public string Desc { get; set; }
        public TestAttribute(string desc)
        {
            this.Desc = desc;
        }
    }
}
