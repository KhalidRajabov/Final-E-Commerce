﻿using Final_E_Commerce.Areas.Admin.ViewModels;
using Final_E_Commerce.DAL;
using Final_E_Commerce.Entities;
using Final_E_Commerce.Extensions;
using Final_E_Commerce.Helper;
using Final_E_Commerce.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Final_E_Commerce.Controllers
{
    [Authorize]
    public class UserController:Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _usermanager;
        private readonly IWebHostEnvironment _env;
        private IConfiguration _config { get; }

        public UserController(AppDbContext context,
        UserManager<AppUser> userManager,
        IWebHostEnvironment env,
        IConfiguration config)
        {
            _context = context;
            _usermanager = userManager;
            _env = env;
            _config = config;
        }
        public async Task<IActionResult> Index()
        {
            AppUser user = await _usermanager.FindByNameAsync(User.Identity.Name);
            UserVM? userVM = new UserVM();
            
            userVM.User = user;
            userVM.UserProfile = await _context?.UserProfiles?.FirstOrDefaultAsync(up => up.AppUserId == user.Id);
            return View(userVM);
        }
        public async Task<IActionResult> UserDetail()
        {
            AppUser? user = await _usermanager.FindByNameAsync(User.Identity.Name);
            UserDetailsVM? userVM = new UserDetailsVM();
            UserDetails? detail = await _context?.UserDetails?.FirstOrDefaultAsync(up => up.AppUserId == user.Id);
            userVM.Firstname = detail.Firstname;
            userVM.Lastname = detail.Lastname;
            userVM.City = detail.City;
            userVM.Country = detail.Country;
            userVM.Street = detail.Street;
            userVM.Company = detail.Company;
            userVM.Email=detail.Email;
            userVM.PhoneNumber = detail.PhoneNumber;
            userVM.ZipCode = detail.ZipCode;
            return View(userVM);
        }


        //not working yet
        public IActionResult ChangeImage()
        {
            return View();
        }


        //not working yet
        [HttpPost]
        public async Task<IActionResult> ChangeImage(UserPhotoVM image)
        {
            if (image.Photo == null)
            {
                ModelState.AddModelError("Photo", "Do not leave it empty");
                return View(image);
            }

            if (!image.Photo.IsImage())
            {
                ModelState.AddModelError("Photo", "Only images");
                return View(image);
            }
            if (image.Photo.ValidSize(10000))
            {
                ModelState.AddModelError("Photo", "Image size can not be larger than 10mb");
                return View(image);
            }
            AppUser user = await _usermanager.FindByNameAsync(User.Identity.Name);
            user.ProfilePicture = image.Photo.SaveImage(_env, "images/ProfilePictures");
            await _context.SaveChangesAsync();
            return RedirectToAction("index");
        }




        public async Task<IActionResult> ProfilePage(string id)
        {
            if (id==null) return RedirectToAction("error", "home");
            AppUser user =await _usermanager.FindByIdAsync(id);
            if (user == null) return RedirectToAction("error", "home");
            UserVM userVM = new UserVM
            {
                User = user,
                UserProfile = await _context.UserProfiles.FirstOrDefaultAsync(u=>u.AppUserId==id)

            };
            return View(userVM);
        }
        public async Task<IActionResult> UpdateProfile()
        {
            AppUser user =await _usermanager.FindByNameAsync(User.Identity.Name);
            UserProfile? profile = await _context?.UserProfiles?.FirstOrDefaultAsync(u => u.AppUserId == user.Id);
            UserProfileVM vM = new UserProfileVM
            {
                Firstname = user.Firstname,
                Lastname = user.Lastname,
                Username = user.UserName,
                PhoneNumber = user.PhoneNumber,
                Birthdate = profile.Birthdate,
                EmailForPublic = profile.EmailForPublic,
                FavouriteBooks = profile.FavouriteBooks,
                FavouriteMovies = profile.FavouriteMovies,
                FavouriteMusics = profile.FavouriteMusics,
                Hobbies = profile.Hobbies,
                AboutMe = profile.AboutMe
            };
            return View(vM);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProfile(UserProfileVM? userProfile)
        {
            AppUser user = await _usermanager.FindByNameAsync(User.Identity.Name);
            if (userProfile.Photo != null)
            {
                if (!userProfile.Photo.IsImage())
                {
                    ModelState.AddModelError("Photo", "Only images");
                    return View(userProfile);
                }
                if (userProfile.Photo.ValidSize(10000))
                {
                    ModelState.AddModelError("Photo", "Image size can not be larger than 10mb");
                    return View(userProfile);
                }
                user.ProfilePicture = userProfile.Photo.SaveImage(_env, "images/ProfilePictures");
            }

            user.PhoneNumber = userProfile.PhoneNumber;
            user.Firstname = userProfile.Firstname;
            user.Lastname = userProfile.Lastname;
            user.Fullname = $"{userProfile.Firstname} {userProfile.Lastname}";
            user.UserName = userProfile.Username;

            UserProfile? profile = await _context?.UserProfiles?.FirstOrDefaultAsync(u => u.AppUserId == user.Id);
            profile.Birthdate = userProfile.Birthdate;
            profile.AboutMe = userProfile.AboutMe;
            profile.Hobbies = userProfile.Hobbies;
            profile.FavouriteMusics = userProfile.FavouriteMusics;
            profile.FavouriteMovies = userProfile.FavouriteMovies;
            profile.FavouriteBooks = userProfile.FavouriteBooks;
            await _context.SaveChangesAsync();
            return RedirectToAction("index");
        }

            public async Task<IActionResult> UpdateProfileDetail()
        {
            AppUser? user = await _usermanager.FindByNameAsync(User.Identity.Name);
            UserDetailsVM? userVM = new UserDetailsVM();
            UserDetails? detail = await _context?.UserDetails?.FirstOrDefaultAsync(up => up.AppUserId == user.Id);
            userVM.Firstname = detail.Firstname;
            userVM.Lastname = detail.Lastname;
            userVM.City = detail.City;
            userVM.Country = detail.Country;
            userVM.Street = detail.Street;
            userVM.Company = detail.Company;
            userVM.Email = detail.Email;
            userVM.PhoneNumber = detail.PhoneNumber;
            userVM.ZipCode = detail.ZipCode;
            return View(userVM);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateProfileDetail(UserDetailsVM detail)
        {
            AppUser? user = await _usermanager.FindByNameAsync(User.Identity.Name);
            UserDetails? userDetails = await _context?.UserDetails?.FirstOrDefaultAsync(up => up.AppUserId == user.Id);
            userDetails.Firstname = detail.Firstname;
            userDetails.Lastname = detail.Lastname;
            userDetails.City = detail.City;
            userDetails.Country = detail.Country;
            userDetails.Street = detail.Street;
            userDetails.Company = detail.Company;
            userDetails.Email = detail.Email;
            userDetails.PhoneNumber = detail.PhoneNumber;
            userDetails.ZipCode = detail.ZipCode;
            _context.SaveChanges();
            return RedirectToAction("UserDetail", "user");
        }
        public async Task<IActionResult> Orders()
        {
            List<Product>? AllProducts = await _context?.Products?
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
            AppUser user = await _usermanager.FindByNameAsync(User.Identity.Name);
            List<Order>? order = _context?.Orders?.Where(o => o.AppUserId == user.Id).OrderByDescending(o => o.Id).ToList();
            OrderVM orderVM = new OrderVM();
            orderVM.Orders = order;
            return View(orderVM);
        }

        public async Task<IActionResult> Detail(int id)
        {
            List<Product>? AllProducts = await _context?.Products?
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
            Order? order = await _context?.Orders?.Where(o => o.Id == id).FirstOrDefaultAsync();
            List<OrderItem> orderItems = await _context?.OrderItems?.Where(o => o.OrderId == order.Id).ToListAsync();
            AppUser? user = await _usermanager?.Users?.FirstOrDefaultAsync(i => i.Id == order.AppUserId);
            OrderItemVM? orderItemVM = new OrderItemVM();
            orderItemVM.User = user;
            orderItemVM.Order = order;
            orderItemVM.OrderItems = orderItems;
            return View(orderItemVM);
        }

        public async Task<IActionResult> Products()
        {
            List<Product>? AllProducts = await _context?.Products?
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
            AppUser user = await _usermanager.FindByNameAsync(User.Identity.Name);
            
            UserProductsVM userProductsVM = new UserProductsVM();
            userProductsVM.Products = await _context?.Products?.Where(p => p.AppUserId == user.Id)
                .Include(p => p.ProductImages)
                .ToListAsync();
            return View(userProductsVM);
        }
        public async Task<IActionResult> CreateProduct()
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
        public async Task<IActionResult> CreateProduct(ProductCreateVM? vm)
        {
            var mainCategories = await _context?.Categories?.Where(p => p.ParentId == null).Where(p => p.IsDeleted != true).ToListAsync();
            var altCategories = await _context?.Categories?.Where(c => c.ParentId != null).Where(p => p.IsDeleted != true).ToListAsync();
            ViewBag.altCategories = new SelectList((altCategories).ToList(), "Id", "Name");
            ViewBag.Categories = new SelectList(_context.Categories.Where(c => c.IsDeleted != true).Where(c => c.ParentId == null).ToList(), "Id", "Name");
            ViewBag.Brands = new SelectList(_context?.Brands?.Where(c => c.IsDeleted != true).ToList(), "Id", "Name");
            ViewBag.Tags = new SelectList(_context?.Tags?.Where(t => t.IsDeleted != true).ToList(), "Id", "Name");
            AppUser user = await _usermanager.FindByNameAsync(User.Identity.Name);
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Fill the required contents");
                return View(vm);
            }

            List<ProductImage> Images = new List<ProductImage>();
            foreach (var item in vm.Photos)
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
            Product product = new Product
            {
                Name = vm.Name,
                AppUserId = user.Id,
                Price = vm.Price,
                Description = vm.Description,
                ReleaseDate = vm.ReleaseDate,
                OperationSystem = vm.OperationSystem,
                GPU = vm.GPU,
                Chipset = vm.Chipset,
                Memory = vm.Memory,
                Body = vm.Body,
                Display = vm.Display,
                FrontCamera = vm.FrontCamera,
                RearCamera = vm.RearCamera,
                Battery = vm.Battery,
                Weight = vm.Weight,
                BrandId = vm.BrandId,
                Count = vm.Count,
                Sold = 0,
                Profit = 0,
                CreatedTime = DateTime.Now,
                InStock = true,
                Views = 0
            };

            if (vm?.SubCategory == null)
            {
                product.CategoryId = vm.CategoryId;
            }
            else
            {
                product.CategoryId = vm.SubCategory;
            }


            if (vm.DiscountPercent > 0 && vm.DiscountUntil < DateTime.Now)
            {
                ModelState.AddModelError("DiscountUntil", "You can not set discount date for earlier than now");
                return View(vm);
            }
            else if (vm.DiscountUntil > DateTime.Now && vm.DiscountUntil != null && vm.DiscountPercent > 0)
            {
                product.DiscountUntil = vm.DiscountUntil;
                product.DiscountPercent = vm.DiscountPercent;
                product.DiscountPrice = vm.Price - (vm.Price * vm.DiscountPercent) / 100;
            }
            product.ProductImages = Images;
            product.ProductImages[0].IsMain = true;
            List<ProductTag> tags = new List<ProductTag>();
            foreach (var item in vm.TagId)
            {
                ProductTag tag = new ProductTag();
                tag.TagId = item;
                tag.ProductId = product.Id;
                tags.Add(tag);
            }
            product.ProductTags = tags;
            product.Status = ProductConfirmationStatus.Pending;
            await _context.AddAsync(product);
            await _context.SaveChangesAsync();
            
            return RedirectToAction("Products");
        }
        public async Task<IActionResult> ProductDetail(int id)
        {
            AppUser? CurrentUser = await _usermanager.FindByNameAsync(User.Identity.Name);
            Product? p = await _context?.Products?
                .Where(p => p.AppUserId == CurrentUser.Id)?
                .Include(i => i.ProductImages)?
                .Include(c => c.Category)?
                .Include(t => t.ProductTags)
                .ThenInclude(p => p.Tags)
                .Include(b => b.Brand)
                .FirstOrDefaultAsync(c => c.Id == id);
            if (p == null) return RedirectToAction("error", "home");
            
            DetailVM detailVM = new DetailVM();
            detailVM.Product = p;
            return View(detailVM);
        }
        public async Task<IActionResult> DeleteProduct(int id)
        {
            Product? product = await _context?.Products?.FirstOrDefaultAsync(p=>p.Id==id);
            product.IsDeleted = true;
            product.DeletedAt = DateTime.Now;
            await _context.SaveChangesAsync();
            return RedirectToAction("Products");
        }
        public async Task<IActionResult> EditProduct(int? id)
        {
            List<Product>? AllProducts = await _context?.Products?
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
            var altCategories = _context?.Categories?.Where(c => c.ParentId != null).Where(p => p.IsDeleted != true).ToList();
            ViewBag.Brands = new SelectList(_context?.Brands?.ToList(), "Id", "Name");
            ViewBag.Categories = new SelectList(_context?.Categories?.Where(c => c.IsDeleted != true).Where(c => c.ParentId == null).ToList(), "Id", "Name");
            ViewBag.altCategories = new SelectList((altCategories).ToList(), "Id", "Name");
            ViewBag.Tags = new SelectList(_context?.Tags?.Where(t => t.IsDeleted != true).ToList(), "Id", "Name");
            if (id == null) return RedirectToAction("error", "home");
            AppUser? CurrentUser =await _usermanager.FindByNameAsync(User.Identity.Name);
            Product? p = await _context?.Products?
                .Where(p=>p.AppUserId==CurrentUser.Id)
                .Include(i => i.ProductImages)?
                .Include(c => c.Category)?
                .Include(t => t.ProductTags)?
                .ThenInclude(p => p.Tags)
                .Include(b => b.Brand)
                .FirstOrDefaultAsync(c => c.Id == id);
            if (p == null) return RedirectToAction("error", "home");
            ProductUpdateVM vm = new ProductUpdateVM
            {
                Name=p.Name,
                Price=p.Price,
                Description=p.Description,
                ReleaseDate=p.ReleaseDate,
                OperationSystem=p.OperationSystem,
                GPU=p.GPU,
                Chipset=p.Chipset,
                Memory=p.Memory,
                Body=p.Body,
                Display=p.Display,
                FrontCamera=p.FrontCamera,
                RearCamera=p.RearCamera,
                Battery=p.Battery,
                Weight=p.Weight,
                DiscountPercent=p.DiscountPercent,
                DiscountUntil=p.DiscountUntil,
                Count=p.Count,
                CategoryId=p.CategoryId,
                BrandId=p.BrandId,
                Product=p,
            };
            
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProduct(int id, ProductUpdateVM? product)
        {
            var altCategories = _context?.Categories?.Where(c => c.ParentId != null).Where(p => p.IsDeleted != true).ToList();
            ViewBag.Brands = new SelectList(_context?.Brands?.ToList(), "Id", "Name");
            ViewBag.Categories = new SelectList(_context?.Categories?.Where(c => c.IsDeleted != true).Where(c => c.ParentId == null).ToList(), "Id", "Name");
            ViewBag.altCategories = new SelectList((altCategories).ToList(), "Id", "Name");
            ViewBag.Tags = new SelectList(_context?.Tags?.Where(t => t.IsDeleted != true).ToList(), "Id", "Name");
            if (!ModelState.IsValid)
            {
                return View();
            }
            AppUser? CurrentUser = await _usermanager.FindByNameAsync(User.Identity.Name);
            Product? dbProduct = await _context?.Products?
                .Where(p => p.AppUserId == CurrentUser.Id && p.IsDeleted != true)?
                .Include(p => p.ProductImages)?
                .Include(p => p.ProductTags)?
                .ThenInclude(t => t.Tags)
                .Include(b => b.Brand)
                .Include(c => c.Category)
                .FirstOrDefaultAsync(b => b.Id == id);
            if (dbProduct == null)
            {
                return View(product);
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
            List<Category>? categories = _context?.Categories?.Where(p => p.IsDeleted != true).Where(c => c.ImageUrl != null).ToList();
            for (int i = 0; i < categories.Count; i++)
            {
                if (product.Category == categories[0])
                {
                    dbProduct.CategoryId = dbProduct.CategoryId;
                }
            }




            dbProduct.Name = product.Name;
            dbProduct.Price = product.Price;
            dbProduct.Description = product.Description;
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

            
            dbProduct.Count = product.Count;
            if (product.DiscountPercent==0||product.DiscountPercent==null)
            {
                dbProduct.DiscountUntil = null;
                dbProduct.DiscountPercent = 0;
                dbProduct.DiscountPrice = 0;
            }
            if(product.DiscountPercent>0&&product.DiscountUntil<DateTime.Now)
            {
                ModelState.AddModelError("DiscountUntil", "You can not set discount date for earlier than now");
                return View(product);
            }
            else if (product.DiscountUntil>DateTime.Now&&product.DiscountUntil!=null&&product.DiscountPercent>0)
            {
                dbProduct.DiscountUntil = product.DiscountUntil;
                dbProduct.DiscountPercent = product.DiscountPercent;
                dbProduct.DiscountPrice = product.Price - (product.Price * product.DiscountPercent) / 100;
            }
            dbProduct.BrandId = product.BrandId;
            if (product?.SubCategory==null)
            {
                dbProduct.CategoryId = product.CategoryId;
            }
            else
            {
                dbProduct.CategoryId = product.SubCategory;
            }
            
            dbProduct.LastUpdatedAt = DateTime.Now;
            if (dbProduct.DiscountPercent > 30)
            {
                List<Subscriber>? subscribers = await _context?.Subscribers?.ToListAsync();
                var token = "";
                string subject = "Endirim var!";
                EmailHelper helper = new EmailHelper(_config.GetSection("ConfirmationParam:Email").Value, _config.GetSection("ConfirmationParam:Password").Value);
                foreach (var receiver in subscribers)
                {   
                    token = $"Salam. {dbProduct.Name} məhsulunda {dbProduct.DiscountPercent}% endirim var. +Artıq {dbProduct.Price} deyil, sadəcə {dbProduct.DiscountPrice} AZN\n"+
                        "Məhsula keçid linki https://localhost:44347/Home/detail/{dbProduct.Id}";
                    var emailResult = helper.SendNews(receiver.Email, token, subject);
                    continue;
                }
                string? discountemail = Url.Action("ConfirmEmail", "Account", new
                {
                    token
                }, Request.Scheme);
            }
            if (product.DiscountPercent>0 && product.DiscountPercent>dbProduct.DiscountPercent)
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
                            $"Məhsula keçid linki https://localhost:44347/Home/detail/{dbProduct.Id}";
                        var emailResult = helper.SendNews(appUser.Email, token, subject);
                        
                    string? discountemail = Url.Action("ConfirmEmail", "Account", new
                    {
                        token
                    }, Request.Scheme);
                }
            }
            var roles = await _usermanager.GetRolesAsync(CurrentUser);
            foreach (var item in roles)
            {
                if (item.ToLower().Contains("admin"))
                {
                    dbProduct.Status = ProductConfirmationStatus.Approved;
                }
                else
                {
                    dbProduct.Status = ProductConfirmationStatus.Pending;
                }
            }
            
            await _context.SaveChangesAsync();

            return RedirectToAction("products","user");
        }
        public async Task<IActionResult> EditPictures(int id)
        {
            List<Product>? AllProducts = await _context?.Products?
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
            AppUser CurrentUser = await _usermanager.FindByNameAsync(User.Identity.Name);
            Product? p = await _context?.Products?
                .Where(p => p.AppUserId == CurrentUser.Id)?
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
            List<Product>? AllProducts = await _context?.Products?
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
            AppUser CurrentUser = await _usermanager.FindByNameAsync(User.Identity.Name);
            var product = await _context?.Products?.Where(p => p.AppUserId == CurrentUser.Id).Include(x => x.ProductImages).FirstOrDefaultAsync(x => x.Id == productid);
            if (product == null) return RedirectToAction("error", "home");
            var mainImage = product.ProductImages?.FirstOrDefault(x => x.IsMain);
            mainImage.IsMain = false;

            image.IsMain = true;
            product.LastUpdatedAt = DateTime.Now;
            await _context.SaveChangesAsync();
            if (Returnurl!=null)
            {
                return Redirect(Returnurl);
            }
            return RedirectToAction("index");
        }
        public async Task<IActionResult> RemoveImage(int? imageid, int? productid, string Returnurl)
        {
            List<Product>? AllProducts = await _context?.Products?
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
            Product? product = await _context?.Products?.FirstOrDefaultAsync(p => p.Id == image.ProductId);
            product.LastUpdatedAt = DateTime.Now;
            await _context.SaveChangesAsync();

            return Redirect(Returnurl);
        }
        public IActionResult GetSubCategory(int cid)
        {
            var SubCategory_List = _context?.Categories?.Where(s => s.ParentId == cid).Where(s => s.ParentId != null).ToList();
            var subs = SubCategory_List.Select(c => new { Id = c.Id, Name = c.Name }).ToList();
            return Json(SubCategory_List);
        }
    }
}