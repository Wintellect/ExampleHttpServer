using System;
using System.Collections.Generic;
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
