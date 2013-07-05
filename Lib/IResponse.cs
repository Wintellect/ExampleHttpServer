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

        public Response(Int32 statusCode, IDictionary<String, Object> headers, Object body = null)
        {
            StatusCode = statusCode;
            Headers = headers;
            Body = body;
        }
    }
}