using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CustomWebServer.Helpers;
using CustomWebServer.Lib;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace CustomWebServer.Handlers
{
    public class RestHandler<TService> : IRequestHandler
    {
        public async Task<IResponse> HandleRequest(IRequest request)
        {
            if(!IsRequestValid(request))
            {
                return new Response(415, "Unsupported Media Type", body: "The RestHandler<T> only supports JSON");
            }

            var service = CreateService();

            var method = GetMethod(request, service);

            if(method == null)
            {
                return new Response(501, "Not Implemented",
                                    body:
                                        String.Format("Unable to locate the \"{0}\" method on {1}", request.Method,
                                                      typeof (TService).Name));
            }

            var methodParams = method.GetParameters();
            var inputParams = new List<Object>();

            if(methodParams.Length > 0 && request.HasBody())
            {
                inputParams.Add(JsonConvert.DeserializeObject(request.Body, methodParams[0].ParameterType));
            }

            var result = method.Invoke(service, inputParams.ToArray());

            return new Response(200, "OK", new Dictionary<string, object>
                                               {
                                                   {"content-type", "application/json"}
                                               },
                                JsonConvert.SerializeObject(result, DefaultSettings()));
        }

        private JsonSerializerSettings DefaultSettings()
        {
            var settings = new JsonSerializerSettings {ContractResolver = new CamelCasePropertyNamesContractResolver()};
            return settings;
        }

        private MethodInfo GetMethod(IRequest request, TService service)
        {
            return (from m in typeof (TService).GetMethods()
                    where String.Equals(m.Name, request.Method, StringComparison.OrdinalIgnoreCase)
                    select m).FirstOrDefault();
        }

        private bool IsRequestValid(IRequest request)
        {
            return String.Equals(request.Method, "GET", StringComparison.OrdinalIgnoreCase) || (
                   request.Headers.ContainsKey("content-type") &&
                   ((String)request.Headers["content-type"]).StartsWith("application/json",
                                 StringComparison.OrdinalIgnoreCase));
        }

        private TService CreateService()
        {
            return Activator.CreateInstance<TService>();
        }
    }
}
