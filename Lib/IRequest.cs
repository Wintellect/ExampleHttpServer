using System;
using System.Collections.Generic;

namespace CustomWebServer.Lib
{
    public interface IRequest
    {
        String Method { get; }
        Uri RequestUri { get; }
        IReadOnlyDictionary<String, Object> Headers { get; }
        String Body { get; }
    }

    public class Request : IRequest
    {
        public String Method { get; private set; }
        public Uri RequestUri { get; private set; }
        public IReadOnlyDictionary<string, object> Headers { get; private set; }
        public String Body { get; private set; }

        public Request(String rawRequest)
        {
            ParseRequest(rawRequest);
        }

        private void ParseRequest(string rawRequest)
        {
            //TODO: Parse HTTP request here
        }
    }
}
