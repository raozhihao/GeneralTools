using System;

namespace ClassLibrary
{
    public interface ISocketClass
    {
        void Log(string msg);

        event Action EventTest;
    }
}