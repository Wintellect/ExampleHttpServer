using CustomWebServer.Helpers;
using CustomWebServer.Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomWebServer.Handlers
{
    public class RedirectHandler: IRequestHandler
    {
        private String RedirectUrl { get; set; }

        public RedirectHandler(String redirectUrl)
        {
            RedirectUrl = redirectUrl;
        }

        public async Task<IResponse> HandleRequest(IRequest request)
        {
            return new Response(
                302,
                "Found",
                new Dictionary<string, object>
                    {
                        {"date", DateTime.UtcNow},
                        {"server", "JWC/1.0 Josh's Awesomesauce Server!!!!"},
                        {"location", request.RequestUri.RebaseTo(RedirectUrl)}
                    });
        }
    }
}
