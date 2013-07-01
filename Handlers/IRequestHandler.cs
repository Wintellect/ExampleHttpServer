using System.Threading.Tasks;
using CustomWebServer.Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CustomWebServer.Handlers
{
    public interface IRequestHandler
    {
        Task<IResponse> HandleRequest(IRequest request);
    }
}
