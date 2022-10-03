namespace Final_E_Commerce.ViewModels
{
    public class UserProfileVM
    {
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
        public string? Username { get; set; }
        public string? PhoneNumber { get; set; }

        public string? Hobbies { get; set; }
        public string? EmailForPublic { get; set; }
        public string? FavouriteMusics { get; set; }
        public string? FavouriteBooks { get; set; }
        public string? FavouriteMovies { get; set; }
        public string? AboutMe { get; set; }
        public DateTime? Birthdate { get; set; }

        public IFormFile? Photo { get; set; }

    }
}
