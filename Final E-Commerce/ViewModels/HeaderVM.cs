using Final_E_Commerce.Entities;

namespace Final_E_Commerce.ViewModels
{
    public class HeaderVM
    {
        public Bio? Bio { get; set; }
        public List<BasketVM>? BasketProducts { get; set; }
        public int UnreadNotificationCount { get; set; }
        public List<Notification>? Notifications { get; set; }
    }
}
