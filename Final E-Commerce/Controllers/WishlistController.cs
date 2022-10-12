﻿using Final_E_Commerce.DAL;
using Final_E_Commerce.Entities;
using Final_E_Commerce.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Final_E_Commerce.Helper;

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
            List<Products>? AllProducts = await _context.Products
                   .Where(p => p.DiscountPercent > 0).ToListAsync();
            foreach (var item in AllProducts)
            {
                if (item.DiscountUntil < DateTime.Now)
                {
                    item.DiscountUntil = null;
                    item.DiscountPercent = 0;
                    item.DiscountPrice = 0;
                    _context.SaveChangesAsync();
                }
            }
            AppUser user = await _usermanager.FindByNameAsync(User.Identity.Name);
            WishlistVM wishlistVM = new WishlistVM();
            List<Wishlist> wihlist= _context.Wishlists.Where(w => w.AppUserId == user.Id).ToList();
            List<Products> listproduct = new List<Products>();
            foreach (var item in wihlist)
            {
                Products product = _context?.Products?.Include(p=>p.ProductImages).FirstOrDefault(p => p.Id == item.ProductId);
                listproduct.Add(product);
            }
            wishlistVM.Products = listproduct;
            return View(wishlistVM);
        }

        public async Task<IActionResult> Add(int id)
        {
            List<Products>? AllProducts = await _context.Products
               .Where(p => p.DiscountPercent > 0).ToListAsync();
            foreach (var item in AllProducts)
            {
                if (item.DiscountUntil < DateTime.Now)
                {
                    item.DiscountUntil = null;
                    item.DiscountPercent = 0;
                    item.DiscountPrice = 0;
                    _context.SaveChangesAsync();
                }
            }
            bool online = false;
            if (User.Identity.IsAuthenticated)
            {
                online = true;
            }
            AppUser user = await _usermanager.FindByNameAsync(User.Identity.Name);
            Products product = await _context?.Products?.FirstOrDefaultAsync(p => p.Id == id);
            Wishlist wishlist = new Wishlist();
            wishlist.ProductId = product.Id;
            wishlist.AppUserId = user.Id;
            bool IsExist = _context.Wishlists.Where(w=>w.AppUserId == user.Id&&w.ProductId==id).Any();
            var obj = new
            {
                exist = true,
                online = online
            };
            if (IsExist)
            {
                return Ok(obj);
            }
            var newobj = new
            {
                exist = false,
                online = online
            };
            await _context.AddAsync(wishlist);
            await _context.SaveChangesAsync();

            return Ok(newobj);
        }

        public async Task<IActionResult> Remove(int id)
        {
            List<Products>? AllProducts = await _context.Products
               .Where(p => p.DiscountPercent > 0).ToListAsync();
            foreach (var item in AllProducts)
            {
                if (item.DiscountUntil < DateTime.Now)
                {
                    item.DiscountUntil = null;
                    item.DiscountPercent = 0;
                    item.DiscountPrice = 0;
                    _context.SaveChangesAsync();
                }
            }
            AppUser user = await _usermanager.FindByNameAsync(User.Identity.Name);
            Products product = await _context?.Products?.FirstOrDefaultAsync(p => p.Id == id);
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
