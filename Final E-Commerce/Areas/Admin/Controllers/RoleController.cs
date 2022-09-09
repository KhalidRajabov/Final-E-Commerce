using Final_E_Commerce.Entities;
using Final_E_Commerce.Migrations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Final_E_Commerce.Areas.Admin.Controllers
{
    [Area("AdminPanel")]
    [Authorize(Roles = "Admin, SuperAdmin")]
    public class RoleController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<AppUser> _signInManager;

        public RoleController
            (UserManager<AppUser> usermanager,
            RoleManager<IdentityRole> roleManager,
            SignInManager<AppUser> signInManager
            )
        {
            _userManager = usermanager;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }
        public IActionResult Index()
        {
            return View(_roleManager.Roles.ToList());
        }

        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Create(string role)
        {
            if (string.IsNullOrEmpty(role)) return View();
            var result = await _roleManager.CreateAsync(new IdentityRole { Name = role });
            return RedirectToAction("index");
        }

        public async Task<IActionResult> DeleteRole(string id)
        {
            if (id == null) return NotFound();
            var role = await _roleManager.FindByIdAsync(id);
            await _roleManager.DeleteAsync(role);
            return RedirectToAction("index");

        }
    }
}
