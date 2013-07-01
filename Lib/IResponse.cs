using System;
using System.Collections.Generic;
using System.Text;

namespace CustomWebServer.Lib
{
    public interface IResponse
    {
        Int32 StatusCode { get; set; }
        IDictionary<String, Object> Headers { get; set; }
        Object Body { get; set; }
    }

    public class Response : IResponse
    {
        public int StatusCode { get; set; }
        public IDictionary<string, object> Headers { get; set; }
        public Object Body { get; set; }

        public Response(Int32 statusCode, IDictionary<String, Object> headers, String body = null)
        {
            StatusCode = statusCode;
            Headers = headers;
            Body = body;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.AppendFormat("HTTP/1.1 {0}{1}", StatusCode, Environment.NewLine);

            AddMissingHeaders();

            foreach (var header in Headers)
            {
                sb.AppendFormat("{0}: {1}{2}", header.Key, header.Value, Environment.NewLine);
            }

            sb.AppendLine();

            if (Body != null)
            {
                sb.Append(Body);
            }

            return sb.ToString();
        }

        private void AddMissingHeaders()
        {
            if(!Headers.ContainsKey("Date"))
            {
                Headers.Add("Date", DateTime.UtcNow);
            }
        }
    }
}