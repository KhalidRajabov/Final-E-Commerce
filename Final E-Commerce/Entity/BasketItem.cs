﻿namespace Final_E_Commerce.Entity
{
    public class BasketItem
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int Count { get; set; }
        public Product? Product { get; set; }
        public int BasketId { get; set; }
        public Basket? Basket { get; set; }
    }
}
