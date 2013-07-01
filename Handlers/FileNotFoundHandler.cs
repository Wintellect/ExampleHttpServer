using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CustomWebServer.Lib;

namespace CustomWebServer.Handlers
{
    public class FileNotFoundHandler: IRequestHandler
    {
        private const string DefaultMessage = "Oh Noes! I can't find what you are looking for!";
        
        public async Task<IResponse> HandleRequest(IRequest request)
        {
            return await HandleRequest(request, DefaultMessage);
        }

        public Task<IResponse> HandleRequest(IRequest request, String msg = null)
        {
            return
                Task.FromResult<IResponse>(new Response(404, new Dictionary<string, object>(),
                                                        msg ?? DefaultMessage));
        }
    }
}
