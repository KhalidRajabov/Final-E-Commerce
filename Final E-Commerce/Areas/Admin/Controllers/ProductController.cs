using Final_E_Commerce.DAL;
using Final_E_Commerce.Entities;
using Final_E_Commerce.Extensions;
using Final_E_Commerce.Helper;
using Final_E_Commerce.Migrations;
using Final_E_Commerce.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;
using System;
using Microsoft.AspNetCore.Identity;

namespace Final_E_Commerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin, SuperAdmin")]
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        private IConfiguration _config { get; }
        private readonly UserManager<AppUser> _usermanager;
        public ProductController(AppDbContext context, IWebHostEnvironment env, IConfiguration config, UserManager<AppUser> usermanager)
        {
            _context = context;
            _env = env;
            _config = config;
            _usermanager = usermanager;
        }


        public async Task<IActionResult> Index(int page = 1, int take = 5)
        {

            List<Products>? product = _context?.Products?
                .Where(p => p.Status == ProductConfirmationStatus.Approved)
                .OrderByDescending(p => p.Id)
                .Include(p => p.Category).Include(pi => pi.ProductImages)
                .Where(p => p.IsDeleted != true).Skip((page - 1) * take).Take(take).ToList();
            List<Products>? awaiting = await _context?.Products?
                .Where(p => !p.IsDeleted && p.Status == ProductConfirmationStatus.Pending).ToListAsync();
            ViewBag.Pending = awaiting.Count;
            PaginationVM<Products> paginationVM = new PaginationVM<Products>(product, PageCount(take), page);

            return View(paginationVM);
        }

        private int PageCount(int take)
        {
            List<Products>? products = _context?.Products?
                .Where(p => p.IsDeleted != true&& p.Status == ProductConfirmationStatus.Approved).ToList();
            return (int)Math.Ceiling((decimal)products.Count() / take);
        }

        
            

        public async Task<IActionResult> Create()
        {

            var mainCategories = await _context?.Categories?.Where(p => p.ParentId == null).Where(p => p.IsDeleted != true).ToListAsync();
            var altCategories = await _context.Categories.Where(c => c.ParentId != null).Where(p => p.IsDeleted != true).ToListAsync();
            ViewBag.altCategories = new SelectList((altCategories).ToList(), "Id", "Name");
            ViewBag.Categories = new SelectList(_context.Categories.Where(c => c.IsDeleted != true).Where(c => c.ParentId == null).ToList(), "Id", "Name");
            ViewBag.Brands = new SelectList(_context?.Brands?.Where(c => c.IsDeleted != true).ToList(), "Id", "Name");
            ViewBag.Tags = new SelectList(_context?.Tags?.Where(t => t.IsDeleted != true).ToList(), "Id", "Name");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductCreateVM product)
        {
            var mainCategories = await _context?.Categories?.Where(p => p.ParentId == null).Where(p => p.IsDeleted != true).ToListAsync();
            var altCategories = await _context.Categories.Where(c => c.ParentId != null).Where(p => p.IsDeleted != true).ToListAsync();
            ViewBag.altCategories = new SelectList((altCategories).ToList(), "Id", "Name");
            ViewBag.Categories = new SelectList(_context.Categories.Where(c => c.IsDeleted != true).Where(c => c.ParentId == null).ToList(), "Id", "Name");
            ViewBag.Brands = new SelectList(_context?.Brands?.Where(c => c.IsDeleted != true).ToList(), "Id", "Name");
            ViewBag.Tags = new SelectList(_context?.Tags?.Where(t => t.IsDeleted != true).ToList(), "Id", "Name");

            if (product.Name == null)
            {
                ModelState.AddModelError("Name", "Cannot be Empty!");
                return View();
            }

            List<ProductImage>? Images = new List<ProductImage>();
            foreach (var item in product.Photos)
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
                image.ImageUrl = item.SaveImage(_env, "images/products");
                Images.Add(image);
            }


            Products? NewProduct = new Products
            {
                Price = product.Price,
                Name = product.Name,
                Description = product.Description,
                ReleaseDate = product.ReleaseDate,
                OperationSystem = product.OperationSystem,
                GPU = product.GPU,
                Chipset = product.Chipset,
                Memory = product.Memory,
                Body = product.Body,
                Display = product.Display,
                FrontCamera = product.FrontCamera,
                RearCamera = product.RearCamera,
                Battery = product.Battery,
                Weight = product.Weight,
                Count = product.Count,
                Sold = 0,
                Profit = 0,
                Views = 0,
                IsDeleted = false,
                ProductImages = Images,
                BrandId = product.BrandId,
                CreatedTime = DateTime.Now,
                InStock = true,
                Status = ProductConfirmationStatus.Approved
                
            };
            if (product.DiscountPercent > 0 && product.DiscountUntil < DateTime.Now)
            {
                ModelState.AddModelError("DiscountUntil", "You can not set discount date for earlier than now");
                return View(product);
            }
            else if (product.DiscountUntil > DateTime.Now && product.DiscountUntil != null && product.DiscountPercent > 0)
            {
                NewProduct.DiscountUntil = product.DiscountUntil;
                NewProduct.DiscountPercent = product.DiscountPercent;
                NewProduct.DiscountPrice = product.Price - (product.Price * product.DiscountPercent) / 100;
            }
            NewProduct.ProductImages[0].IsMain = true;
            if (product?.SubCategory == null)
            {
                NewProduct.CategoryId = product.CategoryId;
            }
            else
            {
                NewProduct.CategoryId = product.SubCategory;
            }
            if (product.TagId!=null)
            {
                List<ProductTag> productTags = new List<ProductTag>();
                foreach (int item in product.TagId)
                {
                    ProductTag productTag = new ProductTag();
                    productTag.TagId = item;
                    productTag.ProductId = NewProduct.Id;
                    productTags.Add(productTag);
                }
                NewProduct.ProductTags = productTags;
            }
            
            await _context.AddAsync(NewProduct);
            await _context.SaveChangesAsync();

            return RedirectToAction("index");
        }



        public async Task<IActionResult> Update(int? id)
        {
            var altCategories = _context?.Categories?.Where(c => c.ParentId != null).Where(p => p.IsDeleted != true).ToList();
            ViewBag.Brands = new SelectList(_context?.Brands?.ToList(), "Id", "Name");
            ViewBag.Categories = new SelectList(_context?.Categories?.Where(c => c.IsDeleted != true).Where(c => c.ParentId == null).ToList(), "Id", "Name");
            ViewBag.altCategories = new SelectList((altCategories).ToList(), "Id", "Name");
            ViewBag.Tags = new SelectList(_context?.Tags?.Where(t => t.IsDeleted != true).ToList(), "Id", "Name");
            if (id == null) return RedirectToAction("error", "home");
            Products? product = await _context?.Products
                ?.Include(i => i.ProductImages)
                ?.Include(c => c.Category)
                ?.Include(b => b.Brand)
                ?.Include(t => t.ProductTags)
                ?.ThenInclude(p => p.Tags)
                ?.FirstOrDefaultAsync(c => c.Id == id);
            if (product == null) return RedirectToAction("error", "home");
            ProductUpdateVM productUpdateVM = new ProductUpdateVM
            {
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                ReleaseDate = product.ReleaseDate,
                OperationSystem = product.OperationSystem,
                GPU = product.GPU,
                Chipset = product.Chipset,
                Memory = product.Memory,
                Body = product.Body,
                Display = product.Display,
                FrontCamera = product.FrontCamera,
                RearCamera = product.RearCamera,
                Battery = product.Battery,
                Weight = product.Weight,
                DiscountPercent = product.DiscountPercent,
                DiscountUntil = product.DiscountUntil,
                Count = product.Count,
                CategoryId = product.CategoryId,
                BrandId = product.BrandId,
                Product = product,
                
            };
            return View(productUpdateVM);
        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, ProductUpdateVM? product)
        {
            var altCategories = _context?.Categories?.Where(c => c.ParentId != null).Where(p => p.IsDeleted != true).ToList();
            ViewBag.Brands = new SelectList(_context?.Brands?.ToList(), "Id", "Name");
            ViewBag.Categories = new SelectList(_context?.Categories?.Where(c => c.IsDeleted != true).Where(c => c.ParentId == null).ToList(), "Id", "Name");
            ViewBag.altCategories = new SelectList(altCategories?.ToList(), "Id", "Name");
            ViewBag.Tags = new SelectList(_context?.Tags?.Where(t => t.IsDeleted != true).ToList(), "Id", "Name");
            if (!ModelState.IsValid)
            {
                return View();
            }
            Products? dbProduct = await _context?.Products
                ?.Include(p => p.ProductImages)
                ?.Include(p => p.ProductTags)
                .ThenInclude(t => t.Tags)
                .Include(b => b.Brand)
                .Include(c => c.Category)
                .Where(c => c.IsDeleted != true)
                .FirstOrDefaultAsync(b => b.Id == id);
            if (dbProduct == null)
            {
                return View();
            }
            List<ProductImage> images = new List<ProductImage>();
            if (product.Photos == null)
            {
                foreach (var item in dbProduct.ProductImages)
                {
                    item.ImageUrl = item.ImageUrl;
                    _context.Add(item);
                }
            }
            else
            {
                foreach (var item in product.Photos)
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
                    image.ImageUrl = item.SaveImage(_env, "images/products");

                    if (product.Photos.Count == 1)
                    {
                        image.IsMain = true;
                    }
                    else
                    {
                        
                        images[0].IsMain = true;
                        /*for (int i = 0; i < images.Count; i++)
                        {
                        }*/
                    }
                    images.Add(image);
                }

                foreach (var item in product.Photos)
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

            if (product.TagId == null)
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
            }
            if (product.Category == null)
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
            




            dbProduct.Name = product.Name;
            dbProduct.Price = product.Price;
            dbProduct.ProductImages = images;
            dbProduct.Count = product.Count;
            dbProduct.BrandId = product.BrandId;
            dbProduct.Description = product.Description; 
            dbProduct.LastUpdatedAt = System.DateTime.Now;
            dbProduct.ReleaseDate = product.ReleaseDate;
            dbProduct.OperationSystem = product.OperationSystem;
            dbProduct.GPU = product.GPU;
            dbProduct.Chipset = product.Chipset;
            dbProduct.Memory = product.Memory;
            dbProduct.Body = product.Body;
            dbProduct.Display = product.Display;
            dbProduct.FrontCamera = product.FrontCamera;
            dbProduct.RearCamera = product.RearCamera;
            dbProduct.Battery = product.Battery;
            dbProduct.Weight = product.Weight;
            dbProduct.Status = ProductConfirmationStatus.Approved;
            if (product?.SubCategory == null)
            {
                dbProduct.CategoryId = product.CategoryId;
            }
            else
            {
                dbProduct.CategoryId = product.SubCategory;
            }
            if (product.DiscountPercent > 0 && product.DiscountUntil < DateTime.Now)
            {
                ModelState.AddModelError("DiscountUntil", "You can not set discount date for earlier than now");
                return View(product);
            }
            else if (product.DiscountUntil > DateTime.Now && product.DiscountUntil != null && product.DiscountPercent > 0)
            {
                dbProduct.DiscountUntil = product.DiscountUntil;
                dbProduct.DiscountPercent = product.DiscountPercent;
                dbProduct.DiscountPrice = product.Price - (product.Price * product.DiscountPercent) / 100;
            }
            if (product.DiscountPercent > 0 && product.DiscountPercent>dbProduct.DiscountPercent)
            {
                List<Wishlist>? wishlist = _context?.Wishlists?.Where(p => p.ProductId == dbProduct.Id).ToList();
                foreach (var user in wishlist)
                {
                    AppUser appUser = await _usermanager.FindByIdAsync(user.AppUserId);

                    var token = "";
                    string subject = "Endirim var!";
                    EmailHelper helper = new EmailHelper(_config.GetSection("ConfirmationParam:Email").Value, _config.GetSection("ConfirmationParam:Password").Value);

                    token = $"Salam. {dbProduct.Name} məhsulunda {dbProduct.DiscountPercent}% endirim var. \n" +
                        $"Artıq {dbProduct.Price} deyil, sadəcə {dbProduct.DiscountPrice} AZN\n" +
                        $"Məhsula keçid linki http://dante666-001-site1.atempurl.com/Home/detail/{dbProduct.Id}";
                    var emailResult = helper.SendNews(appUser.Email, token, subject);

                    string? discountemail = Url.Action("ConfirmEmail", "Account", new
                    {
                        token
                    }, Request.Scheme);
                }

            }
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }



        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null) return RedirectToAction("error", "home");
            Products? dbProduct = await _context?.Products?.Include(c => c.Category)
                ?.Include(c => c.ProductTags)
                .ThenInclude(t => t.Tags)
                .Include(p=>p.Brand)
                .Include(pi => pi.ProductImages)
                .FirstOrDefaultAsync(c => c.Id == id);
            if (dbProduct == null) return RedirectToAction("error", "home");
            AppUser user = await _usermanager.FindByIdAsync(dbProduct.AppUserId);
            DetailVM detailVM = new DetailVM();
            detailVM.Product = dbProduct;
            detailVM.Owner = user;
            return View(detailVM);
        }


        

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return RedirectToAction("error", "home");
            Products? product = await _context.Products.FindAsync(id);
            if (product == null) return RedirectToAction("error", "home");
            product.IsDeleted = true;
            product.DeletedAt = DateTime.Now;
            await _context.SaveChangesAsync();
            return RedirectToAction("index");
        }

        public IActionResult GetSubCategory(int cid)
        {
            var SubCategory_List = _context?.Categories?.Where(s => s.ParentId == cid).Where(s => s.ParentId != null).Select(c => new { Id = c.Id, Name = c.Name }).ToList();
            return Json(SubCategory_List);
        }
        public async Task<IActionResult> EditPictures(int id)
        {
            List<Products>? AllProducts = await _context?.Products?
               .Where(p => p.DiscountPercent > 0).ToListAsync();
            foreach (var item in AllProducts)
            {
                if (item.DiscountUntil < DateTime.Now)
                {
                    item.DiscountUntil = null;
                    item.DiscountPercent = 0;
                    item.DiscountPrice = 0;
                    _context?.SaveChangesAsync();
                }
            }
            
            Products? p = await _context?.Products?
                .Include(i => i.ProductImages)?
                .Include(c => c.Category)?
                .Include(t => t.ProductTags)?
                .ThenInclude(p => p.Tags)
                .Include(b => b.Brand)
                .FirstOrDefaultAsync(c => c.Id == id);
            if (p == null) return RedirectToAction("error", "home");
            ProductUpdateVM vm = new ProductUpdateVM();
            vm.Product = p;
            return View(vm);
        }

        public async Task<IActionResult> MainImage(int? imageid, int? productid, string? Returnurl)
        {
            List<Products>? AllProducts = await _context?.Products?
               .Where(p => p.DiscountPercent > 0).ToListAsync();
            foreach (var item in AllProducts)
            {
                if (item.DiscountUntil < DateTime.Now)
                {
                    item.DiscountUntil = null;
                    item.DiscountPercent = 0;
                    item.DiscountPrice = 0;
                    await _context.SaveChangesAsync();
                }
            }
            if (imageid == null || productid == null) return RedirectToAction("error", "home");
            var image = await _context?.ProductImages?.FirstOrDefaultAsync(x => x.Id == imageid && x.ProductId == productid);
            if (image == null) return RedirectToAction("error", "home");
            var product = await _context?.Products?.Include(x => x.ProductImages).FirstOrDefaultAsync(x => x.Id == productid);
            if (product == null) return RedirectToAction("error", "home");
            var mainImage = product.ProductImages?.FirstOrDefault(x => x.IsMain);
            mainImage.IsMain = false;

            image.IsMain = true;
            product.LastUpdatedAt = DateTime.Now;
            await _context.SaveChangesAsync();
            if (Returnurl != null)
            {
                return Redirect(Returnurl);
            }
            return RedirectToAction("index");
        }
        public async Task<IActionResult> RemoveImage(int? imageid, int? productid, string Returnurl)
        {
            List<Products>? AllProducts = await _context?.Products?
               .Where(p => p.DiscountPercent > 0).ToListAsync();
            foreach (var item in AllProducts)
            {
                if (item.DiscountUntil < DateTime.Now)
                {
                    item.DiscountUntil = null;
                    item.DiscountPercent = 0;
                    item.DiscountPrice = 0;
                    await _context.SaveChangesAsync();
                }
            }
            if (imageid == null || productid == null) return RedirectToAction("error", "home");
            var image = await _context?.ProductImages?.FirstOrDefaultAsync(x => x.Id == imageid && x.ProductId == productid);
            if (image == null) return RedirectToAction("error", "home");

            string? path = Path.Combine(_env.WebRootPath, @"images\products", image.ImageUrl);
            Helper.Helper.DeleteImage(path);

            _context.ProductImages.Remove(image);
            Products? product = await _context?.Products?.FirstOrDefaultAsync(p => p.Id == image.ProductId);
            product.LastUpdatedAt = DateTime.Now;
            await _context.SaveChangesAsync();

            return Redirect(Returnurl);
        }





        public async Task<IActionResult> Pendings()
        {
            ListProductsVM listProductsVM = new ListProductsVM
            {
                Products = await _context?.Products?
                .Where(p => !p.IsDeleted && p.Status == ProductConfirmationStatus.Pending)
                ?.Include(p => p.ProductImages)
                ?.Include(c => c.Category)
                ?.Include(p => p.Brand)
                ?.Include(p => p.ProductTags)
                ?.ThenInclude(t => t.Tags)
                ?.Include(r => r.UserProductRatings)
                .ToListAsync()
            };
            return View(listProductsVM);
        }
        public async Task<IActionResult> PendingDetail(int? id)
        {
            DetailVM product = new DetailVM
            {
                Product = await _context?.Products?
                .Where(p => !p.IsDeleted && p.Status == ProductConfirmationStatus.Pending)
                ?.Include(p => p.ProductImages)
                ?.Include(c => c.Category)
                ?.Include(p => p.Brand)
                ?.Include(p => p.ProductTags)
                ?.ThenInclude(t => t.Tags)
                ?.Include(r => r.UserProductRatings)
                .FirstOrDefaultAsync(p=>p.Id==id)
            };
            return View(product);
            
        }


        public async Task<IActionResult> Confirm(int? id)
        {
            if (id == null) return RedirectToAction("error", "home");
            Products? product= await _context?.Products?.Where(o => o.Id == id).FirstOrDefaultAsync();
            if (product == null) return RedirectToAction("error", "home");
            product.Status = ProductConfirmationStatus.Approved;
            await _context.SaveChangesAsync();
            AppUser user =await _usermanager.FindByIdAsync(product.AppUserId);
            if (user == null) return RedirectToAction("error", "home");
            else
            {
                var token = "";
                string subject = "Your product was confirmed";
                EmailHelper helper = new EmailHelper(_config.GetSection("ConfirmationParam:Email").Value, _config.GetSection("ConfirmationParam:Password").Value);

                token = $"Hello {user.Fullname}. We've finished rewieving {product.Name}, now it is available on the site. \n" +
                    $"Review your <a href='https://localhost:44393/User/productdetail/{product.Id}' style='color:red'>{product.Name} </a>";
                var emailResult = helper.SendNews(user.Email, token, subject);

                string? discountemail = Url.Action("ConfirmEmail", "Account", new
                {
                    token
                }, Request.Scheme);
            }

            List<UserSubscription> subscribers = await _context.Subscription.Where(s => s.ProfileId == product.AppUserId).ToListAsync();
            foreach (var sub in subscribers)
            {
                AppUser follower = await _usermanager.FindByIdAsync(sub.SubscriberId);
                var token = "";
                string subject = $"New product by {user.Fullname}";
                EmailHelper helper = new EmailHelper(_config.GetSection("ConfirmationParam:Email").Value, _config.GetSection("ConfirmationParam:Password").Value);

                token = $"Hello {follower.Fullname}. {user.Fullname} Just added a new item. <br> \n" +
                    $"{product.Name} for the price of {product.Price}. <a href='https://localhost:44393/User/productdetail/{product.Id}' style='color:red'>Check i </a>";
                var emailResult = helper.SendNews(user.Email, token, subject);

                string? discountemail = Url.Action("ConfirmEmail", "Account", new
                {
                    token
                }, Request.Scheme);

            }
            var obj = new
            {
                status="successfull"
            };
            return Ok(obj);
        }


        public async Task<IActionResult> Decline(int? id)
        {
            if (id == null) return RedirectToAction("error", "home");
            Products? product = await _context?.Products?.Where(o => o.Id == id).FirstOrDefaultAsync();
            if (product == null) return RedirectToAction("error", "home");
            product.Status = ProductConfirmationStatus.Refused;
            await _context.SaveChangesAsync();
            return RedirectToAction("Pendings");
        }
    }
}
