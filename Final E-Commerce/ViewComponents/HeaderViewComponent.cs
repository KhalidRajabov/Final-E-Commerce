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
            ViewBag.Products = "";
            int TotalCount = 0;
            double TotalPrice = 0;
            string basket = Request.Cookies[$"basket{username}"];
            if (basket != null)
            {
                List<BasketVM> products = JsonConvert.DeserializeObject<List<BasketVM>>(basket);
                foreach (var item in products)
                {
                    TotalCount += item.ProductCount;
                }
                foreach (var item in products)
                {
                    TotalPrice += item.Price * item.ProductCount;
                }
            }
            ViewBag.BasketCount = TotalCount;
            ViewBag.TotalPrice = TotalPrice;
            HeaderVM hdVM = new HeaderVM();
            Bio bio = await _context.Bios.FirstOrDefaultAsync();
            BioReturnDTO biodto = new BioReturnDTO();
            hdVM.Bio = _mapper.Map<BioReturnDTO>(bio);
            return View(await Task.FromResult(hdVM));
        }
    }
}
