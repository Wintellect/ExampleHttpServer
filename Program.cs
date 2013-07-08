using System;
using CustomWebServer.Lib;

namespace CustomWebServer
{
    class Program
    {
        static void Main(string[] args)
        {
            var port = GetPort(args);
            var server = new Server("127.0.0.1", port);

            server.StartAsync(request => {

                                  //TODO: Create new response and return here
                                  throw new NotImplementedException();

                              }).Wait();
        }

        private static Int32 GetPort(string[] args)
        {
            var port = 7777;

            if(args.Length > 0)
            {
                Int32.TryParse(args[0], out port);
            }

            return port;
        }
    }
}
