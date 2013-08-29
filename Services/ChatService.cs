using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomWebServer.Services
{
    public class ChatService
    {
        private static readonly ConcurrentBag<Message> _chats =
            new ConcurrentBag<Message>();
        
        public IEnumerable<Message> Post(ChatRequest request)
        {
            if(request == null)
            {
                request = new ChatRequest
                              {
                                  HighestOrderRecieved = null,
                                  Message = null
                              };
            }

            if(request.Message != null)
            {
                var order = GetOrderNumber();
                _chats.Add(new Message(request.Message, order));
            }

            return GetLatestChats(request.HighestOrderRecieved ?? 0);
        }

        private static int GetOrderNumber()
        {
            if(_chats.IsEmpty)
            {
                return 1;
            }

            return _chats.Max(m => m.OrderNum) + 1;
        }

        private IEnumerable<Message> GetLatestChats(Int32 highestRecievedOrder)
        {
            return
                _chats.Where(m => m.OrderNum > highestRecievedOrder).OrderBy(m => m.OrderNum).ToList();
        }
    }
}
