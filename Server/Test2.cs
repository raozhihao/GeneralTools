namespace Server
{
    class Test2
    {
        public void Test()
        {
            var server = new GeneralTool.General.TaskLib.SocketServer(null);
            server.RecDataEvent += this.Server_RecDataEvent;
            server.IsReciverForAll = true;
            server.IsAutoSize = true;
            server.InitSocket("127.0.0.1", 8899);
            server.Connect();
        }

        private void Server_RecDataEvent(GeneralTool.General.TaskLib.RecDataObject obj)
        {

        }
    }
}
