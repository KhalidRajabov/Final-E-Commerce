using Final_E_Commerce.Areas.Admin.ViewModels;
using Final_E_Commerce.DAL;
using Final_E_Commerce.Entities;
using Final_E_Commerce.Helper;
using Final_E_Commerce.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Final_E_Commerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin, SuperAdmin")]
    public class UserController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly AppDbContext _context;
        private IConfiguration _config { get; }

        public UserController
            (UserManager<AppUser> usermanager,
            RoleManager<IdentityRole> roleManager,
            SignInManager<AppUser> signInManager,
            AppDbContext context
,
            IConfiguration config)
        {
            _userManager = usermanager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _context = context;
            _config = config;
        }

        public async Task<IActionResult> Index(string? search)
        {
            var users = search == null ?
                _userManager.Users.ToList() :
                _userManager.Users.Where(users => users.Fullname.ToLower().Contains(search.ToLower()) ||
                users.UserName.ToLower().Contains(search.ToLower()) ||
                users.Email.ToLower().Contains(search.ToLower())).ToList();

            foreach (var user in users)
            {
                var roles =await _userManager.GetRolesAsync(user);
            }
            UserVM userVM = new UserVM
            {
                Users = users
            };
            return View(userVM);
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
            List<Orders>? order = _context?.Orders?.Where(o => o.AppUserId == user.Id).OrderByDescending(o => o.Id).ToList();
            UserInfoVM? userVM = new UserInfoVM();
            var roles = await _userManager.GetRolesAsync(user);
            userVM.Role = roles.ToList();
            userVM.Id= id;
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
            Orders? order = await _context?.Orders?.Where(o => o.Id == id).FirstOrDefaultAsync();
            List<OrderItem> orderItems = await _context?.OrderItems?.Where(o => o.OrderId == order.Id).ToListAsync();
            AppUser? user = await _userManager.Users.FirstOrDefaultAsync(i => i.Id == order.AppUserId);
            OrderItemVM orderItemVM = new OrderItemVM();
            orderItemVM.User = user;
            orderItemVM.Order = order;
            orderItemVM.OrderItems = orderItems;
            return View(orderItemVM);
        }
      
      
        public IActionResult Create()
        {
            RegisterVM registerVM = new RegisterVM();
            return View(registerVM);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RegisterVM registerVM)
        {
            

            if (!ModelState.IsValid) return View();
            AppUser appUser = new AppUser
            {
                Firstname = registerVM.Firstname,
                Lastname = registerVM.Lastname,
                Fullname = $"{registerVM.Firstname} {registerVM.Lastname}",
                UserName = registerVM.Username,
                Email = registerVM.Email,
                ProfilePicture = "default.jpg",
                DateRegistered = DateTime.Now.AddHours(12)
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
            string token = await _userManager.GenerateEmailConfirmationTokenAsync(appUser);
            string? ConfirmationLink = Url.Action("ConfirmEmail", "EmailConfirmation", new { token, Email = registerVM.Email }, Request.Scheme);

            EmailHelper emailHelper = new EmailHelper(_config.GetSection("ConfirmationParam:Email").Value, _config.GetSection("ConfirmationParam:Password").Value);
            var emailResult = emailHelper.SendEmail(registerVM.Email, ConfirmationLink);

            if (!emailResult)
            {
                return View(registerVM);
            }

            UserProfile Profile = new UserProfile();
            UserDetails Detail = new UserDetails();
            Profile.AppUserId = appUser.Id;

            Detail.AppUserId = appUser.Id;
            _context.Add(Profile);
            _context.Add(Detail);
            _context.SaveChanges();
            return RedirectToAction("index");
        }

        public async Task<IActionResult> SearchUsers(string search)
        {
            List<AppUser> users = await _userManager.Users
                .Where(u => u.Fullname.ToLower().Contains(search)||
                u.UserName.ToLower().Contains(search)||
                u.Email.ToLower().Contains(search)||
                u.PhoneNumber.ToLower().Contains(search))
                .ToListAsync();
            UserVM userVM = new UserVM
            {
                Users = users,
            };
            return PartialView("_Users", userVM);
        }

        [HttpGet]
        public async Task<IActionResult> ChangePassword(string userId)
        {
            ViewBag.UserId = userId;
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordVM password)
        {
            AppUser user = await _userManager.FindByIdAsync(password.UserId);
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            IdentityResult result = await _userManager.ResetPasswordAsync(user, token, password.NewPassword);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View();
            }
            var token2 = "";
            string subject = "Şifrə yenilənməsi!";
            EmailHelper helper = new EmailHelper(_config.GetSection("ConfirmationParam:Email").Value, _config.GetSection("ConfirmationParam:Password").Value);

            token2 = $"Salam {user.Fullname}. Parolunuz admin tərəfindən yeniləndi. <br>\n" +
                $"Yeni parol: {password.NewPassword}\n" +
                $"Hesaba keçid üçün <a href='http://rammkhalid-001-site1.itempurl.com/account/login'>Login ol</a>";
            var emailResult = helper.SendNews(user.Email, token2, subject);

            string? discountemail = Url.Action("ConfirmEmail", "Account", new
            {
                token2
            }, Request.Scheme);


            return RedirectToAction("index");
        }

    }
}
