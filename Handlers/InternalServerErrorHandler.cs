using CustomWebServer.Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomWebServer.Handlers
{
    public class InternalServerErrorHandler: IRequestHandler
    {
        private Exception Ex { get; set; }

        public InternalServerErrorHandler(Exception ex)
        {
            Ex = ex;
        }

        public async Task<IResponse> HandleRequest(IRequest request)
        {
            var body = Ex.ToString();

            return new Response(
                500,
                "Internal Server Error",
                new Dictionary<String, Object>
                    {
                        {"content-type", "text/plain"},
                        {"content-length", body.Length}
                    },
                body);
        }
    }
}
