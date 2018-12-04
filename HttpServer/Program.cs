using System;

namespace HttpServer
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0 || !int.TryParse(args[0], out int port))
            {
                Console.WriteLine("请先输入端口号");
                return;
            }

            GServer server = new GServer(port);
            server.Start();
        }
    }
}
