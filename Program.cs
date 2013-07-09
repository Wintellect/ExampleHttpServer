using System;
using System.Collections.Generic;
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

            server.StartAsync(request => {

                if (request.RequestUri.LocalPath == "/")
                {
                    var body = "Hello World!";

                    return new Response(
                    200,
                    "OK",
                    new Dictionary<string, object>
                        {
                        {"date", DateTime.UtcNow},
                        {"server", "JWC/1.0 Josh's Awesomesauce Server!!!!"},
                        {"content-type", "text/plain"},
                        {"content-length", body.Length}
                        },
                    body);
                }

                if(request.RequestUri.LocalPath == "/index.html")
                {
                    return new Response(
                    302,
                    "Found",
                    new Dictionary<string, object>
                        {
                        {"date", DateTime.UtcNow},
                        {"server", "JWC/1.0 Josh's Awesomesauce Server!!!!"},
                        {"location", request.RequestUri.RebaseTo("/")}
                        });
                }

                var body404 = "<h1>No soup for you!</h1>";

                                  return new Response(
                                  404, "Not Found",
                                  new Dictionary<string, object>
                                      {
                                      {"date", DateTime.UtcNow},
                                      {"server", "JWC/1.0 Josh's Awesomesauce Server!!!!"},
                                      {"content-type", "text/html"},
                                      {"content-length", body404.Length}
                                      },
                                  body404);

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
