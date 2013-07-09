using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomWebServer.Helpers
{
    public static class UriHelpers
    {
        public static Uri RebaseTo(this Uri uri, String relativeUrl)
        {
            if (uri.IsAbsoluteUri)
            {
                return new Uri(uri, relativeUrl);
            }

            return new Uri(relativeUrl);
        }
    }
}
