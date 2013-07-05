using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using CustomWebServer.Helpers;

namespace CustomWebServer.Lib
{
    public interface IRequest
    {
        string Method { get; }
        Uri RequestUri { get; }
        IReadOnlyDictionary<String, Object> Headers { get; }
        String Body { get; }
    }

    public class Request : IRequest
    {
        public string Method { get; private set; }
        public Uri RequestUri { get; private set; }
        public IReadOnlyDictionary<string, object> Headers { get; private set; }
        public string Body { get; private set; }

        private string RawRequest { get; set; }
        private string RawRequestUri { get; set; }
        private DateTime TimeStamp { get; set; }

        public Request(string rawRequest)
        {
            RawRequest = rawRequest;
            TimeStamp = DateTime.UtcNow;

            ParseRequest(RawRequest);
        }

        private void ParseRequest(string rawRequest)
        {
            if (String.IsNullOrEmpty(rawRequest)) return;

            var lines = rawRequest.Split(new[] {Environment.NewLine}, StringSplitOptions.None)
                .ToList();

            var requestLine = lines[0];

            lines.RemoveAt(0);

            ParseRequestLine(requestLine);

            var headers = lines.TakeWhile(l => l != String.Empty).ToList();

            lines.RemoveRange(0, headers.Count);

            ParseHeaderLines(headers);

            ConstructUri(Headers, RawRequestUri);

            if (lines.Count > 0)
            {
                Body = String.Join(Environment.NewLine, lines);
            }
        }

        private void ConstructUri(IReadOnlyDictionary<string, object> headers, String rawUri)
        {
            var requestUri = new Uri(rawUri, UriKind.RelativeOrAbsolute);

            if (requestUri.IsAbsoluteUri)
            {
                RequestUri = requestUri;
            }
            else
            {
                Object host;
                if (headers.TryGetValue("host", out host))
                {
                    var baseUri = new Uri("http://" + (String)host);
                    requestUri = new Uri(baseUri, requestUri);
                }
            }

            RequestUri = requestUri;
        }

        private void ParseRequestLine(string requestLine)
        {
            var parts = Regex.Split(requestLine, @"\s");
            Method = parts[0];
            RawRequestUri = parts[1];
        }

        private void ParseHeaderLines(IEnumerable<string> headers)
        {
            var parsedHeaders = headers.Select(SplitHeader)
                .Where(t => t != null)
                .ToDictionary(t => t.Item1, t => t.Item2, StringComparer.OrdinalIgnoreCase);

            Headers = new ReadOnlyDictionary<String, Object>(parsedHeaders);
        }

        private Tuple<String, Object> SplitHeader(string headerLine)
        {
            if(String.IsNullOrEmpty(headerLine))
            {
                return null;
            }

            var parts = headerLine.Split(new[] {':'}, 2);

            return new Tuple<String, Object>(parts[0].SafeTrim(), parts[1].SafeTrim());
        }

        public override string ToString()
        {
            return String.Format("Request Time: {0}{1}{2}", TimeStamp, Environment.NewLine, RawRequest);
        }
    }
}
