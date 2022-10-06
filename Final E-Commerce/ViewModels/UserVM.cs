using Final_E_Commerce.Entities;

namespace Final_E_Commerce.ViewModels
{
    public class UserVM
    {
        public AppUser? User { get; set; }
        public UserProfile? UserProfile { get; set; }
        public List<AppUser>? Users { get; set; }
    }
}
