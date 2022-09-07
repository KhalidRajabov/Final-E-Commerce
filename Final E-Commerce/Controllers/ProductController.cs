using Final_E_Commerce.DAL;
using Final_E_Commerce.Entities;
using Final_E_Commerce.Helper;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using System.Security.Policy;
using Final_E_Commerce.ViewModels;
using Microsoft.EntityFrameworkCore;
using Final_E_Commerce.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace Final_E_Commerce.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _usermanager;
        private readonly IWebHostEnvironment _env;
        private IConfiguration _config { get; }
        public ProductController(AppDbContext context, IWebHostEnvironment env, IConfiguration config, UserManager<AppUser> usermanager)
        {
            _context = context;
            _env = env;
            _config = config;
            _usermanager = usermanager;
        }


        public async Task<IActionResult> Index(int page = 1, int take = 5)
        {
            AppUser user = await _usermanager.FindByNameAsync(User.Identity.Name);
            List<Product> product = _context.Products.Where(p=>p.AppUserId==user.Id).OrderByDescending(p => p.Id).Include(p => p.Category).Include(pi => pi.ProductImages)
                .Where(p => p.IsDeleted != true).Skip((page - 1) * take).Take(take).ToList();
            PaginationVM<Product> paginationVM = new PaginationVM<Product>(product, PageCount(take), page);

            return View(paginationVM);
        }

        private int PageCount(int take)
        {
            List<Product> products = _context.Products.Where(p => p.IsDeleted != true).ToList();
            return (int)Math.Ceiling((decimal)products.Count() / take);
        }



        public async Task<IActionResult> Create()
        {

            var mainCategories = await _context.Categories.Where(p => p.ParentId == null).Where(p => p.IsDeleted != true).ToListAsync();
            var altCategories = await _context.Categories.Where(c => c.ParentId != null).Where(p => p.IsDeleted != true).ToListAsync();
            ViewBag.altCategories = new SelectList((altCategories).ToList(), "Id", "Name");
            ViewBag.Categories = new SelectList(_context.Categories.Where(c => c.IsDeleted != true).Where(c => c.ParentId == null).ToList(), "Id", "Name");
            ViewBag.Brands = new SelectList(_context.Brands.Where(c => c.IsDeleted != true).ToList(), "Id", "Name");
            ViewBag.Tags = new SelectList(_context.Tags.Where(t => t.IsDeleted != true).ToList(), "Id", "Name");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product)
        {
            var mainCategories = await _context.Categories.Where(p => p.ParentId == null).Where(p => p.IsDeleted != true).ToListAsync();
            var altCategories = await _context.Categories.Where(c => c.ParentId != null).Where(p => p.IsDeleted != true).ToListAsync();
            ViewBag.altCategories = new SelectList((altCategories).ToList(), "Id", "Name");
            ViewBag.Categories = new SelectList(_context.Categories.Where(c => c.IsDeleted != true).Where(c => c.ParentId == null).ToList(), "Id", "Name");
            ViewBag.Brands = new SelectList(_context.Brands.Where(c => c.IsDeleted != true).ToList(), "Id", "Name");
            ViewBag.Tags = new SelectList(_context.Tags.Where(t => t.IsDeleted != true).ToList(), "Id", "Name");

            if (product.Name == null)
            {
                ModelState.AddModelError("Name", "Cannot be Empty!");
                return View();
            }

            List<ProductImage> Images = new List<ProductImage>();
            foreach (var item in product.Photo)
            {
                if (item == null)
                {
                    ModelState.AddModelError("Photo", "Do not leave it empty");
                    return View();
                }
                if (!item.IsImage())
                {
                    ModelState.AddModelError("Photo", "Do not leave it empty");
                    return View();
                }
                if (item.ValidSize(10000))
                {
                    ModelState.AddModelError("Photo", "Image size can not be large");
                    return View();
                }
                ProductImage image = new ProductImage();
                image.ImageUrl = item.SaveImage(_env, "images/product");
                Images.Add(image);
            }


            Product NewProduct = new Product
            {
                Price = product.Price,
                Name = product.Name,
                BrandId = product.BrandId,
                CategoryId = product.CategoryId,
                DiscountPercent = product.DiscountPercent,
                DiscountPrice = product.Price - (product.Price * product.DiscountPercent) / 100,
                Count = product.Count,
                ProductImages = Images,
                Description = product.Description,
                CreatedTime = DateTime.Now
            };

            NewProduct.ProductImages[0].IsMain = true;
            if (product.Category == null)
            {
                NewProduct.CategoryId = product.CategoryId;
            }
            else
            {
                NewProduct.CategoryId = product.CategoryId;
            }
            List<ProductTag> productTags = new List<ProductTag>();
            foreach (int item in product.TagId)
            {
                ProductTag productTag = new ProductTag();
                productTag.TagId = item;
                productTag.ProductId = NewProduct.Id;
                productTags.Add(productTag);
            }
            NewProduct.ProductTags = productTags;
            await _context.AddAsync(NewProduct);
            await _context.SaveChangesAsync();



            return RedirectToAction("index");
        }



        public async Task<IActionResult> Update(int? id)
        {
            var altCategories = _context.Categories.Where(c => c.ParentId != null).Where(p => p.IsDeleted != true).ToList();
            ViewBag.Brands = new SelectList(_context.Brands.ToList(), "Id", "Name");
            ViewBag.Categories = new SelectList(_context.Categories.Where(c => c.IsDeleted != true).Where(c => c.ParentId == null).ToList(), "Id", "Name");
            ViewBag.altCategories = new SelectList((altCategories).ToList(), "Id", "Name");
            ViewBag.Tags = new SelectList(_context.Tags.Where(t => t.IsDeleted != true).ToList(), "Id", "Name");
            if (id == null) return NotFound();
            Product product = await _context.Products
                .Include(i => i.ProductImages)
                .Include(c => c.Category)
                .Include(b => b.Brand)
                .Include(t => t.ProductTags).ThenInclude(p => p.Tags).FirstOrDefaultAsync(c => c.Id == id);
            if (product == null) return NotFound();
            return View(product);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, Product product)
        {
            var altCategories = _context.Categories.Where(c => c.ParentId != null).Where(p => p.IsDeleted != true).ToList();
            ViewBag.Brands = new SelectList(_context.Brands.ToList(), "Id", "Name");
            ViewBag.Categories = new SelectList(_context.Categories.Where(c => c.IsDeleted != true).Where(c => c.ParentId == null).ToList(), "Id", "Name");
            ViewBag.altCategories = new SelectList((altCategories).ToList(), "Id", "Name");
            ViewBag.Tags = new SelectList(_context.Tags.Where(t => t.IsDeleted != true).ToList(), "Id", "Name");
            if (!ModelState.IsValid)
            {
                return View();
            }
            Product dbProduct = await _context.Products
                .Include(p => p.ProductImages)
                .Include(p => p.ProductTags)
                .ThenInclude(t => t.Tags)
                .Include(b => b.Brand)
                .Include(c => c.Category)
                .Where(c => c.IsDeleted != true)
                .FirstOrDefaultAsync(b => b.Id == product.Id);
            if (dbProduct == null)
            {
                return View();
            }
            List<ProductImage> images = new List<ProductImage>();
            string path = "";
            if (product.Photo == null)
            {
                foreach (var item in dbProduct.ProductImages)
                {
                    item.ImageUrl = item.ImageUrl;
                    _context.Add(item);
                }
            }
            else
            {
                foreach (var item in product.Photo)
                {
                    if (item == null)
                    {
                        ModelState.AddModelError("Photo", "Can not be empty");
                        return View();
                    }
                    if (!item.IsImage())
                    {
                        ModelState.AddModelError("Photo", "Only images");
                        return View();
                    }

                    if (item.ValidSize(20000))
                    {
                        ModelState.AddModelError("Photo", "The image size is larger than required size(max 20 mb)");
                        return View();
                    }
                    ProductImage image = new ProductImage();
                    image.ImageUrl = item.SaveImage(_env, "images/product");

                    if (product.Photo.Count == 1)
                    {
                        image.IsMain = true;
                    }
                    else
                    {
                        for (int i = 0; i < images.Count; i++)
                        {
                            images[0].IsMain = true;
                        }
                    }
                    images.Add(image);
                }

                foreach (var item in product.Photo)
                {
                    if (!item.IsImage())
                    {
                        ModelState.AddModelError("Photo", "Images only");
                        return View();
                    }

                    if (item.ValidSize(20000))
                    {
                        ModelState.AddModelError("Photo", "The image size is larger than required size(max 20 mb)");
                        return View();
                    }
                }
            }

            foreach (var item in dbProduct.ProductImages)
            {
                if (item.ImageUrl != null)
                {
                    path = Path.Combine(_env.WebRootPath, "images/products", item.ImageUrl);
                }
            }
            if (path != null)
            {
                Helper.Helper.DeleteImage(path);
            }
            else return NotFound();

            /*if (product.TagId == null)
            {
                foreach (var item1 in dbProduct.ProductTags)
                {
                    item1.TagId = item1.TagId;
                }
            }
            else
            {
                List<ProductTag> productTags = new List<ProductTag>();
                foreach (int item in product.TagId)
                {
                    ProductTag productTag = new ProductTag();
                    productTag.TagId = item;
                    productTag.ProductId = dbProduct.Id;
                    productTags.Add(productTag);
                }
                dbProduct.ProductTags = productTags;
            }*/
            if (product.Category == null && product.Category == null)
            {
                dbProduct.CategoryId = dbProduct.CategoryId;
            }
            else
            {
                dbProduct.CategoryId = product.CategoryId;
            }

            if (product.Count == 0)
            {
                dbProduct.InStock = false;
            }
            List<Category> categories = _context.Categories.Where(p => p.IsDeleted != true).Where(c => c.ImageUrl != null).ToList();
            for (int i = 0; i < categories.Count; i++)
            {
                if (product.Category == categories[0])
                {
                    dbProduct.CategoryId = dbProduct.CategoryId;
                }
            }




            dbProduct.Name = product.Name;
            dbProduct.Price = product.Price;
            dbProduct.ProductImages = images;
            dbProduct.Count = product.Count;
            dbProduct.DiscountPercent = product.DiscountPercent;
            dbProduct.DiscountPrice = product.Price - (product.Price * product.DiscountPercent) / 100;
            dbProduct.BrandId = product.BrandId;
            dbProduct.CategoryId = product.CategoryId;
            dbProduct.Description = product.Description;
            dbProduct.IsFeatured = product.IsFeatured;
            dbProduct.NewArrival = product.NewArrival;
            dbProduct.Bestseller = product.Bestseller;
            dbProduct.LastUpdatedAt = System.DateTime.Now;
            if (dbProduct.DiscountPercent > 30)
            {
                List<Subscriber> subscribers = await _context.Subscribers.ToListAsync();
                var token = "";
                string subject = "Endirim var!";
                EmailHelper helper = new EmailHelper(_config.GetSection("ConfirmationParam:Email").Value, _config.GetSection("ConfirmationParam:Password").Value);
                foreach (var receiver in subscribers)
                {
                    token = $"Salam. {dbProduct.Name} məhsulunda {dbProduct.DiscountPercent}% endirim var. \n" +
                        $"Məhsula keçid linki https://localhost:44347/Home/detail/{dbProduct.Id}";
                    var emailResult = helper.SendNews(receiver.Email, token, subject);
                    continue;
                }
                string confirmation = Url.Action("ConfirmEmail", "Account", new
                {
                    token
                }, Request.Scheme);
            }
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }



        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null) return NotFound();
            Product dbProduct = await _context.Products.Include(c => c.Category)
                .Include(c => c.ProductTags)
                .ThenInclude(t => t.Tags)
                .Include(pi => pi.ProductImages)
                .FirstOrDefaultAsync(c => c.Id == id);
            if (dbProduct == null) return NotFound();
            return View(dbProduct);
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            Product product = await _context.Products.FindAsync(id);
            if (product == null) return NotFound();
            product.IsDeleted = true;
            product.DeletedAt = DateTime.Now;
            await _context.SaveChangesAsync();
            return RedirectToAction("index");
        }

        public IActionResult GetSubCategory(int cid)
        {
            var SubCategory_List = _context.Categories.Where(s => s.ParentId == cid).Where(s => s.ParentId != null).Select(c => new { Id = c.Id, Name = c.Name }).ToList();
            return Json(SubCategory_List);
        }
    }
}
