using System.ComponentModel.DataAnnotations;

namespace Final_E_Commerce.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime OrderedAt { get; set; }

        public double? Price { get; set; }
     
        public string? Address { get; set; }
     
        public string? Email { get; set; }

        public string? Country { get; set; }
     
        public string? City { get; set; }
     
        public string? Firstname { get; set; }
     
        public string? Lastname { get; set; }
     
        public string? Phone { get; set; }
     
        public string? Zipcode { get; set; }
     
        public string? Companyname { get; set; }


        public OrderStatus OrderStatus { get; set; }



        public string? AppUserId { get; set; }
        public AppUser? AppUser { get; set; }

        public List<OrderItem>? OrderItems { get; set; }
    }

    public enum OrderStatus
    {
        Pending=1,
        Approved,
        Refused
    }
}

