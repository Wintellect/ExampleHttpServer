using System;
using System.Collections.Generic;
using System.Linq;

namespace CustomWebServer.Lib
{
    public interface IResponse
    {
        Int32 StatusCode { get; set; }
        String StatusDescription { get; set; }
        IDictionary<String, Object> Headers { get; }
        Object Body { get; set; }
    }

    public class Response : IResponse
    {
        public Int32 StatusCode { get; set; }
        public String StatusDescription { get; set; }
        public IDictionary<string, object> Headers { get; private set; }
        public Object Body { get; set; }

        public Response(Int32 status, String description, IEnumerable<KeyValuePair<String, Object>> headers, Object body = null)
        {
            StatusCode = status;
            StatusDescription = description;
            Headers = GetHeaders(headers ?? Enumerable.Empty<KeyValuePair<String, Object>>());
            Body = body;
        }

        private IDictionary<String, Object> GetHeaders(IEnumerable<KeyValuePair<String, Object>> headers)
        {
            var headerDict = new Dictionary<String, Object>(StringComparer.OrdinalIgnoreCase);

            foreach (var header in headers)
            {
                if(headerDict.ContainsKey(header.Key))
                {
                    headerDict[header.Key] = header.Value;
                }
                else
                {
                    headerDict.Add(header.Key, header.Value);
                }
            }

            return headerDict;
        }
    }
}