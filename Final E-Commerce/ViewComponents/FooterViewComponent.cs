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
    public class FooterViewComponent:ViewComponent
    {
        private readonly AppDbContext _context;
        

        public FooterViewComponent(AppDbContext context)
        {
            _context = context;
            
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            
            FooterVM footerVM = new FooterVM();
            
            footerVM.Bio = await _context.Bios.FirstOrDefaultAsync();
            footerVM.Categories = await _context.Categories.Where(c=>c.ParentId==null).Take(4).ToListAsync();
            return View(await Task.FromResult(footerVM));
        }

    }
}
