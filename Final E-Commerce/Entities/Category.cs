﻿using System.ComponentModel.DataAnnotations.Schema;

namespace Final_E_Commerce.Entities
{
    public class Category : BaseEntity
    {
        public string? ImageUrl { get; set; }
        public string? Description { get; set; }
        [NotMapped]
        public IFormFile? Images { get; set; }
        public Nullable<int> ParentId { get; set; }
        public Category? Parent { get; set; }
        public List<Category>? Children { get; set; }
        public List<Products>? Products { get; set; }
    }
}