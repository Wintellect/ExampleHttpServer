using CustomWebServer.Lib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomWebServer.Helpers
{
    public static class StringExtensions
    {
        public static string SafeTrim(this String str)
        {
            return str == null ? null : str.Trim();
        }
    }

    public static class RequestExtensions
    {
        public static string AsString(this IRequest request)
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
    }
}
