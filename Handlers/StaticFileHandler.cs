using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CustomWebServer.Lib;
using System.IO;

namespace CustomWebServer.Handlers
{
    public class StaticFileHandler : IRequestHandler
    {
        private readonly DirectoryInfo _rootDirectory;
        private readonly String _defaultFileName;
        private readonly FileInfo _404;
        private readonly IDictionary<String, String> _contentTypes;

        public StaticFileHandler(String rootDirectory, String defaultFile = null)
        {
            _rootDirectory = new DirectoryInfo(rootDirectory);
            _defaultFileName = defaultFile ?? String.Empty;
            _404 = new FileInfo(Path.Combine(_rootDirectory.FullName, "404.html"));
        }

        public async Task<IResponse> HandleRequest(IRequest request)
        {
            var fullPath = CreateFilePath(request);
            var fileInfo = new FileInfo(fullPath);

            if (fileInfo.Exists)
            {
                return new Response(200, "OK", CreateResponseHeaders(fileInfo), fileInfo.OpenRead());
            }

            return new Response(404, "Not Found", CreateResponseHeaders(_404), _404.OpenRead());
        }

        private IDictionary<String, Object> CreateResponseHeaders(FileInfo fileInfo)
        {
            return new Dictionary<String, Object>
                              {
                                  {"content-type", "text/html"},
                                  {"content-length", fileInfo.Length}
                              };
        }

        private string CreateFilePath(IRequest request)
        {
            var rootPath = _rootDirectory.FullName;
            var virtualPath = request.RequestUri.LocalPath;

            if (!rootPath.EndsWith(@"\"))
            {
                rootPath += Path.DirectorySeparatorChar;
            }

            if (virtualPath.StartsWith(@"/"))
            {
                virtualPath = virtualPath.TrimStart(Path.AltDirectorySeparatorChar);
            }

            if (virtualPath == String.Empty)
            {
                virtualPath = _defaultFileName;
            }

            return Path.Combine(rootPath, virtualPath);
        }
    }
}