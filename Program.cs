using System;
using System.Collections.Generic;
using CustomWebServer.Handlers;
using CustomWebServer.Helpers;
using CustomWebServer.Lib;

namespace CustomWebServer
{
    class Program
    {
        static void Main(string[] args)
        {
            var port = GetPort(args);
            var server = new Server("127.0.0.1", port);

            var router = new RoutingHandler();
            router.CreateRoute(@"/|(\..+)$", new StaticFileHandler(@"C:\Dev\CustomWebServer\TestWebSite", "index.html"));
            router.SetDefaultHandler(new FileNotFoundHandler());

            server.StartAsync(router).Wait();
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
