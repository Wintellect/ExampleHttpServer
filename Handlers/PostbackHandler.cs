using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CustomWebServer.Helpers;
using CustomWebServer.Lib;
using CustomWebServer.Views;

namespace CustomWebServer.Handlers
{
    internal class PostbackHandler : IRequestHandler
    {
        public Task<IResponse> HandleRequest(IRequest request)
        {
            IResponse response;

            if(String.Equals(request.Method, "GET", StringComparison.OrdinalIgnoreCase))
            {
                response = GetRequest(request);
            }
            else if(String.Equals(request.Method, "POST", StringComparison.OrdinalIgnoreCase))
            {
                response = PostRequest(request);
            }
            else
            {
                response = new Response(501, new Dictionary<string, object>());
            }
            
            return Task.FromResult(response);
        }

        private IResponse PostRequest(IRequest request)
        {
            var form = request.GetFormParameters();
            var name = String.IsNullOrWhiteSpace((string) form["myName"]) ? "[Too lazy to enter name]" : form["myName"];
            var pageTemplate = new PostbackHandlerView
                                   {
                                       Model = new
                                                   {
                                                       IsPostBack = true,
                                                       Form = form,
                                                       Name = name
                                                   }
                                   };

            return new Response(200, new Dictionary<String, Object>(), pageTemplate.TransformText());
        }

        private IResponse GetRequest(IRequest request)
        {
            var pageTemplate = new PostbackHandlerView
                                   {
                                       Model = new
                                                   {
                                                       IsPostBack = false,
                                                       Flavors = new[]
                                                                     {
                                                                         "Vanilla",
                                                                         "Chocolate",
                                                                         "Cherry"
                                                                     }
                                                   }
                                   };

            return new Response(200, new Dictionary<String, Object>(), pageTemplate.TransformText());
        }
    }
}