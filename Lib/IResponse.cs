using System;
using System.Collections.Generic;

namespace CustomWebServer.Lib
{
    public interface IResponse
    {
        Int32 StatusCode { get; set; }
        String StatusDescription { get; set; }
        IDictionary<String, Object> Headers { get; }
        Object Body { get; set; }
    }
}