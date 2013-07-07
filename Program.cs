using CustomWebServer.Handlers;
using CustomWebServer.Lib;
using System.Linq;
using System.Text;

namespace CustomWebServer
{
    class Program
    {
        static void Main(string[] args)
        {
            var server = new Server("127.0.0.1", 7777);
            
            var router = new Router();
            router.CreateRoute(@"/Echo", new EchoRequestHandler());
            router.CreateRoute(@"/PostbackHandler", new PostbackHandler());
            router.CreateRoute(@"/|(\..+)$", new StaticFileHandler(@"C:\Dev\TestWebSite", "index.html"));
            router.SetDefaultHandler(new FileNotFoundHandler());

            server.StartAsync(router).Wait();
        }
    }
}
