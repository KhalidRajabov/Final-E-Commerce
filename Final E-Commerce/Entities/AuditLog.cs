using Microsoft.AspNetCore.Mvc.RazorPages.Infrastructure;

namespace Final_E_Commerce.Entities
{
    public class AuditLog
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? Path { get; set; }
        public bool IsHttps { get; set; }
        public string? QueryString { get; set; }
        public string? Method { get; set; }
        public string? Area { get; set; }
        public string? Controller { get; set; }
        public string? Action { get; set; }
        public int StatusCode { get; set; }
        public DateTime RequestTime { get; set; }
        public DateTime RespondTime { get; set; }
        public string? MadeByUserId { get; set; }
    }
}
