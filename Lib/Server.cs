using System;
using System.Collections.Generic;
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

                    var response = await SafeRequestHandler(handler, request);

                    await WriteResponse(client.GetStream(), response);
                }
            }
        }

        private async Task<IResponse> SafeRequestHandler(IRequestHandler handler, String request)
        {
            IResponse response = null;

            try
            {
                response = await handler.HandleRequest(new Request(request));
            }
            catch(Exception ex)
            {
                response = CreateErrorResponse(ex);
            }

            return response;
        }

        private IResponse CreateErrorResponse(Exception ex)
        {
            var body = ex.ToString();

            return new Response(
                500,
                new Dictionary<string, object>
                    {
                        {"content-type", "text/plain"},
                        {"content-length", body.Length}
                    },
                body);
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

        private async Task WriteResponse(Stream stream, IResponse response)
        {
            var headerBytes = Encoding.ASCII.GetBytes(FormatHeaders(response));

            await stream.WriteAsync(headerBytes, 0, headerBytes.Length);

            if (response.Body == null) return;

            if (response.Body is Stream)
            {
                await WriteBody(stream, response.Body as Stream);
            } 
            else if(response.Body is String)
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

            var i = 0;
            var bytes = new byte[1024];

            using (bodyStream)
            {
                while ((i = await bodyStream.ReadAsync(bytes, 0, bytes.Length)) != 0)
                {
                    await responseStream.WriteAsync(bytes, 0, bytes.Length);
                }
            }
        }

        public String FormatHeaders(IResponse response)
        {
            var sb = new StringBuilder();

            sb.AppendFormat("HTTP/1.1 {0}{1}", response.StatusCode, Environment.NewLine);

            AddMissingHeaders(response);

            foreach (var header in response.Headers)
            {
                sb.AppendFormat("{0}: {1}{2}", header.Key, header.Value, Environment.NewLine);
            }

            sb.AppendLine();

            return sb.ToString();
        }

        private void AddMissingHeaders(IResponse response)
        {
            if (!response.Headers.ContainsKey("Date"))
            {
                response.Headers.Add("Date", DateTime.UtcNow);
            }
        }
    }
}
