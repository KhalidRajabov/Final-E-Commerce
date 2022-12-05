using Final_E_Commerce.Entities;
using Microsoft.AspNetCore.SignalR;

namespace Final_E_Commerce.Hubs
{
    public class ChatHub:Hub
    {
        public async Task SendMessage(ChatMessage message)
        {
            await Clients.All.SendAsync("receiveMessage", message);
        }
    }
}
