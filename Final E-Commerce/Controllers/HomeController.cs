using Final_E_Commerce.DAL;
using Microsoft.AspNetCore.Mvc;
namespace Final_E_Commerce.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;
        public HomeController(AppDbContext context)
        {
            _context = context;
        }
    }
}