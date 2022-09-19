using Final_E_Commerce.Areas.Admin.ViewModels;
using Final_E_Commerce.DAL;
using Final_E_Commerce.Entities;
using Final_E_Commerce.Migrations;
using Final_E_Commerce.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Final_E_Commerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin, SuperAdmin")]
    public class UserManagerController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly AppDbContext _context;

        public UserManagerController
            (UserManager<AppUser> usermanager,
            RoleManager<IdentityRole> roleManager,
            SignInManager<AppUser> signInManager,
            AppDbContext context
            )
        {
            _userManager = usermanager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _context = context;
        }

        public IActionResult Index(string search)
        {
            var users = search == null ?
                _userManager.Users.ToList() :
                _userManager.Users.Where(users => users.Fullname.ToLower().Contains(search.ToLower()) ||
                users.UserName.ToLower().Contains(search.ToLower()) ||
                users.Email.ToLower().Contains(search.ToLower())).ToList();
            return View(users);
        }

        public async Task<IActionResult> Edit(string id)
        {
            if (id == null) return RedirectToAction("error", "home");
            AppUser user = await _userManager.FindByIdAsync(id);
            if (user == null) return RedirectToAction("error", "home");
            var userRoles = await _userManager.GetRolesAsync(user);
            var dbRoles = _roleManager.Roles.ToList();
            RoleVM rolevm = new RoleVM
            {
                Fullname = user.Fullname,
                Roles = dbRoles,
                UserRoles = userRoles,
                UserId = user.Id
            };
            return View(rolevm);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(List<string> roles, string id)
        {
            if (id == null) return RedirectToAction("error", "home");
            AppUser user = await _userManager.FindByIdAsync(id);
            if (user == null) return RedirectToAction("error", "home");
            var userRoles = await _userManager.GetRolesAsync(user);
            var addedRoles = roles.Except(userRoles);
            var removedRoles = userRoles.Except(roles);
            await _userManager.AddToRolesAsync(user, addedRoles);
            await _userManager.RemoveFromRolesAsync(user, removedRoles);
            return RedirectToAction("index");
        }
        public async Task<IActionResult> Detail(string? id)
        {
            if (id == null) return RedirectToAction("error", "home");
            AppUser user = await _userManager.FindByIdAsync(id);
            if (user == null) return RedirectToAction("error", "home");
            List<Order>? order = _context.Orders.Where(o => o.AppUserId == user.Id).OrderByDescending(o => o.Id).ToList();
            UserInfoVM? userVM = new UserInfoVM();
            var roles = await _userManager.GetRolesAsync(user);
            userVM.Role = roles.ToList();
            userVM.Fullname = user.Fullname;
            userVM.Email = user.Email;
            userVM.Phone = user.PhoneNumber;

            userVM.Username = user.UserName;
            userVM.EmailConfirmed = user.EmailConfirmed;
            userVM.Orders = order;

            return View(userVM);
        }
        public async Task<IActionResult> OrderDetail(int id)
        {
            Order? order = await _context.Orders.Where(o => o.Id == id).FirstOrDefaultAsync();
            List<OrderItem> orderItems = await _context.OrderItems.Where(o => o.OrderId == order.Id).ToListAsync();
            AppUser? user = await _userManager.Users.FirstOrDefaultAsync(i => i.Id == order.AppUserId);
            OrderItemVM orderItemVM = new OrderItemVM();
            orderItemVM.User = user;
            orderItemVM.Order = order;
            orderItemVM.OrderItems = orderItems;
            return View(orderItemVM);
        }
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null) return RedirectToAction("error", "home");
            AppUser user = await _userManager.FindByIdAsync(id);
            if (user == null) return RedirectToAction("error", "home");
            UserInfoVM userVM = new UserInfoVM();
            var roles = await _userManager.GetRolesAsync(user);
            userVM.Role = roles.ToList();
            userVM.Fullname = user.Fullname;
            userVM.Email = user.Email;
            userVM.Phone = user.PhoneNumber;

            userVM.Username = user.UserName;
            userVM.Id = user.Id;
            return View(userVM);
        }
        public async Task<IActionResult> DeleteUser(string id)
        {
            if (id == null) return RedirectToAction("error", "home");
            AppUser user = await _userManager.FindByIdAsync(id);
            if (user == null) return RedirectToAction("error", "home");
            await _userManager.DeleteAsync(user);
            return RedirectToAction("index");
        }
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RegisterVM registerVM)
        {


            if (!ModelState.IsValid) return View();
            AppUser appUser = new AppUser
            {
                Fullname = $"{registerVM.Firstname} {registerVM.Lastname}",
                UserName = registerVM.Username,
                Email = registerVM.Email

            };
            IdentityResult result = await _userManager.CreateAsync(appUser, registerVM.Password);
            if (!result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
                return View(registerVM);
            }
            await _userManager.AddToRoleAsync(appUser, "Member");
            return RedirectToAction("index");
        }
        /*public async Task<IActionResult> Index()
        {
            List<AppUser> users = _userManager.Users.ToList();
            List<UserVM> usersVM = new List<UserVM>();
            foreach (AppUser user in users)
            {
                UserVM userVM = new UserVM
                {
                    Id = user.Id,
                    Name = user.FullName,
                    Username = user.UserName,
                    Email = user.Email,
                    IsActivated = user.IsActivated,
                    Role = (await _userManager.GetRolesAsync(user))[0]
                };
            usersVM.Add(userVM);
            }
            return View(usersVM);
        }
        public async Task<IActionResult> Activate(string? id)
        {
            if (id == null) return RedirectToAction("error", "home");
            AppUser user = await _userManager.FindByIdAsync(id);
            if (user == null) return RedirectToAction("error", "home");
            return View(user);
        }*/
    }
}
