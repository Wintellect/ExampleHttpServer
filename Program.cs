using System;
using CustomWebServer.Lib;

namespace CustomWebServer
{
    class Program
    {
        static void Main(string[] args)
        {
            var server = new Server("127.0.0.1", 7777);

            server.StartAsync(request => {

                                  //TODO: Create new response and return here
                                  throw new NotImplementedException();

                              }).Wait();
        }
    }
}
