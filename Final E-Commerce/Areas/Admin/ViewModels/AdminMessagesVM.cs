using Final_E_Commerce.Entities;
using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations;
using RequiredAttribute = System.ComponentModel.DataAnnotations.RequiredAttribute;

namespace Final_E_Commerce.Areas.Admin.ViewModels
{
    public class AdminMessagesVM
    {
        public List<Message>? Messages { get; set; }
        public Message? SingleMessage { get; set; }
        [Required, MinLength(15),MaxLength(2000)]
        public string? Reply { get; set; }
    }
}
