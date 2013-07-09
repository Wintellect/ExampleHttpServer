using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using CustomWebServer.Handlers;
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

        public async Task StartAsync(IRequestHandler handler)
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

        private async Task<IResponse> HandleRequest(IRequestHandler handler, String request)
        {
            IResponse response;

            try
            {
                response = await handler.HandleRequest(new Request(request));
            }
            catch(Exception ex)
            {
                var errorHandler = new InternalServerErrorHandler(ex);
                response = errorHandler.HandleRequest(new Request(request)).Result;
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
            
            if (response.Body is Stream)
            {
                await WriteBody(stream, response.Body as Stream);
            }
            else if (response.Body is String)
            {
                await WriteBody(stream, response.Body as String);
            }
        }

        private async Task WriteBody(Stream responseStream, String body)
        {
            var bodyBytes = Encoding.UTF8.GetBytes(body);
            
            await responseStream.WriteAsync(bodyBytes, 0, bodyBytes.Length);
        }

        private async Task WriteBody(Stream responseStream, Stream bodyStream)
        {
            if (bodyStream == null) return;

            var bytes = new byte[1024];

            using (bodyStream)
            {
                var i = 0;
                while ((i = await bodyStream.ReadAsync(bytes, 0, bytes.Length)) != 0)
                {
                    await responseStream.WriteAsync(bytes, 0, i);
                }
            }
        }
        
        public String FormatHeaders(IResponse response)
        {
            var sb = new StringBuilder();

            sb.AppendFormat("HTTP/1.1 {0} {1}{2}", response.StatusCode, response.StatusDescription, Environment.NewLine);

            AddMissingHeaders(response);

            foreach (var header in response.Headers)
            {
                sb.AppendLine(header.FormatAsHeader());
            }

            sb.AppendLine();

            return sb.ToString();
        }

        private void AddMissingHeaders(IResponse response)
        {
            response.AddHeaderIfMissing("date", DateTime.UtcNow);
            response.AddHeaderIfMissing("connection", "close");

            var body = response.Body as string;
            if (body != null)
            {
                response.AddHeaderIfMissing("content-length", body.Length);
            }
        }

        private static bool IsRequestValid(String request)
        {
            return !String.IsNullOrEmpty(request);
        }
    }
}
