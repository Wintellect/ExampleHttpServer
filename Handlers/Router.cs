using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CustomWebServer.Lib;
using System.Text.RegularExpressions;

namespace CustomWebServer.Handlers
{
    public class Router : IRequestHandler
    {
        private const RegexOptions Options =
            RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase;

        private readonly List<Route> _routes = new List<Route>();

        private IRequestHandler _defaultHandler; 

        public void CreateRoute(String pattern, IRequestHandler requestHandler)
        {
            if(String.IsNullOrEmpty(pattern))
            {
                throw new ArgumentException("Pattern cannot be null or empty");
            }

            if(requestHandler == null)
            {
                throw new ArgumentNullException("requestHandler");
            }

            _routes.Add(new Route
                                   {
                                       Pattern = new Regex(pattern, Options),
                                       Handler = requestHandler
                                   });
        }

        public void SetDefaultHandler(IRequestHandler handler)
        {
            _defaultHandler = handler;
        }

        public async Task<IResponse> HandleRequest(IRequest req)
        {
            if (req.RequestUri != null)
            {
                foreach (var route in _routes.Where(route => route.Pattern.IsMatch(req.RequestUri.OriginalString)))
                {
                    return await route.Handler.HandleRequest(req);
                }
            }

            return await _defaultHandler.HandleRequest(req);
        }

        private class Route
        {
            internal Regex Pattern { get; set; }
            internal IRequestHandler Handler { get; set; }
        }
    }
}
