﻿using Final_E_Commerce.DAL;
using Final_E_Commerce.Entities;
using Final_E_Commerce.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Final_E_Commerce.Controllers
{
    [Authorize]
    public class WishlistController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _usermanager;

        public WishlistController(UserManager<AppUser> usermanager, AppDbContext context)
        {
            _usermanager = usermanager;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            AppUser user = await _usermanager.FindByNameAsync(User.Identity.Name);
            WishlistVM wishlistVM = new WishlistVM();
            List<Wishlist> wihlist= _context.Wishlists.Where(w => w.AppUserId == user.Id).ToList();
            List<Product> listproduct = new List<Product>();
            foreach (var item in wihlist)
            {
                Product product = _context.Products.Include(p=>p.ProductImages).FirstOrDefault(p => p.Id == item.ProductId);
                listproduct.Add(product);
            }
            wishlistVM.Products = listproduct;
            return View(wishlistVM);
        }
        public async Task<IActionResult> Add(int id)
        {
            AppUser user = await _usermanager.FindByNameAsync(User.Identity.Name);
            Product product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
            Wishlist wishlist = new Wishlist();
            wishlist.ProductId = product.Id;
            wishlist.AppUserId = user.Id;
            bool IsExist = _context.Wishlists.Where(w=>w.AppUserId == user.Id&&w.ProductId==id).Any();
            if (IsExist)
            {
                return Ok($"{product.Name} alread exists in your wishlist");
            }
            await _context.AddAsync(wishlist);
            await _context.SaveChangesAsync();
            return Ok($"{product.Name} added into your wishlist");
        }

        public async Task<IActionResult> Remove(int id)
        {
            AppUser user = await _usermanager.FindByNameAsync(User.Identity.Name);
            Product product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
            Wishlist IsExist = _context.Wishlists.Where(w => w.AppUserId == user.Id && w.ProductId == id).FirstOrDefault();
            if (IsExist==null)
            {
                return Ok($"{product.Name} doesn't exist in your wishlist");
            }
            _context.Wishlists.Remove(IsExist);
            _context.SaveChanges();
            return Ok($"{product.Name} deleted from your wishlist");
        }
    }
}
