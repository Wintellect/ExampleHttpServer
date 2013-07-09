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

            server.StartAsync(
                new GenericHandler(
                    request => {

                        if (request.RequestUri.LocalPath == "/")
                        {
                            return new PlainTextHandler("Hello World!")
                                .HandleRequest(request);
                        }

                        if (request.RequestUri.LocalPath == "/index.html")
                        {
                            return new RedirectHandler("/")
                                .HandleRequest(request);
                        }

                        return new FileNotFoundHandler()
                            .HandleRequest(request);

                    })).Wait();
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
