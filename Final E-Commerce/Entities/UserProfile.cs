using System.ComponentModel.DataAnnotations;

namespace Final_E_Commerce.Entities
{
    public class UserProfile
    {
        public int Id { get; set; }
        public DateTime Birthdate { get; set; }
        public int? Age { get; set; }
        [DataType(DataType.EmailAddress)]
        
        public string? Hobbies { get; set; }
        public string? EmailForPublic { get; set; }
        public string? FavouriteMusics { get; set; }
        public string? FavouriteBooks { get; set; }
        public string? FavouriteMovies { get; set; }
        public string? AboutMe { get; set; }

        public string? AppUserId { get; set; }
        public AppUser? Appuser { get; set; }
    }
}
