using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NgChatSignalR.Models
{
    public class MessageViewModel
    {
        public int Type { get; set; }
        public string FromId { get; set; }
        public string ToId { get; set; }
        public string Message { get; set; }
        public DateTime? DateSent { get; set; }
        public DateTime? DateSeen { get; set; }
    }
}
