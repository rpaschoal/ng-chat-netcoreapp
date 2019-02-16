using Microsoft.AspNetCore.SignalR;
using NgChatSignalR.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class ChatHub : Hub
{
    private static List<ParticipantResponseViewModel> AllConnectedParticipants { get; set; } = new List<ParticipantResponseViewModel>();
    private static List<ParticipantResponseViewModel> DisconnectedParticipants { get; set; } = new List<ParticipantResponseViewModel>();
    private object ParticipantsConnectionLock = new object();

    public static IEnumerable<ParticipantResponseViewModel> ConnectedParticipants(string currentUserId)
    {
        return AllConnectedParticipants
            .Where(x => x.Participant.Id != currentUserId);
    }

    public void Join(string userName)
    {
        lock (ParticipantsConnectionLock)
        {
            AllConnectedParticipants.Add(new ParticipantResponseViewModel()
            {
                Metadata = new ParticipantMetadataViewModel()
                {
                    TotalUnreadMessages = 0
                },
                Participant = new ChatParticipantViewModel()
                {
                    DisplayName = userName,
                    Id = Context.ConnectionId
                }
            });

            // This will be used as the user's unique ID to be used on ng-chat as the connected user.
            // You should most likely use another ID on your application
            Clients.Caller.SendAsync("generatedUserId", Context.ConnectionId);

            Clients.All.SendAsync("friendsListChanged", AllConnectedParticipants);
        }
    }

    public void SendMessage(MessageViewModel message)
    {
        var sender = AllConnectedParticipants.Find(x => x.Participant.Id == message.FromId);

        if (sender != null)
        {
            Clients.Client(message.ToId).SendAsync("messageReceived", sender.Participant, message);
        }
    }

    public override Task OnDisconnectedAsync(Exception exception)
    {
        lock (ParticipantsConnectionLock)
        {
            var connectionIndex = AllConnectedParticipants.FindIndex(x => x.Participant.Id == Context.ConnectionId);

            if (connectionIndex >= 0)
            {
                var participant = AllConnectedParticipants.ElementAt(connectionIndex);

                AllConnectedParticipants.Remove(participant);
                DisconnectedParticipants.Add(participant);

                Clients.All.SendAsync("friendsListChanged", AllConnectedParticipants);
            }

            return base.OnDisconnectedAsync(exception);
        }
    }

    public override Task OnConnectedAsync()
    {
        lock (ParticipantsConnectionLock)
        {
            var connectionIndex = DisconnectedParticipants.FindIndex(x => x.Participant.Id == Context.ConnectionId);

            if (connectionIndex >= 0)
            {
                var participant = DisconnectedParticipants.ElementAt(connectionIndex);

                DisconnectedParticipants.Remove(participant);
                AllConnectedParticipants.Add(participant);

                Clients.All.SendAsync("friendsListChanged", AllConnectedParticipants);
            }

            return base.OnConnectedAsync();
        }
    }
}
