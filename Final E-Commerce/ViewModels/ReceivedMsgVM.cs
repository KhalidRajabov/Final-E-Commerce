using Final_E_Commerce.Entities;

namespace Final_E_Commerce.ViewModels
{
    public class ReceivedMsgVM
    {
        public AppUser? Sender { get; set; }
        public DateTime LastMessageDate { get; set; }
        public int UnreadMessageCount { get; set; }
    }
}
