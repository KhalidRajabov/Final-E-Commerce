﻿namespace Final_E_Commerce.Entities
{
    public class Wishlist
    {
        public int Id { get; set; }
        public string? AppUserId { get; set; }
        public int ProductId { get; set; }
        public List<Product>? Products { get; set; }
    }
}
