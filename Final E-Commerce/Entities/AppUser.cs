﻿using Microsoft.AspNetCore.Identity;
 
namespace Final_E_Commerce.Entities
{
    public class AppUser:IdentityUser
    {
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
        public string? Fullname { get; set; }
        public string? ProfilePicture { get; set; }
        public bool Subscribed { get; set; }
        public DateTime DateRegistered { get; set; }


        public List<Products>? Products { get; set; }
        public List<Orders>? Orders { get; set; }
        public List<Wishlist>? Wishlist { get; set; }
        public UserDetails? UserDetails { get; set; }
        public UserProfile? UserProfile { get; set; }
        public List<ProductComment>? ProductComment { get; set; }
        public List<Blogs>? Blogs { get; set; }
        public List<BlogComment>? BlogComment { get; set; }
        public List<UserProductRatings>? UserProductRatings { get; set; }
        public List<Messages>? Messages { get; set; }
        public List<UserSubscription>? Subscription { get; set; }
        public List<Communication>? Communications { get; set; }
        public List<ChatMessage>? ChatMessages { get; set; }
        public List<Notification>? Notifications { get; set; }
        //public List<BasketItem> BasketItems { get; set; }
    }
}