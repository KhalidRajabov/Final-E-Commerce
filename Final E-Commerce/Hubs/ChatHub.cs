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

        public string GetConnectionId() => Context.ConnectionId;
    }
}
