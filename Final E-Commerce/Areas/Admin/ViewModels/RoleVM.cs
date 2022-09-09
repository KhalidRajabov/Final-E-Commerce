using Microsoft.AspNetCore.Identity;

namespace Final_E_Commerce.Areas.Admin.ViewModels
{
    public class RoleVM
    {
        public string? UserId { get; set; }
        public string? Fullname { get; internal set; }
        public List<IdentityRole>? Roles { get; set; }

        public IList<string>? UserRoles { get; set; }
    }
}
