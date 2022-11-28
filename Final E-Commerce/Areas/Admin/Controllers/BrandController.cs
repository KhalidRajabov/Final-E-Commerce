using Final_E_Commerce.Areas.Admin.ViewModels;
using Final_E_Commerce.DAL;
using Final_E_Commerce.Entities;
using Final_E_Commerce.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;

namespace Final_E_Commerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin, SuperAdmin")]
    public class BrandController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly UserManager<AppUser> _usermanager;
        public BrandController(AppDbContext context, IWebHostEnvironment env, UserManager<AppUser> usermanager)
        {
            _context = context;
            _env = env;
            _usermanager = usermanager;
        }
        public IActionResult Index()
        {
            ListBrandVM brandVM = new ListBrandVM
            {
                Brands = _context?.Brands?.Where(b => b.IsDeleted != true).ToList()
            };
            
            return View(brandVM);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BrandVM brand)
        {
            if (brand.Photo == null)
            {
                ModelState.AddModelError("Photo", "Do not leave it empty");
                return View();
            }

            if (!brand.Photo.IsImage())
            {
                ModelState.AddModelError("Photo", "Do not leave it empty");
                return View();
            }
            if (brand.Photo.ValidSize(10000))
            {
                ModelState.AddModelError("Photo", "Image size can not be large");
                return View();
            }

            if (!ModelState.IsValid)
            {
                return View();
            }

            bool isValid = await _context?.Brands?.Where(b => b.IsDeleted != true).AnyAsync(c => c.Name.ToLower() == brand.Name.ToLower());
            if (isValid)
            {
                ModelState.AddModelError("Name", "This brand name already exists");
                return View();
            }
            Brand NewBrand = new Brand();
            NewBrand.ImageUrl = brand.Photo.SaveImage(_env, "images/brands");
            NewBrand.CreatedTime = DateTime.Now.AddHours(12);
            NewBrand.Name = brand.Name;
            await _context.AddAsync(NewBrand);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }


        public async Task<IActionResult> Update(int? id)
        {

            if (id == null) return RedirectToAction("error", "home");
            Brand? brand = await _context.Brands.FindAsync(id);
            if (brand == null) return RedirectToAction("error", "home");
            BrandVM brandVM = new BrandVM();
            brandVM.Brand = brand;
            return View(brandVM);
        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, BrandVM brand)
        {

            if (!ModelState.IsValid) return View();
            if (id == null) return RedirectToAction("error", "home");
            Brand? dbBrand = await _context.Brands.FindAsync(id);
            if (dbBrand == null) return RedirectToAction("error", "home");

            if (brand.Photo == null)
            {
                dbBrand.ImageUrl = dbBrand.ImageUrl;
            }
            else
            {
                if (!brand.Photo.IsImage())
                {
                    ModelState.AddModelError("Photo", "Choose images only");
                    return View();
                }
                if (brand.Photo.ValidSize(20000))
                {
                    ModelState.AddModelError("Photo", "Image size can not be large");
                    return View();
                }
                string? oldImg = dbBrand.ImageUrl;
                string path = Path.Combine(_env.WebRootPath, "img", oldImg);
                dbBrand.ImageUrl = brand.Photo.SaveImage(_env, "images/brands");
                Helper.Helper.DeleteImage(path);

            }
            dbBrand.Name = brand.Name;
            dbBrand.LastUpdatedAt = DateTime.Now.AddHours(12);

            await _context.SaveChangesAsync();
            return RedirectToAction("index");
        }

        public async Task<IActionResult> Details(int id)
        {
            if (id == null) return RedirectToAction("error", "home");
            BrandVM brand = new BrandVM();
            brand.Brand=await _context.Brands.FindAsync(id); 
            
            if (brand.Brand == null) return RedirectToAction("error", "home");
            ViewBag.BrandProducts = _context?.Products?.Where(p => p.BrandId == id).Count();

            return View(brand);

        }

        public async Task<IActionResult> Delete(int? id)
        {
            AppUser user = await _usermanager.FindByNameAsync(User.Identity.Name);
            if (id == null) return RedirectToAction("error", "home");
            Brand? brands = await _context.Brands.FindAsync(id);
            if (brands == null) return RedirectToAction("error", "home");
            brands.IsDeleted = true;
            brands.DeletedAt = DateTime.Now.AddHours(12);
            brands.DeletedBy = user.Fullname;
            await _context.SaveChangesAsync();
            return RedirectToAction("index");
        }

    }
}
