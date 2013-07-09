using System.Threading.Tasks;
using CustomWebServer.Helpers;
using CustomWebServer.Lib;

namespace CustomWebServer.Handlers
{
    internal class EchoRequestHandler : IRequestHandler
    {
        public Task<IResponse> HandleRequest(IRequest request)
        {
            var response = new Response(200, "OK", null, request.AsString());
            return Task.FromResult<IResponse>(response);
        }
    }
}