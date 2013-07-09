using System.Threading.Tasks;

namespace CustomWebServer.Lib
{
    public interface IRequestHandler
    {
        Task<IResponse> HandleRequest(IRequest request);
    }
}