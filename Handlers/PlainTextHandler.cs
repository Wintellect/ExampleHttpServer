using CustomWebServer.Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomWebServer.Handlers
{
    public class PlainTextHandler: IRequestHandler
    {
        public string Body { get; set; }

        public PlainTextHandler(String body)
        {
            Body = body;
        }

        public async Task<IResponse> HandleRequest(IRequest request)
        {
            return new Response(
                200,
                "OK",
                new Dictionary<string, object>
                                    {
                                        {"date", DateTime.UtcNow},
                                        {"server", "JWC/1.0 Josh's Awesomesauce Server!!!!"},
                                        {"content-type", "text/plain"},
                                        {"content-length", Body.Length}
                                    },
                Body);
        }
    }
}
