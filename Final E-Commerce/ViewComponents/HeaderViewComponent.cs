using AutoMapper;
using Final_E_Commerce.DAL;
using Final_E_Commerce.DTO.Bio_DTO;
using Final_E_Commerce.Entities;
using Final_E_Commerce.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Final_E_Commerce.ViewComponents
{
    public class HeaderViewComponent:ViewComponent
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;
        public HeaderViewComponent(AppDbContext context, IMapper mapper, UserManager<AppUser> userManager)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            string username = "";
            if (User.Identity.IsAuthenticated)
            {
                username = User.Identity.Name;
            }
            ViewBag.UserId = "";
            ViewBag.UserRole = "";
            ViewBag.User = "Login";
            if (User.Identity.IsAuthenticated)
            {
                AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
                ViewBag.User = user.UserName;
                ViewBag.UserId = user.Id;
                var roles = (await _userManager.GetRolesAsync(user));
                foreach (var item in roles)
                {
                    if (item.ToLower().Contains("admin"))
                    {
                        ViewBag.UserRole = "admin";
                    }
                }

            }
            ViewBag.BasketCount = 0;
            ViewBag.TotalPrice = 0;
            HeaderVM hdVM = new HeaderVM();
            string basket = Request.Cookies[$"basket{username}"];


            List<BasketVM> basketVM;
            if (basket != null)
            {
                basketVM = JsonConvert.DeserializeObject<List<BasketVM>>(basket);
                foreach (var item in basketVM)
                {
                    ViewBag.BasketCount += item.ProductCount;
                    ViewBag.TotalPrice += item.Price * item.ProductCount;
                    Product? dbProducts = _context?.Products?.Include(pi => pi.ProductImages).FirstOrDefault(x => x.Id == item.Id);
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
            hdVM.BasketProducts = basketVM;
            hdVM.Bio = await _context.Bios.FirstOrDefaultAsync();
            return View(await Task.FromResult(hdVM));
        }
    }
}