using GeneralTool.General;
using System;

namespace IOCTest
{
    class Student
    {
        public int Id { get; set; } = RandomEx.Next(1, 10);
        public String Name { get; set; } = Guid.NewGuid().ToString();
    }
}
