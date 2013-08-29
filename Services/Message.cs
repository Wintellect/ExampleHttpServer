using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CustomWebServer.Services
{
    public class Message
    {
        public String MessageText { get; set; }

        public int OrderNum { get; set; }

        public Message(string message, Int32 orderNum)
        {
            OrderNum = orderNum;
            MessageText = message;
        }
    }
}
