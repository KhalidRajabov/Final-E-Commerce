using Final_E_Commerce.Entities;
using System.ComponentModel.DataAnnotations;

namespace Final_E_Commerce.ViewModels
{
    public class SubscriptionVM
    {
        public Bio? Bio { get; set; }
        public AppUser? User { get; set; }
    }
}
