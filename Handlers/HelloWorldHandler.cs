using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CustomWebServer.Lib;

namespace CustomWebServer.Handlers
{
    public class HelloWorldHandler : IRequestHandler
    {
        public Task<IResponse> HandleRequest(IRequest request)
        {
            var body = "Hello, World!";

            return Task.FromResult<IResponse>(new Response(200, new Dictionary<string, object>
                                                                    {
                                                                        {"Date", DateTime.UtcNow},
                                                                        {"Content-Type", "text/html"},
                                                                        {"Content-Length", body.Length},
                                                                        {"Connection", "close"}
                                                                    },
                                                           body));
        }
    }
}
