using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CustomWebServer.Services
{
    public class ChatRequest
    {
        public String Message { get; set; }

        public Int32? HighestOrderRecieved { get; set; }
    }
}
