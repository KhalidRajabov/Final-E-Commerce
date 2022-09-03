using Final_E_Commerce.DAL;
using Final_E_Commerce.Entities;
using Final_E_Commerce.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Final_E_Commerce.Controllers
{
    public class UserController:Controller
    {
        private readonly AppDbContext _context;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _usermanager;

        public UserController(AppDbContext context,
        SignInManager<AppUser> signInManager,
        UserManager<AppUser> userManager)
        {
            _context = context;
            _signInManager = signInManager;
            _usermanager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            UserVM userVM = new UserVM();
            userVM.User = await _usermanager.FindByNameAsync(User.Identity.Name);
            return View(userVM);
        }
        public async Task<IActionResult> Orders()
        {
            AppUser user = await _usermanager.FindByNameAsync(User.Identity.Name);
            List<Order> order = _context.Orders.Where(o => o.AppUserId == user.Id).OrderByDescending(o => o.Id).ToList();
            OrderVM orderVM = new OrderVM();
            orderVM.Orders = order;
            return View(orderVM);
        }

        public async Task<IActionResult> Detail(int id)
        {

            Order order = await _context.Orders.Where(o => o.Id == id).FirstOrDefaultAsync();
            List<OrderItem> orderItems = await _context.OrderItems.Where(o => o.OrderId == order.Id)
                .Include(p => p.Product).ToListAsync();
            AppUser user = await _usermanager.Users.FirstOrDefaultAsync(i => i.Id == order.AppUserId);
            OrderItemVM orderItemVM = new OrderItemVM();
            orderItemVM.User = user;
            orderItemVM.Order = order;
            orderItemVM.OrderItems = orderItems;
            return View(orderItemVM);
        }
    }
}
