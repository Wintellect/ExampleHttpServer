using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace CustomWebServer.Helpers
{
    public static class StringExtensions
    {
        public static string SafeTrim(this String str)
        {
            return str == null ? null : str.Trim();
        }

        public static string Unquote(this String str)
        {
            return str == null ? null : str.Trim('"');
        }

        public static string UrlDecode(this String str)
        {
            return str == null ? null : HttpUtility.UrlDecode(str);
        }
    }
}