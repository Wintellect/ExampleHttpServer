using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CustomWebServer.Helpers
{
    public static class ETag
    {
        public static string Create(params Object[] inputs)
        {
            var inputString = inputs.Stringify();

            using (var md5Hash = MD5.Create())
            {
                var data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(inputString));
                var sBuilder = new StringBuilder();

                for (var i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }

                return sBuilder.ToString();
            }
        }

        private static string Stringify(this Object[] inputs)
        {
            if(inputs == null || inputs.Length == 0)
            {
                return String.Empty;
            }

            return String.Concat(inputs);
        }
    }
}
