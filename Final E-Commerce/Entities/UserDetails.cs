using System.ComponentModel.DataAnnotations;

namespace Final_E_Commerce.Entities
{
	public class UserDetails
	{
		public int Id { get; set; }
		public string? Firstname { get; set; }
        public string? Lastname { get; set; }
        public string? Country { get; set; }
        public string? City { get; set; }
        public string? Street { get; set; }
		public string? PhoneNumber { get; set; }
		public string? Company { get; set; }
		public string? ZipCode { get; set; }
		[DataType(DataType.EmailAddress)]
		public string? Email { get; set; }

		public string? AppUserId { get; set; }
		public AppUser? AppUser { get; set; }
	}
}
