using Final_E_Commerce.DAL;
using Final_E_Commerce.Entities;
using Final_E_Commerce.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Final_E_Commerce.Controllers
{
    public class WishlistController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _usermanager;

        public WishlistController(UserManager<AppUser> usermanager, AppDbContext context)
        {
            _usermanager = usermanager;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            AppUser user = await _usermanager.FindByNameAsync(User.Identity.Name);
            WishlistVM wishlistVM = new WishlistVM();
            wishlistVM.Wishlist = _context.Wishlists.Where(w => w.AppUserId == user.Id).ToList();
            return View(wishlistVM);
        }
        public async Task<IActionResult> Add(int id)
        {
            AppUser user = await _usermanager.FindByNameAsync(User.Identity.Name);
            Product product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
            Wishlist wishlist = new Wishlist();
            wishlist.ProductId = product.Id;
            wishlist.AppUserId = user.Id;
            bool IsExist = _context.Wishlists.Where(w=>w.AppUserId == user.Id&&w.ProductId==id).Any();
            if (IsExist)
            {
                return View();
            }
            await _context.AddAsync(wishlist);
            await _context.SaveChangesAsync();
            return Ok($"{product.Name} added");
        }
    }
}
