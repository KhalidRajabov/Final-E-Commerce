using Final_E_Commerce.Areas.Admin.Repositories;
using Microsoft.AspNetCore.SignalR;
using NuGet.Protocol.Core.Types;

namespace Final_E_Commerce.Hubs
{
    public class PendingsHub : Hub
    {
        PendingsRepository? PendingsRepository;

        public PendingsHub(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            PendingsRepository = new PendingsRepository(connectionString);
        }
        public async Task SendProducts()
        {
            var products = PendingsRepository.GetProducts();
            await Clients.All.SendAsync("ReceivedProducts", products);
        }
    }
}