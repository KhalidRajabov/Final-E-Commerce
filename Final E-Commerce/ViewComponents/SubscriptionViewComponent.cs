using Final_E_Commerce.DAL;
using Final_E_Commerce.Entities;
using Final_E_Commerce.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Final_E_Commerce.ViewComponents
{
    public class SubscriptionViewComponent:ViewComponent
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        public SubscriptionViewComponent(AppDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            SubscriptionVM subscriptionVM = new SubscriptionVM();
            subscriptionVM.Bio = _context?.Bios?.FirstOrDefault();
            if (User.Identity.IsAuthenticated)
            {
                subscriptionVM.User = await _userManager.FindByNameAsync(User.Identity.Name);
            }
            return View(await Task.FromResult(subscriptionVM));
        }
    }
}
