﻿using Final_E_Commerce.DAL;
using Final_E_Commerce.Entities;
using Final_E_Commerce.Helper;
using Final_E_Commerce.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace Final_E_Commerce.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _usermanager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<AppUser> _signInManager;
        private IConfiguration _config { get; }

        public AccountController
            (UserManager<AppUser> usermanager,
            RoleManager<IdentityRole> roleManager,
            SignInManager<AppUser> signInManager,
            IConfiguration config,
            AppDbContext context)
        {
            _usermanager = usermanager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _config = config;
            _context = context;
        }

        public IActionResult Register()
        {
            if (User.Identity.IsAuthenticated) return RedirectToAction("index", "home");
            RegisterVM registerVM = new RegisterVM();
            return View(registerVM);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterVM? registerVM)
        {
            if (User.Identity.IsAuthenticated) return RedirectToAction("index", "home");

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

            IdentityResult result = await _usermanager.CreateAsync(appUser, registerVM.Password);
            if (!result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
                return View(registerVM);
            }
            await _usermanager.AddToRoleAsync(appUser, "Member");
            string registertoken = await _usermanager.GenerateEmailConfirmationTokenAsync(appUser);
            string token = await _usermanager.GenerateEmailConfirmationTokenAsync(appUser);
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
            return RedirectToAction("login", "account");
        }

        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated) return RedirectToAction("index", "home");
            LoginVM loginVM = new LoginVM();
            return View(loginVM);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM loginvm, string ReturnUrl)
        {
            if (User.Identity.IsAuthenticated) return RedirectToAction("index", "home");
            AppUser appUser = await _usermanager.FindByEmailAsync(loginvm.Email);
            if (appUser == null)
            {
                ModelState.AddModelError("", "Email or password is wrong");
                return View(loginvm);
            }


            var roles = await _usermanager.GetRolesAsync(appUser);
            foreach (var item in roles)
            {
                if (item.ToLower() == "ban" || roles == null)
                {
                    ModelState.AddModelError("", "You are banned");
                    await _signInManager.SignOutAsync();
                    return View(loginvm);
                }

                else if (item.ToLower().Contains("admin"))
                {
                    if (!appUser.EmailConfirmed)
                    {
                        ModelState.AddModelError("", "Email was sent to your email. Please confirm it");
                        return View();
                    }
                    SignInResult test = await _signInManager.PasswordSignInAsync(appUser, loginvm.Password, loginvm.RememberMe, true);
                    if (!test.Succeeded)
                    {
                        ModelState.AddModelError("", "Email or password is wrong");
                        return View(loginvm);
                    }
                    else if (ReturnUrl != null)
                    {
                        return Redirect(ReturnUrl);
                    }
                    return RedirectToAction("index", "dashboard", new { Area = "Admin" });
                }
            }
            SignInResult result = await _signInManager.PasswordSignInAsync(appUser, loginvm.Password, loginvm.RememberMe, true);
            if (result.IsLockedOut)
            {
                ModelState.AddModelError("", "Your account is blocked");
                return View(loginvm);
            }
            
            if (!appUser.EmailConfirmed)
            {
                ModelState.AddModelError("", "Email was sent to your email. Please confirm it");
                return View(loginvm);
            }


            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Email or password is wrong");
                return View(loginvm);
            }


            await _signInManager.SignInAsync(appUser, isPersistent: true);
            if (ReturnUrl != null)
            {
                return Redirect(ReturnUrl);
            }

            return RedirectToAction("index", "home");

        }


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ExternalLoginConfirmation(ExternalLoginVM model, string? returnUrl=null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            if (ModelState.IsValid)
            {
                var info = await _signInManager.GetExternalLoginInfoAsync();
                if (info==null)
                {
                    return RedirectToAction("error", "home");
                }
                var user = new AppUser { UserName = model.Username};
                user.Email = info.Principal.FindFirstValue(ClaimTypes.Email);
                user.EmailConfirmed = true;
                user.Firstname = info.Principal.FindFirstValue(ClaimTypes.GivenName);
                user.Lastname = info.Principal.FindFirstValue(ClaimTypes.Surname);
                user.Fullname = $"{user.Firstname} {user.Lastname}";
                user.ProfilePicture = "default.jpg";
                user.DateRegistered = DateTime.Now.AddHours(12);
                var result = await _usermanager.CreateAsync(user);
                if (result.Succeeded)
                    await _usermanager.AddToRoleAsync(user, "Member");
                {
                    UserProfile Profile = new UserProfile();
                    UserDetails Detail = new UserDetails();
                    Profile.AppUserId = user.Id;
                    Detail.AppUserId = user.Id;
                    _context.Add(Profile);
                    _context.Add(Detail);
                    _context.SaveChanges();
                    result = await _usermanager.AddLoginAsync(user, info);
                    if (result.Succeeded)
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        await _signInManager.UpdateExternalAuthenticationTokensAsync(info);
                        return LocalRedirect(returnUrl);
                    }
                }
            }
            ViewData["ReturnUrl"] = returnUrl;  
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult ExternalLogin(string provider, string? returnUrl = null)
        {
            var redirect = Url.Action("ExternalLoginCallback", "account", new { ReturnUrl = returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirect);
            return Challenge(properties, provider);
        }
        [HttpGet]
        public async Task<IActionResult> ExternalLoginCallback(string? returnUrl = null, string? remoteError = null)
        {
            if (remoteError!=null)
            {
                ModelState.AddModelError(string.Empty,"Error from external provider" );
                return RedirectToAction("login");
            }
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info==null)
            {
                return RedirectToAction("login");
            }
            var user = await _usermanager.FindByEmailAsync(info.Principal.FindFirstValue(ClaimTypes.Email));
            if (user!=null)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("index", "home");
            };
            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false);
            if (result.Succeeded)
            {
                await _signInManager.UpdateExternalAuthenticationTokensAsync(info);
                return LocalRedirect(returnUrl);
            }
            else
            {
                ViewData["ReturnUrl"] = returnUrl;
                ViewData["ProviderDisplayName"] = info.ProviderDisplayName;
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);
                return View("ExternalEmailConfirmation");
            }

        }


        public async Task<IActionResult> Logout(string returnurl)
        {
            await _signInManager.SignOutAsync();
            if (returnurl != null)
            {
                return Redirect(returnurl);
            }
            return RedirectToAction("index", "home");
        }

        /*public async Task CreateRole()
        {

            if (!await _roleManager.RoleExistsAsync("Admin"))
            {
                await _roleManager.CreateAsync(new IdentityRole { Name = "Admin" });
            }
            if (!await _roleManager.RoleExistsAsync("Member"))
            {
                await _roleManager.CreateAsync(new IdentityRole { Name = "Member" });
            }
            if (!await _roleManager.RoleExistsAsync("SuperAdmin"))
            {
                await _roleManager.CreateAsync(new IdentityRole { Name = "SuperAdmin" });
            }
            if (!await _roleManager.RoleExistsAsync("Ban"))
            {
                await _roleManager.CreateAsync(new IdentityRole { Name = "Ban" });
            }
            if (!await _roleManager.RoleExistsAsync("Editor"))
            {
                await _roleManager.CreateAsync(new IdentityRole { Name = "Editor" });
            }
            if (!await _roleManager.RoleExistsAsync("Moderator"))
            {
                await _roleManager.CreateAsync(new IdentityRole { Name = "Moderator" });
            }
        }*/

    }
}
