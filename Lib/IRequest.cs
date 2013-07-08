using System;
using System.Collections.Generic;

namespace CustomWebServer.Lib
{
    public interface IRequest
    {
        String Method { get; }
        Uri RequestUri { get; }
        IReadOnlyDictionary<String, Object> Headers { get; }
        String Body { get; }
    }
}
