using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using CustomWebServer.Handlers;

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

        public async Task StartAsync(IRequestHandler handler)
        {
            _listener.Start();

            while(true)
            {
                using (var client = await _listener.AcceptTcpClientAsync())
                {
                    var request = await ReadRequest(client.GetStream());

                    var response = await handler.HandleRequest(new Request(request));

                    await WriteResponse(client.GetStream(), response.ToString(), response.Encoding);
                    
                    client.Close();
                }
            }
        }

        private async Task<String> ReadRequest(NetworkStream stream, Encoding encoding = null)
        {
            if (encoding == null) encoding = Encoding.UTF8;

            var i = 0;
            var bytes = new byte[1024];
            var sb = new StringBuilder();

            while ((i = await stream.ReadAsync(bytes, 0, bytes.Length)) != 0)
            {
                sb.Append(encoding.GetString(bytes, 0, i));

                if (!stream.DataAvailable) break;
            }

            return sb.ToString();
        }

        private async Task WriteResponse(Stream stream, string responseString, Encoding encoding = null)
        {
            if (encoding == null) encoding = Encoding.UTF8;

            var bytes = encoding.GetBytes(responseString);

            await stream.WriteAsync(bytes, 0, bytes.Length);
        }
    }
}
