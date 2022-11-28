using Final_E_Commerce.DAL;
using Final_E_Commerce.Entities;
using Final_E_Commerce.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Final_E_Commerce.Controllers
{
    public class SearchController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _usermanager;
        private readonly SignInManager<AppUser>? _signInManager;

        public SearchController(AppDbContext context, UserManager<AppUser> usermanager, SignInManager<AppUser>? signInManager)
        {
            _context = context;
            _usermanager = usermanager;
            _signInManager = signInManager;
        }

        public async Task<IActionResult> Index()
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
            return View();
        }

        public async Task<IActionResult> SearchProduct(string search)
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
            List<Products> products = _context.Products
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
                .Take(10).ToList();
            if (products == null)
            {
                return RedirectToAction("error", "home");
            }
            List<Blogs>? blogs = _context?.Blogs?
                .Where(b=>b.Title.ToLower().Contains(search.ToLower())||
                b.Content.Contains(search.ToLower())).Take(5).ToList();
            DetailVM detailVM = new DetailVM();
            detailVM.SearchProducts = products;
            detailVM.Blogs=blogs;

            return PartialView("_Search", detailVM);
        }

        public async Task<IActionResult> PopularProducts()
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
            List<Products> PopularProducts = _context.Products
                .Where(p => p.Status == ProductConfirmationStatus.Approved)
                .OrderByDescending(p => p.Views).Take(6).Include(p=>p.ProductImages).ToList();
            DetailVM detailVM = new DetailVM();
            detailVM.SearchProducts = PopularProducts;
            return PartialView("_Popular", detailVM);       
        }
        public async Task<IActionResult> GetProductForModal(int id)
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
            Products? product = await _context?.Products?
                .Where(p => p.Status == ProductConfirmationStatus.Approved)
                .Include(p=>p.ProductImages).FirstOrDefaultAsync(p => p.Id == id);
            DetailVM? detailVM = new DetailVM();
            string? image = "";
            foreach (var item in product.ProductImages)
            {
                if (item.IsMain)
                {
                    image = item.ImageUrl;
                    break;
                }
            }
            detailVM.Product = product;
            var obj = new
            {
                image = image,
                name = product.Name,
                price = product.Price,
                description = product.Description
            };
            return Ok(obj);
        }
    }
}
