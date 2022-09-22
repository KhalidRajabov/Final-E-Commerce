using Final_E_Commerce.Areas.Admin.ViewModels;
using Final_E_Commerce.DAL;
using Final_E_Commerce.Entities;
using Final_E_Commerce.Extensions;
using Final_E_Commerce.Migrations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Final_E_Commerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin, SuperAdmin")]
    public class BrandController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        public BrandController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index()
        {
            ListBrandVM brandVM = new ListBrandVM
            {
                Brands = _context.Brands.Where(b => b.IsDeleted != true).ToList()
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

            bool isValid = _context.Brands.Where(b => b.IsDeleted != true).Any(c => c.Name.ToLower() == brand.Name.ToLower());
            if (isValid)
            {
                ModelState.AddModelError("Name", "This category name already exists");
                return View();
            }
            brand.ImageUrl = brand.Photo.SaveImage(_env, "images/brand");
            brand.Brand.CreatedTime = System.DateTime.Now;
            await _context.Brands.AddAsync(brand.Brand);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }


        public async Task<IActionResult> Update(int? id)
        {

            if (id == null) return RedirectToAction("error", "home");
            Brand brand = await _context.Brands.FindAsync(id);
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
            Brand dbBrand = await _context.Brands.FindAsync(id);
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
                string oldImg = dbBrand.ImageUrl;
                string path = Path.Combine(_env.WebRootPath, "img", oldImg);
                dbBrand.ImageUrl = brand.Photo.SaveImage(_env, "images/brand");
                Helper.Helper.DeleteImage(path);

            }
            dbBrand.Name = brand.Name;
            dbBrand.LastUpdatedAt = DateTime.Now;

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
            if (id == null) return RedirectToAction("error", "home");
            Brand brands = await _context.Brands.FindAsync(id);
            if (brands == null) return RedirectToAction("error", "home");
            brands.IsDeleted = true;
            brands.DeletedAt = DateTime.Now;
            await _context.SaveChangesAsync();
            return RedirectToAction("index");
        }

    }
}
