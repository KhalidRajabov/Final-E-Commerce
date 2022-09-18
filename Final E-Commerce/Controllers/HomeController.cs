using AutoMapper;
using Final_E_Commerce.DAL;
using Final_E_Commerce.DTO.Bio_DTO;
using Final_E_Commerce.Entities;
using Final_E_Commerce.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.DTO.CategoryDTO;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using WebApi.DTO.Product_DTOs;
using Microsoft.AspNetCore.Identity;

namespace Final_E_Commerce.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext? _context;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _usermanager;
        public HomeController(AppDbContext? context, IMapper mapper, UserManager<AppUser> usermanager)
        {
            _context = context;
            _mapper = mapper;
            _usermanager = usermanager;
        }
        public async Task<IActionResult> Index()
        {
            List<Product>? AllProducts = await _context.Products
                .Where(p=>p.DiscountPercent>0).ToListAsync();
            foreach (var product in AllProducts)
            {
                if (product.DiscountUntil<DateTime.Now)
                {
                    product.DiscountUntil = null;
                    product.DiscountPercent = 0;
                    product.DiscountPrice = 0;
                    _context.SaveChangesAsync();
                }
            }
            HomeVM homeVM = new HomeVM();
            homeVM.Bio = _context.Bios.FirstOrDefault();
            homeVM.Category = _context.Categories.FirstOrDefault(c=>c.Id==1);
            homeVM.MostPopularProduct = _context.Products
                .Where(p => p.Status == ProductConfirmationStatus.Approved)
                .OrderByDescending(p=>p.Views).Take(1).Include(p=>p.ProductImages).FirstOrDefault();
            homeVM.PopularProducts = _context.Products
                .Where(p => p.Status == ProductConfirmationStatus.Approved)
                .OrderByDescending(p=>p.Views).Skip(1).Take(3).Include(p=>p.ProductImages).ToList();
            homeVM.BestSellerProducts = _context.Products
                .OrderByDescending(p => p.Sold).Take(8)
                .Where(p=>p.Status==ProductConfirmationStatus.Approved).Include(p => p.ProductImages).ToList();
            return View(homeVM);
        }
        public async Task<IActionResult> Detail(int? id)
        {
            List<Product>? AllProducts = await _context.Products
               .Where(p => p.DiscountPercent > 0).ToListAsync();
            foreach (var item in AllProducts)
            {
                if (item.DiscountUntil < DateTime.Now)
                {
                    item.DiscountUntil = null;
                    item.DiscountPercent = 0;
                    item.DiscountPrice = 0;
                    _context.SaveChangesAsync();
                }
            }
            Product product = _context.Products
                .Where(p=>p.Status==ProductConfirmationStatus.Approved)
                .Include(p => p.ProductImages)
                .Include(c => c.Category)
                .Include(p => p.Brand)
                .Include(p => p.ProductTags)
                .ThenInclude(t => t.Tags)
                .FirstOrDefault(p => p.Id == id);
            AppUser ProductOwner =await _usermanager.FindByIdAsync(product.AppUserId);

            if (product == null) return RedirectToAction("Error");
            ViewBag.ExistWishlist = false;
            if (User.Identity.IsAuthenticated)
            {
                AppUser user = await _usermanager.FindByNameAsync(User.Identity.Name);
                bool IsExist = _context.Wishlists.Where(w => w.AppUserId == user.Id && w.ProductId == id).Any();
                if (IsExist)
                {
                    ViewBag.ExistWishlist = true;
                }
            }
            product.Views++;
            await _context.SaveChangesAsync();
            var UsersWantThis = _context.Wishlists.Where(p=>p.ProductId==id).ToList();
            DetailVM detailVM = new DetailVM();
            detailVM.Product = product;
            detailVM.Owner=ProductOwner;
            detailVM.UsersWantIt = UsersWantThis.Count;
            
            //detailVM.RelatedProducts= _context.Products.Where(c => c.CategoryId == product.CategoryId).ToList();

            return View(detailVM);
        }
        public IActionResult Error()
        {
            return View();
        }
      
    }
}