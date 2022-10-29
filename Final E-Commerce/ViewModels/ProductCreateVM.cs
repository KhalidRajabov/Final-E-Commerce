using Final_E_Commerce.Entities;
using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations;
using RequiredAttribute = System.ComponentModel.DataAnnotations.RequiredAttribute;

namespace Final_E_Commerce.ViewModels
{
    public class ProductCreateVM
    {
        [Required, MaxLength(100, ErrorMessage = "Can not be more than 15"), MinLength(5, ErrorMessage = "Can not be less than 5")]
        public string? Name { get; set; }
        [Required, Range(5, 10000, ErrorMessage = "Price value must be between 5$ and 10000$")]
        public double? Price { get; set; }
        [Required, MinLength(30, ErrorMessage = "Description can not be less than 30"), MaxLength(500, ErrorMessage = "Description can not be more than 500")]
        public string? Description { get; set; }
        [Required]
        public Nullable<DateTime> ReleaseDate { get; set; }
        public string? OperationSystem { get; set; }
        public string? GPU { get; set; }
        public string? Chipset { get; set; }
        public string? Memory { get; set; }
        public string? Body { get; set; }
        public string? Display { get; set; }
        public string? FrontCamera { get; set; }
        public string? RearCamera { get; set; }
        public string? Battery { get; set; }
        public string? Weight { get; set; }
        public bool IsNew { get; set; }

        public Nullable<double> DiscountPercent { get; set; }
        public Nullable<DateTime> DiscountUntil { get; set; }

        [Required, Range(1, 1000000, ErrorMessage = "Product count must be between 50 and 1 million")]
        public int? Count { get; set; }
        public List<IFormFile>? Photos { get; set; }
        [Required]
        public int CategoryId { get; set; }
        public int SubCategory { get; set; }
        public Category? Category { get; set; }
        [Required]
        public int BrandId { get; set; }
        public Brand? Brand { get; set; }
        public List<int>? TagId { get; set; }
        public List<Tag>? Tags { get; set; }
    }
}
