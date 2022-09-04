﻿using AutoMapper;
using Final_E_Commerce.DAL;
using Final_E_Commerce.DTO.Bio_DTO;
using Final_E_Commerce.Entities;
using Final_E_Commerce.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.DTO.CategoryDTO;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using WebApi.DTO.Product_DTOs;
using Microsoft.AspNetCore.Identity;

namespace Final_E_Commerce.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _usermanager;
        public HomeController(AppDbContext context, IMapper mapper, UserManager<AppUser> usermanager)
        {
            _context = context;
            _mapper = mapper;
            _usermanager = usermanager;
        }
        public async Task<IActionResult> Index()
        {

            HomeVM homeVM = new HomeVM();
            homeVM.Bio = _context.Bios.FirstOrDefault();
            homeVM.Category = _context.Categories.FirstOrDefault(c=>c.Id==1);
            homeVM.Products = _context.Products.OrderBy(p=>p.Id).Take(3).Include(p=>p.ProductImages).ToList();
            return View(homeVM);
        }
        public async Task<IActionResult> Detail(int? id)
        {
            Product product = _context.Products.Include(p=>p.ProductImages).FirstOrDefault(p=>p.Id==id);
            ViewBag.ExistWishlist = false;
            if (User.Identity.IsAuthenticated)
            {
                AppUser user = await _usermanager.FindByNameAsync(User.Identity.Name);
                bool IsExist = _context.Wishlists.Where(w => w.AppUserId == user.Id && w.ProductId == id).Any();
                if (IsExist)
                {
                    ViewBag.ExistWishlist = true;
                }
            }
            product.Views++;
            await _context.SaveChangesAsync();
            DetailVM detailVM = new DetailVM();
            detailVM.Product = product;
            detailVM.RelatedProducts= _context.Products.Where(c => c.CategoryId == product.CategoryId).ToList();

            return View(detailVM);
        }
    }
}