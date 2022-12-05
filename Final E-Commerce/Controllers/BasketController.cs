using Final_E_Commerce.DAL;
using Final_E_Commerce.Entities;
using Final_E_Commerce.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
namespace Final_E_Commerce.Controllers
{
    [Authorize]
    public class BasketController : Controller
    {
        private readonly AppDbContext? _context;
        private readonly SignInManager<AppUser>? _signInManager;
        private readonly UserManager<AppUser>? _usermanager;

        public BasketController(AppDbContext? context,
        SignInManager<AppUser>? signInManager,
        UserManager<AppUser>? userManager)
        {
            _context = context;
            _signInManager = signInManager;
            _usermanager = userManager;
        }
        [Authorize]
        public async Task<IActionResult> Index()
        {
            List<Products>? AllProducts = _context?.Products?
               .Where(p => p.DiscountPercent > 0).ToList();
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
            string? username = "";
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("login", "account");
            }
            else
            {
                username = User.Identity.Name;

                AppUser user = await _usermanager.FindByNameAsync(username);
                var roles = await _usermanager.GetRolesAsync(user);
                foreach (var item in roles)
                {
                    if (item.ToLower() == "ban" || roles == null)
                    {
                        await _signInManager.SignOutAsync();
                        return RedirectToAction("error", "home");
                    }
                }
            }
            string? basket = Request.Cookies[$"basket{username}"];
            List<BasketVM>? basketVM;
            if (basket != null)
            {
                basketVM = JsonConvert.DeserializeObject<List<BasketVM>>(basket);
                foreach (var item in basketVM)
                {
                    Products? dbProducts = _context?.Products?.Include(pi => pi.ProductImages).FirstOrDefault(x => x.Id == item.Id);
                    item.Name = dbProducts.Name;
                    if (dbProducts.DiscountPercent > 0)
                    {
                        item.Price = dbProducts.DiscountPrice;
                    }
                    else
                    {
                        item.Price = dbProducts.Price;
                    }
                    foreach (var image in dbProducts.ProductImages)
                    {
                        if (image.IsMain)
                        {
                            item.ImageUrl = image.ImageUrl;
                        }
                    }
                }
            }
            else
            {
                basketVM = new List<BasketVM>();
            }
            return View(basketVM);
        }


