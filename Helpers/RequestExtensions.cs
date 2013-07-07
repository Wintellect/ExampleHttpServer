using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CustomWebServer.Lib;

namespace CustomWebServer.Helpers
{
    public static class RequestExtensions
    {
        public static String AsString(this IRequest request)
        {
            var sb = new StringBuilder();

            sb.AppendFormat("{0} {1} HTTP/1.1", request.Method, request.RequestUri.PathAndQuery);
            sb.AppendLine();
            
            foreach (var header in request.Headers)
            {
                sb.AppendFormat("{0}: {1}", header.Key, header.Value);
                sb.AppendLine();
            }

            sb.AppendLine();
            sb.Append(request.Body);

            return sb.ToString();
        }

        public static bool IsExpired(this IRequest response, DateTime lastModified, String etag = null)
        {
            if(response.Headers.ContainsKey("cache-control") && (string) response.Headers["cache-control"] == "no-cache")
            {
                return true;
            }

            if(etag != null && response.Headers.ContainsKey("if-none-match"))
            {
                return etag != ((string) response.Headers["if-none-match"]).Unquote();
            }

            if(response.Headers.ContainsKey("if-modified-since"))
            {
                DateTime ifModifiedSince;

                if(DateTime.TryParse((string) response.Headers["if-modified-since"], out ifModifiedSince))
                {
                    return lastModified.TrimMilliseconds() > ifModifiedSince.TrimMilliseconds();
                }
            }

            return true;
        }

        public static IDictionary<String, Object> GetFormParameters(this IRequest request)
        {
            var formParams = new Dictionary<String, Object>(StringComparer.OrdinalIgnoreCase);

            if(request.Headers.ContainsKey("content-type") && Equals(request.Headers["content-type"], "application/x-www-form-urlencoded"))
            {
                var @params = from param in request.Body.Split('&')
                              let kvp = param.Split('=')
                              where kvp != null && kvp.Length > 0
                              select CreateFormParameter(kvp);

                foreach (var kvp in @params)
                {
                    if (formParams.ContainsKey(kvp.Key))
                    {
                        formParams[kvp.Key] += "," + kvp.Value;
                    }
                    else
                    {
                        formParams.Add(kvp.Key, kvp.Value);
                    }
                }
            }

            return formParams;
        }

        private static KeyValuePair<string, object> CreateFormParameter(IList<String> kvp)
        {
            return kvp.Count > 1
                       ? new KeyValuePair<String, Object>(kvp[0].SafeTrim(), kvp[1].UrlDecode())
                       : new KeyValuePair<String, Object>(kvp[0].SafeTrim(), null);
        }
    }
}