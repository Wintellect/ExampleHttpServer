using CustomWebServer.Handlers;
using CustomWebServer.Helpers;
using CustomWebServer.Lib;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomWebServer
{
    class Program
    {
        static void Main(string[] args)
        {
            var server = new Server("127.0.0.1", 7777);
            
            var router = new Router();
            router.CreateRoute(@"/Echo", new EchoRequestHandler());
            router.CreateRoute(@"/|(\..+)$", new StaticFileHandler(@"C:\Dev\TestWebSite", "index.html"));
            router.SetDefaultHandler(new FileNotFoundHandler());

            server.StartAsync(router).Wait();
        }
    }

    internal class EchoRequestHandler : IRequestHandler
    {
        public Task<IResponse> HandleRequest(IRequest request)
        {
            var response = new Response(200, new Dictionary<string, object>(), request.AsString());
            return Task.FromResult<IResponse>(response);
        }
    }
}
