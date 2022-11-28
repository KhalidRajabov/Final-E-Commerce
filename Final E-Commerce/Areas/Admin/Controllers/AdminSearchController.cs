using Final_E_Commerce.DAL;
using Final_E_Commerce.Entities;
using Final_E_Commerce.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Final_E_Commerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin, SuperAdmin")]
    public class AdminSearchController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _usermanager;
        private readonly SignInManager<AppUser>? _signInManager;

        public AdminSearchController(AppDbContext context, SignInManager<AppUser>? signInManager, UserManager<AppUser> usermanager)
        {
            _context = context;
            _signInManager = signInManager;
            _usermanager = usermanager;
        }

        public async Task<IActionResult> SearchAll(string search)
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
                    _context?.SaveChangesAsync();
                }
            }
            if (User.Identity.IsAuthenticated)
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
            }

            DetailVM detailVM = new DetailVM();
            if (search!=null)
            {
                detailVM.SearchProducts = _context.Products
                    .Where(p => p.Status == ProductConfirmationStatus.Approved)
                    .Where(p =>
                    p.Name.ToLower().Contains(search.ToLower()) ||
                    p.Description.ToLower().Contains(search.ToLower()) ||
                    p.Body.ToLower().Contains(search.ToLower()) ||
                    p.RearCamera.ToLower().Contains(search.ToLower()) ||
                    p.FrontCamera.ToLower().Contains(search.ToLower()) ||
                    p.Weight.ToLower().Contains(search.ToLower()) ||
                    p.Display.ToLower().Contains(search.ToLower()) ||
                    p.GPU.ToLower().Contains(search.ToLower()) ||
                    p.OperationSystem.ToLower().Contains(search.ToLower()) ||
                    p.Memory.ToLower().Contains(search.ToLower()))
                    .Include(p => p.Category).Include(p => p.ProductImages)
                    .OrderByDescending(p => p.Views)
                    .Take(3).ToList();
            
                detailVM.Users = _usermanager.Users
                    .Where(u=>u.UserName.ToLower().Contains(search.ToLower())||
                    u.Fullname.ToLower().Contains(search.ToLower())||
                    u.Email.ToLower().Contains(search.ToLower()))
                    .Take(5).ToList();
            }
            return PartialView("_AdminSearch", detailVM);
        }
    }
}
