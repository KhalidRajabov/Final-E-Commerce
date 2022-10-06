using Final_E_Commerce.Areas.Admin.ViewModels;
using Final_E_Commerce.DAL;
using Final_E_Commerce.Entities;
using Final_E_Commerce.Extensions;
using Final_E_Commerce.Migrations;
using Final_E_Commerce.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Final_E_Commerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin, SuperAdmin")]
    public class CategoryController:Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public CategoryController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }



        //6ci deqiq
        public IActionResult Index(int page = 1, int take = 5)
        {
            ListCategory categoryVMs = new ListCategory();
            categoryVMs.Categories = _context.Categories.Where(c => c.ParentId == null && c.IsDeleted != true).Skip((page - 1) * take).Take(take).ToList();

            PaginationVM<Category> paginationVM = new PaginationVM<Category>(categoryVMs.Categories, PageCount(take), page);

            return View(paginationVM);
        }
        private int PageCount(int take)
        {
            ListCategory categories = new ListCategory();
            categories.Categories = _context.Categories.Where(c => c.IsDeleted != true).Where(c => c.ParentId == null).ToList();

            return (int)Math.Ceiling((decimal)categories.Categories.Count() / take);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryVM category)
        {
            if (category.Images == null)
            {
                ModelState.AddModelError("Photo", "Do not leave it empty");
                return View();
            }

            if (!category.Images.IsImage())
            {
                ModelState.AddModelError("Photo", "Do not leave it empty");
                return View();
            }
            if (category.Images.ValidSize(10000))
            {
                ModelState.AddModelError("Photo", "Image size can not be large");
                return View();
            }
            if (!ModelState.IsValid)
            {
                return View();
            }


            bool isValid = await _context.Categories.Where(c => c.IsDeleted != true).AnyAsync(c => c.Name.ToLower() == category.Name.ToLower());
            if (isValid)
            {
                ModelState.AddModelError("Name", "This category name already exists");
                return View();
            }
            Category newcategory = new Category
            {
                Name = category.Name,
                ImageUrl = category.Images.SaveImage(_env, "images"),
                CreatedTime = DateTime.UtcNow.AddHours(4),
                Description = category.Description
            };
            await _context.AddAsync(newcategory);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public IActionResult CreateSubCategory()
        {
            ViewBag.MainCategories = new SelectList(_context.Categories.Where(c => c.IsDeleted != true).Where(c => c.ParentId == null).ToList(), "Id", "Name");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateSubCategory(CategoryVM subcategory)
        {
            ViewBag.MainCategories = new SelectList(_context.Categories.Where(c => c.IsDeleted != true).Where(c => c.ParentId == null).ToList(), "Id", "Name");
            if (!ModelState.IsValid)
            {
                return View();
            }

            if (subcategory.ParentId == null)
            {
                ModelState.AddModelError("ParentId", "Select a category");
                return View();
            }
            bool isValid = _context.Categories.Where(c => c.IsDeleted != true).Where(c => c.ParentId != null).Any(c => c.Name.ToLower() == subcategory.Name.ToLower());
            if (isValid)
            {
                ModelState.AddModelError("Name", "This category name already exists");
                return View();
            }
            Category newsubcategory = new Category
            {
                Name = subcategory.Name,
                Description = subcategory.Description,
                ParentId = subcategory.ParentId
            };
            await _context.Categories.AddAsync(newsubcategory);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }




        public async Task<IActionResult> DeleteCategory(int? id)
        {
            if (id == null) return RedirectToAction("error", "home");
            Category category = await _context.Categories.FindAsync(id);
            if (category == null) return RedirectToAction("error", "home");
            category.IsDeleted = true;
            category.DeletedAt = DateTime.UtcNow.AddHours(4);
            await _context.SaveChangesAsync();
            return RedirectToAction("index");
        }

        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) return RedirectToAction("error", "home");
            Category category = await _context.Categories.FindAsync(id);
            if (category == null) return RedirectToAction("error", "home");
            CategoryVM categoryVM = new CategoryVM();
            categoryVM.Category = category;
            return View(categoryVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, CategoryVM category)
        {
            if (!ModelState.IsValid) return View();
            if (id == null) return RedirectToAction("error", "home");
            Category dbCategory = await _context.Categories.FindAsync(id);
            if (dbCategory == null) return RedirectToAction("error", "home");
            Category checkCategory = _context.Categories.FirstOrDefault(c => c.Name.ToLower() == category.Name.ToLower());
            if (checkCategory != null)
            {
                if (dbCategory.Name != checkCategory.Name)
                {
                    ModelState.AddModelError("Name", "This category name already exists");
                    return View();
                }
            }
            if (category.Images == null)
            {
                dbCategory.ImageUrl = dbCategory.ImageUrl;
            }
            else
            {
                if (!category.Images.IsImage())
                {
                    ModelState.AddModelError("Photo", "Choose images only");
                    return View();
                }
                if (category.Images.ValidSize(20000))
                {
                    ModelState.AddModelError("Photo", "Image size can not be large");
                    return View();
                }
                string oldImg = dbCategory.ImageUrl;
                string path = Path.Combine(_env.WebRootPath, "images", oldImg);
                Helper.Helper.DeleteImage(path);
                dbCategory.ImageUrl = category.Images.SaveImage(_env, "images");
            }
            dbCategory.Name = category.Name;
            dbCategory.Description = category.Description;
            dbCategory.LastUpdatedAt = DateTime.UtcNow.AddHours(4);

            await _context.SaveChangesAsync();
            return RedirectToAction("index");
        }

        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null) return RedirectToAction("error", "home");
            Category dbCategory = await _context.Categories.FindAsync(id);
            if (dbCategory == null) return RedirectToAction("error", "home");
            CategoryVM category = new CategoryVM();
            category.Category = dbCategory;
            List<Product> products = _context?.Products?.Where(c => c.Category.ParentId == dbCategory.Id).ToList();
            ViewBag.ProductCount=products.Count();
            return View(category);
        }
    }
}
