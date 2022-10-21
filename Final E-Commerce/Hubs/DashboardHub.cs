using Final_E_Commerce.Areas.Admin.Repositories;
using Microsoft.AspNetCore.SignalR;

namespace Final_E_Commerce.Hubs
{
    public class DashboardHub:Hub
    {
        ProductRepository? ProductRepository;

        public DashboardHub(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");  
            ProductRepository = new ProductRepository(connectionString);
        }
        public async Task SendProducts()
        {
            var products = ProductRepository.GetProducts();
            await Clients.All.SendAsync("ReceivedProducts", products);
        }

       
    }
}