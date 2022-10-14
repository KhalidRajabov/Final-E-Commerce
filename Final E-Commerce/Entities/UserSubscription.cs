namespace Final_E_Commerce.Entities
{
    public class UserSubscription
    {
        public int Id { get; set; }
        public string? SubscriberId { get; set; }
        public string? ProfileId { get; set; }


        public List<AppUser>? AppUsers { get; set; }

    }
}