        public async Task<IActionResult> AddItem(int? id, int? quantity)
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
            AppUser user = await _usermanager.FindByNameAsync(User.Identity.Name);
            var roles = await _usermanager.GetRolesAsync(user);
            foreach (var item in roles)
            {
                if (item.ToLower() == "ban" || roles == null)
                {
                    await _signInManager.SignOutAsync();
                    return RedirectToAction("error", "home");
                }
            }
            string? username = "";
            bool? online = false;
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("login", "account");
            }
            else
            {
                username = User.Identity.Name;
                online = true;
            }
            if (id == null)
                if (id == null) return RedirectToAction("error", "home");
            Products? dbProduct = await _context?.Products?.Include(p => p.ProductImages).FirstOrDefaultAsync(p => p.Id == id);
            if (dbProduct == null) return RedirectToAction("error", "home");
            List<BasketVM>? products;
            if (Request.Cookies[$"basket{username}"] == null)
            {
                products = new List<BasketVM>();
            }
            else
            {
                products = JsonConvert.DeserializeObject<List<BasketVM>>(Request.Cookies[$"basket{username}"]);
            }
            int? ProductCount = 0;
            BasketVM? IsExist = products.Find(x => x.Id == id);
            if (IsExist == null)
            {
                BasketVM basketvm = new BasketVM
                {
                    Id = dbProduct.Id,
                    ProductCount = 1,
                    Name = dbProduct.Name,

                };
                ProductCount = basketvm.ProductCount;
                if (dbProduct.DiscountPercent > 0)
                {
                    basketvm.Price = dbProduct.DiscountPrice;
                }
                else
                {
                    basketvm.Price = dbProduct.Price;
                }
                products.Add(basketvm);
            }
            else if (quantity >= 0)
            {
                IsExist.ProductCount += quantity;
                ProductCount = IsExist.ProductCount;
            }
            else
            {
                IsExist.ProductCount++;
                ProductCount = IsExist.ProductCount;
            }

            double? price = 0;
            double? count = 0;


            foreach (var product in products)
            {
                price += product.Price * product.ProductCount;
                count += product.ProductCount;
            }
            string? mainimage = dbProduct.ProductImages[0].ImageUrl;
            Response.Cookies.Append($"basket{username}", JsonConvert.SerializeObject(products), new CookieOptions { MaxAge = TimeSpan.FromDays(100) });
            /*foreach (var item in dbProduct.ProductImages)
            {
                if (item.IsMain)
                {
                    mainimage = item.ImageUrl;
                }
            }*/
            var obj = new
            {
                Price = price,
                Count = count,
                online = online,
                Name = dbProduct.Name,
                Image = mainimage,
                itemprice = dbProduct.Price,
                productcount = ProductCount,
                id = dbProduct.Id
            };
            //obj data-id ile baghlidir. response "obj" obyektidir,
            //Ok'in icnde return edilmelidir ki API'de response gorsun
            return Ok(obj);
        }



        public async Task<IActionResult> RemoveItem(int? id, string returnurl)
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
            AppUser user = await _usermanager.FindByNameAsync(User.Identity.Name);
            var roles = await _usermanager.GetRolesAsync(user);
            foreach (var item in roles)
            {
                if (item.ToLower() == "ban" || roles == null)
                {
                    await _signInManager.SignOutAsync();
                    return RedirectToAction("error", "home");
                }
            }
            string? username = "";
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("login", "account");
            }
            else
            {
                username = User.Identity.Name;
            }
            if (id == null) return RedirectToAction("error", "home");
            string? basket = Request.Cookies[$"basket{username}"];
            List<BasketVM>? products = JsonConvert.DeserializeObject<List<BasketVM>>(basket);
            BasketVM? dbProduct = products.Find(p => p.Id == id);
            if (dbProduct == null) return RedirectToAction("error", "home");


            products.Remove(dbProduct);
            Response.Cookies.Append($"basket{username}", JsonConvert.SerializeObject(products), new CookieOptions { MaxAge = TimeSpan.FromDays(100) });

            double? subtotal = 0;
            int? basketCount = 0;

            if (products.Count > 0)
            {
                foreach (BasketVM item in products)
                {
                    subtotal += item.Price * dbProduct.ProductCount;
                    basketCount += item.ProductCount;
                }
            }

            var obj = new
            {
                Price = $"(${subtotal})",
                Count = basketCount,
            };
            if (returnurl != null)
            {
                return Redirect(returnurl);
            }
            return Ok(obj);
        }




        public async Task<IActionResult> Minus(int? id)
        {
            List<Products>? AllProducts = await _context.Products?
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
            AppUser user = await _usermanager.FindByNameAsync(User.Identity.Name);
            var roles = await _usermanager.GetRolesAsync(user);
            foreach (var item in roles)
            {
                if (item.ToLower() == "ban" || roles == null)
                {
                    await _signInManager.SignOutAsync();
                    return RedirectToAction("error", "home");
                }
            }
            string? username = "";
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("login", "account");
            }
            else
            {
                username = User.Identity.Name;
            }
            if (id == null) return RedirectToAction("error", "home");
            string? basket = Request.Cookies[$"basket{username}"];
            List<BasketVM>? products = JsonConvert.DeserializeObject<List<BasketVM>>(basket);
            BasketVM? dbproducts = products.Find(p => p.Id == id);
            if (dbproducts == null) return RedirectToAction("error", "home");


            double? subtotal = 0;
            int? basketCount = 0;
            int? ProductCount = 0;

            if (dbproducts.ProductCount > 1)
            {
                dbproducts.ProductCount--;
                ProductCount = dbproducts.ProductCount;
                Response.Cookies.Append($"basket{username}", JsonConvert.SerializeObject(dbproducts));
            }
            else
            {
                products.Remove(dbproducts);


                List<BasketVM> productsNew = products.FindAll(p => p.Id != id);

                Response.Cookies.Append($"basket{username}", JsonConvert.SerializeObject(productsNew));

                foreach (BasketVM pr in productsNew)
                {
                    subtotal += pr.Price * pr.ProductCount;
                    basketCount += pr.ProductCount;
                }

                var obje = new
                {
                    count = 0,
                    price = subtotal,
                    main = basketCount,
                    productcount = ProductCount
                };

                return Ok(obje);
            }
            Response.Cookies.Append($"basket{username}", JsonConvert.SerializeObject(products), new CookieOptions
            {
                MaxAge = TimeSpan.FromDays(100)
            });


            foreach (var product in products)
            {
                subtotal += product.Price * product.ProductCount;
                basketCount += product.ProductCount;
            }

            var obj = new
            {
                Price = subtotal,
                Count = dbproducts.ProductCount,
                main = basketCount,
                itemTotal = dbproducts.Price * dbproducts.ProductCount,
                productcount = ProductCount
            };
            return Ok(obj);
        }



        public async Task<IActionResult> Plus(int? id)
        {
            List<Products>? AllProducts = await _context.Products?
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
            AppUser user = await _usermanager.FindByNameAsync(User.Identity.Name);
            var roles = await _usermanager.GetRolesAsync(user);
            foreach (var item in roles)
            {
                if (item.ToLower() == "ban" || roles == null)
                {
                    await _signInManager.SignOutAsync();
                    return RedirectToAction("error", "home");
                }
            }
            string? username = "";
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("login", "account");
            }
            else
            {
                username = User.Identity.Name;
            }
            if (id == null) return RedirectToAction("error", "home");
            string? basket = Request.Cookies[$"basket{username}"];
            List<BasketVM>? products;
            products = JsonConvert.DeserializeObject<List<BasketVM>>(basket);
            BasketVM? dbproducts = products.Find(p => p.Id == id);
            if (dbproducts == null) return RedirectToAction("error", "home");
            dbproducts.ProductCount++;
            int? ProductCount = dbproducts.ProductCount;
            Response.Cookies.Append($"basket{username}", JsonConvert.SerializeObject(products), new CookieOptions
            {
                MaxAge = TimeSpan.FromDays(100)
            });

            double? price = 0;
            double? count = 0;

            foreach (var product in products)
            {
                price += product.Price * product.ProductCount;
                count += product.ProductCount;
            }
            var obj = new
            {
                Price = price,
                Count = count,
                main = dbproducts.ProductCount,
                itemTotal = dbproducts.Price * dbproducts.ProductCount,
                id = id,
                productcount = ProductCount
            };
            return Ok(obj);
        }



        public async Task<IActionResult> CheckOut()
        {

            List<Products>? AllProducts = await _context.Products?
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
            AppUser AppUser = await _usermanager.FindByNameAsync(User.Identity.Name);
            var roles = await _usermanager.GetRolesAsync(AppUser);
            foreach (var item in roles)
            {
                if (item.ToLower() == "ban" || roles == null)
                {
                    await _signInManager.SignOutAsync();
                    return RedirectToAction("error", "home");
                }
            }
            string? username = "";
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("login", "account");
            }
            else
            {
                username = User.Identity.Name;
            }
            ViewBag.BasketCount = 0;
            ViewBag.TotalPrice = 0;
            string? basket = Request.Cookies[$"basket{username}"];
            List<BasketVM>? products = new List<BasketVM>();
            if (basket != null)
            {
                products = JsonConvert.DeserializeObject<List<BasketVM>>(basket);
            }
            if (products == null || products.Count <= 0)
            {
                return RedirectToAction("index", "home");
            }
            else
            {
                foreach (var item in products)
                {
                    ViewBag.BasketCount += item.ProductCount;
                    ViewBag.TotalPrice += item.Price * item.ProductCount;
                    Products? dbProducts = _context?.Products?.Include(pi => pi.ProductImages).FirstOrDefault(x => x.Id == item.Id);
                    item.Name = dbProducts.Name;
                    if (dbProducts.DiscountPercent > 0)
                    {
                        item.Price = dbProducts.DiscountPrice;
                    }
                    else
                    {
                        item.Price = dbProducts.Price;
                    }
                    foreach (var image in dbProducts.ProductImages)
                    {
                        if (image.IsMain)
                        {
                            item.ImageUrl = image.ImageUrl;
                        }
                    }
                }
            }
            AppUser user = await _usermanager.FindByNameAsync(User.Identity.Name);
            UserDetails? userDetails = await _context.UserDetails?.FirstOrDefaultAsync(u => u.AppUserId == user.Id);

            CheckoutVM checkoutVM = new CheckoutVM();
            checkoutVM.Firstname = userDetails.Firstname;
            checkoutVM.Lastname = userDetails.Lastname;
            checkoutVM.Country = userDetails.Country;
            checkoutVM.City = userDetails.City;
            checkoutVM.ZipCode = userDetails.ZipCode;
            checkoutVM.Email = userDetails.Email;
            checkoutVM.Address = userDetails.Street;
            checkoutVM.PhoneNumber = userDetails.PhoneNumber;
            checkoutVM.Company = userDetails.Company;
            checkoutVM.Baskets = products;

            return View(checkoutVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Order(CheckoutVM newOrder)
        {
            List<Products>? AllProducts = await _context.Products?
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
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Fill all forms");
                return View();
            }
            if (User.Identity.IsAuthenticated)
            {
                AppUser user = await _usermanager.FindByNameAsync(User.Identity.Name);
                Orders order = new Orders();
                order.Firstname = newOrder.Firstname;
                order.Lastname = newOrder.Lastname;
                order.City = newOrder.City;
                order.Country = newOrder.Country;
                order.Email = newOrder.Email;
                order.Phone = newOrder.PhoneNumber;
                order.Zipcode = newOrder.ZipCode;
                order.Address = newOrder.Address;
                order.Companyname = newOrder.Company;
                order.OrderedAt = DateTime.Now.AddHours(12);
                order.AppUserId = user.Id;
                order.OrderStatus = OrderStatus.Pending;


                string? userName = User.Identity.Name;


                List<BasketVM>? basketProducts = JsonConvert.DeserializeObject<List<BasketVM>>(Request.Cookies[$"basket{userName}"]);
                if (basketProducts == null)
                {
                    return RedirectToAction("index");
                }
                ViewBag.Products = basketProducts;
                List<OrderItem>? orderItems = new List<OrderItem>();
                double? total = 0;
                foreach (var basketProduct in basketProducts)
                {
                    Products? dbProduct = await _context.Products.FindAsync(basketProduct.Id);
                    if (basketProduct.ProductCount > dbProduct.Count)
                    {
                        TempData["fail"] = "Satış uğursuzdur..";
                        return RedirectToAction("Index");
                    }
                    OrderItem orderItem = new OrderItem();
                    orderItem.ProductId = dbProduct.Id;
                    orderItem.Count = basketProduct.ProductCount;
                    orderItem.OrderId = order.Id;
                    orderItem.Name = basketProduct.Name;
                    if (dbProduct.DiscountUntil > DateTime.Now.AddHours(12) && dbProduct.DiscountPercent > 0)
                    {
                        orderItem.Price = dbProduct.DiscountPrice;
                        orderItem.TotalPrice = dbProduct.DiscountPrice * basketProduct.ProductCount;
                        total += basketProduct.ProductCount * dbProduct.DiscountPrice;
                    }
                    else
                    {
                        orderItem.Price = dbProduct.Price;
                        orderItem.TotalPrice = dbProduct.Price * basketProduct.ProductCount;
                        total += basketProduct.ProductCount * dbProduct.Price;
                    }
                    orderItems.Add(orderItem);
                    dbProduct.Count = dbProduct.Count - basketProduct.ProductCount;
                }

                order.OrderItems = orderItems;
                order.Price = total;
                //List<string> roles = new List<string>();
                var roles = await _usermanager.GetRolesAsync(user);
                foreach (var item in roles)
                {
                    if (item.ToLower().Contains("admin"))
                    {
                        order.OrderStatus = OrderStatus.Approved;
                    }
                    else
                    {
                        order.OrderStatus = OrderStatus.Pending;
                    }
                }

                await _context.AddAsync(order);
                await _context.SaveChangesAsync();

                TempData["success"] = "Satış uğurla başa çatdı..";
                return RedirectToAction("index");
            }
            else
            {
                return RedirectToAction("login", "account");
            }
        }
    }
}