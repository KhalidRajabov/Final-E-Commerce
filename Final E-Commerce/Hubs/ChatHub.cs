using Microsoft.AspNetCore.SignalR;

namespace Final_E_Commerce.Hubs
{
    public class ChatHub:Hub
    {
      /*  public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }*/

        public async Task SendToUser(string user, string receiverConnectionId, string message)
        {
            await Clients.User(receiverConnectionId).SendAsync("ReceiveMessage", user, message);
        }
        public async Task Read(string user, string receiverConnectionId, bool? read=false)
        {
            await Clients.User(receiverConnectionId).SendAsync("MessageRead", user, read);
        }
        public async Task Typing(string user, string receiverConnectionId, bool? typing = false)
        {
            await Clients.User(receiverConnectionId).SendAsync("MessageTyping", user, typing);
        }

        public string GetConnectionId() => Context.ConnectionId;
    }
}
