using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomWebServer.Helpers
{
    public static class HeaderValueExtensions
    {
        private static readonly IDictionary<String, Func<Object, Object>> Formatters =
            new Dictionary<string, Func<object, object>>(StringComparer.OrdinalIgnoreCase)
                {
                    {"date", DateTimeFormatter},
                    {"expires", DateTimeFormatter},
                    {"cache-control", CacheControlFormatter},
                    {"last-modified", DateTimeFormatter},
                    {"etag", QuottedStringFormatter}
                };

        public static string FormatAsHeader(this KeyValuePair<String, Object> header)
        {
            var formatter = GetFormatter(header.Key);

            return String.Format("{0}: {1}", header.Key, formatter(header.Value));
        }

        private static Func<Object, Object> GetFormatter(String key)
        {
            if (Formatters.ContainsKey(key))
            {
                return Formatters[key];
            }

            return o => o;
        }

        private static object QuottedStringFormatter(object o)
        {
            const String quote = "\"";
            var strVal = String.Format("{0}", o);

            if (!strVal.StartsWith(quote))
            {
                strVal = quote + strVal;
            }

            if (!strVal.EndsWith(quote))
            {
                strVal += quote;
            }

            return strVal;
        }

        private static object CacheControlFormatter(object val)
        {
            var type = val.GetType();

            if (type == typeof(TimeSpan))
            {
                return String.Format("max-age={0}", ((TimeSpan)val).TotalSeconds);
            }

            return val;
        }

        private static object DateTimeFormatter(object dateTimeValue)
        {
            var dt = dateTimeValue as DateTime?;

            return dt == null ? dateTimeValue : String.Format("{0:R}", dt);
        }
    }
}