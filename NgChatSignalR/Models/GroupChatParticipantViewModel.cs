using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NgChatSignalR.Models
{
    public class GroupChatParticipantViewModel : ChatParticipantViewModel
    {
        public IList<ChatParticipantViewModel> ChattingTo { get; set; }
    }
}
