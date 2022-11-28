using Final_E_Commerce.DAL;
using Final_E_Commerce.Entities;
using Final_E_Commerce.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Final_E_Commerce.Helper;

namespace Final_E_Commerce.Controllers
{
    [Authorize]
    public class WishlistController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _usermanager;
        private readonly SignInManager<AppUser>? _signInManager;

        public WishlistController(UserManager<AppUser> usermanager, AppDbContext context, SignInManager<AppUser>? signInManager)
        {
            _usermanager = usermanager;
            _context = context;
            _signInManager = signInManager;
        }

        public async Task<IActionResult> Index()
        {
            AppUser AppUser = await _usermanager.FindByNameAsync(User.Identity.Name);
            var userroles = await _usermanager.GetRolesAsync(AppUser);
            foreach (var item in userroles)
            {
                if (item.ToLower() == "ban" || userroles == null)
                {
                    await _signInManager.SignOutAsync();
                    return RedirectToAction("error", "home");
                }
            }
            List<Products>? AllProducts = await _context?.Products?
                   .Where(p => p.DiscountPercent > 0).ToListAsync();
            foreach (var item in AllProducts)
            {
                if (item.DiscountUntil < DateTime.Now.AddHours(12))
                {
                    item.DiscountUntil = null;
                    item.DiscountPercent = 0;
                    item.DiscountPrice = 0;
                    await _context.SaveChangesAsync();
                }
            }
            AppUser user = await _usermanager.FindByNameAsync(User.Identity.Name);
            WishlistVM wishlistVM = new WishlistVM();
            List<Wishlist>? wihlist= _context?.Wishlists?.Where(w => w.AppUserId == user.Id).ToList();
            List<Products> listproduct = new List<Products>();
            foreach (var item in wihlist)
            {
                Products? product = await _context?.Products?.Include(p=>p.ProductImages).FirstOrDefaultAsync(p => p.Id == item.ProductId);
                listproduct.Add(product);
            }
            wishlistVM.Products = listproduct;
            return View(wishlistVM);
        }

        public async Task<IActionResult> Add(int id)
        {

            List<Products>? AllProducts = await _context?.Products?
               .Where(p => p.DiscountPercent > 0).ToListAsync();
            foreach (var item in AllProducts)
            {
                if (item.DiscountUntil < DateTime.Now.AddHours(12))
                {
                    item.DiscountUntil = null;
                    item.DiscountPercent = 0;
                    item.DiscountPrice = 0;
                    await _context.SaveChangesAsync();
                }
            }
            AppUser AppUser = await _usermanager.FindByNameAsync(User.Identity.Name);
            var userroles = await _usermanager.GetRolesAsync(AppUser);
            foreach (var item in userroles)
            {
                if (item.ToLower() == "ban" || userroles == null)
                {
                    await _signInManager.SignOutAsync();
                    return RedirectToAction("error", "home");
                }
            }
            bool online = false;
            if (User.Identity.IsAuthenticated)
            {
                online = true;
            }
            AppUser user = await _usermanager.FindByNameAsync(User.Identity.Name);
            Products? product = await _context?.Products?.FirstOrDefaultAsync(p => p.Id == id);
            Wishlist wishlist = new Wishlist();
            wishlist.ProductId = product.Id;
            wishlist.AppUserId = user.Id;
            bool IsExist = await _context?.Wishlists?.Where(w=>w.AppUserId == user.Id&&w.ProductId==id).AnyAsync();
            var obj = new
            {
                exist = true,
                online = online
            };
            if (IsExist)
            {
                return Ok(obj);
            }
            var newobj = new
            {
                exist = false,
                online = online
            };
            await _context.AddAsync(wishlist);
            await _context.SaveChangesAsync();

            return Ok(newobj);
        }

        public async Task<IActionResult> Remove(int id)
        {

            List<Products>? AllProducts = await _context?.Products?
               .Where(p => p.DiscountPercent > 0).ToListAsync();
            foreach (var item in AllProducts)
            {
                if (item.DiscountUntil < DateTime.Now.AddHours(12))
                {
                    item.DiscountUntil = null;
                    item.DiscountPercent = 0;
                    item.DiscountPrice = 0;
                    await _context.SaveChangesAsync();
                }
            }
            AppUser AppUser = await _usermanager.FindByNameAsync(User.Identity.Name);
            var userroles = await _usermanager.GetRolesAsync(AppUser);
            foreach (var item in userroles)
            {
                if (item.ToLower() == "ban" || userroles == null)
                {
                    await _signInManager.SignOutAsync();
                    return RedirectToAction("error", "home");
                }
            }
            AppUser user = await _usermanager.FindByNameAsync(User.Identity.Name);
            Products? product = await _context?.Products?.FirstOrDefaultAsync(p => p.Id == id);
            Wishlist? IsExist = await _context?.Wishlists?.Where(w => w.AppUserId == user.Id && w.ProductId == id).FirstOrDefaultAsync();
            if (IsExist==null)
            {
                return Ok($"{product.Name} doesn't exist in your wishlist");
            }
            _context.Wishlists.Remove(IsExist);
            await _context.SaveChangesAsync();
            return Ok($"{product.Name} deleted from your wishlist");
        }
    }
}
