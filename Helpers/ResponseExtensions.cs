using System;
using CustomWebServer.Lib;

namespace CustomWebServer.Helpers
{
    public static class ResponseExtensions
    {
        public static void AddHeaderIfMissing(this IResponse response, String headerName, Object headerVal)
        {
            if (!response.Headers.ContainsKey(headerName))
            {
                response.Headers.Add(headerName, headerVal);
            }
        }
    }
}