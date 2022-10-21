using Final_E_Commerce.Entities;

namespace Final_E_Commerce.Areas.Admin.ViewModels
{
    public class UserInfoVM
    {
        public string? Id { get; set; }
        public string? Fullname { get; set; }
        public string? Username { get; set; }
        public string? ImageURL { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public bool EmailConfirmed { get; set; }
        public bool IsActivated { get; set; }
        public List<string>? Role { get; set; }
        public string? About { get; set; }
        public List<Orders>? Orders { get; set; }

    }
}
