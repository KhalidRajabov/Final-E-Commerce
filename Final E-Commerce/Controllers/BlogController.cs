using Final_E_Commerce.DAL;
using Final_E_Commerce.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Final_E_Commerce.Controllers
{
    public class BlogController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        private IConfiguration _config { get; }
        private readonly UserManager<AppUser> _usermanager;
        public BlogController(AppDbContext context, IWebHostEnvironment env, IConfiguration config, UserManager<AppUser> usermanager)
        {
            _context = context;
            _env = env;
            _config = config;
            _usermanager = usermanager;
        }


        public IActionResult Index()
        {
            return View();
        }
    }
}
