using CustomWebServer.Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomWebServer.Handlers
{
    public class GenericHandler: IRequestHandler
    {
        private Func<IRequest, IResponse> Handler { get; set; }
 
        public GenericHandler(Func<IRequest, IResponse> handler)
        {
            if(handler == null)
            {
                throw new ArgumentNullException("handler");
            }

            Handler = handler;
        }

        public async Task<IResponse> HandleRequest(IRequest request)
        {
            return Handler(request);
        }
    }
}
