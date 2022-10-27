using Final_E_Commerce.Areas.Admin.Repositories;
using Microsoft.AspNetCore.SignalR;

namespace Final_E_Commerce.Hubs
{
    public class MessagesHub:Hub
    {
        MessageRepository? MessageRepository;

        public MessagesHub(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            MessageRepository = new MessageRepository(connectionString);
        }
        public async Task SendMessages()
        {
            var messages = MessageRepository.GetMessages();
            await Clients.All.SendAsync("ReceivedMessages", messages);
        }

       
    }
}