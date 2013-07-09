using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using CustomWebServer.Helpers;

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

        public async Task StartAsync(Func<IRequest, IResponse> handler)
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

        private async Task<IResponse> HandleRequest(Func<IRequest, IResponse> handler, String request)
        {
            IResponse response;

            try
            {
                response = handler(new Request(request));
            }
            catch(Exception ex)
            {
                response = new Response(500, "Internal Server Error", null, ex);
            }

            return response;
        }

        private async Task<String> ReadRequest(NetworkStream stream)
        {
            var encoding = Encoding.UTF8;
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

        private async Task WriteResponse(Stream stream, IResponse response)
        {
            var headerBytes = Encoding.ASCII.GetBytes(FormatHeaders(response));

            await stream.WriteAsync(headerBytes, 0, headerBytes.Length);

            if (response.Body == null) return;

            var bodyBytes = Encoding.UTF8.GetBytes(response.Body.ToString());

            await stream.WriteAsync(bodyBytes, 0, bodyBytes.Length);
        }

        public String FormatHeaders(IResponse response)
        {
            var sb = new StringBuilder();

            sb.AppendFormat("HTTP/1.1 {0} {1}{2}", response.StatusCode, response.StatusDescription, Environment.NewLine);

            foreach (var header in response.Headers)
            {
                sb.AppendFormat("{0}: {1}{2}", header.Key, header.Value, Environment.NewLine);
            }

            sb.AppendLine();

            return sb.ToString();
        }

        private static bool IsRequestValid(String request)
        {
            return !String.IsNullOrEmpty(request);
        }
    }
}
