using CustomWebServer.Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomWebServer.Handlers
{
    public class FileNotFoundHandler : IRequestHandler
    {
        public async Task<IResponse> HandleRequest(IRequest request)
        {
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
        }
    }
}
