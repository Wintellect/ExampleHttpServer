using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace CustomWebServer.Lib
{
    public class Server
    {
        private readonly TcpListener _listener;
        
        public Server(String address, Int32 port)
        {
            var ip = IPAddress.Parse(address);

            _listener = new TcpListener(ip, port);
        }

        public async Task StartAsync(Func<IResponse, IResponse> handler)
        {
            _listener.Start();

            while(true)
            {
                using (var client = await _listener.AcceptTcpClientAsync())
                {
                    var request = await ReadRequest(client.GetStream());

                    if (IsRequestValid(request))
                    {
                        var response = await HandleRequest(handler, request);

                        await WriteResponse(client.GetStream(), response);
                    }
                }
            }
        }

        private async Task<IResponse> HandleRequest(Func<IResponse, IResponse> handler, String request)
        {
            throw new NotImplementedException();
        }

        private async Task<String> ReadRequest(NetworkStream stream)
        {
            throw new NotImplementedException();
        }

        private async Task WriteResponse(Stream stream, IResponse response)
        {
            throw new NotImplementedException();
        }

        private static bool IsRequestValid(String request)
        {
            throw new NotImplementedException();
        }
    }
}
