using System.ComponentModel.DataAnnotations;

namespace Final_E_Commerce.Entity
{
    public class BaseEntity
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public bool IsDeleted { get; set; } = false;

        public Nullable<DateTime> CreatedTime { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }
        public Nullable<DateTime> LastUpdatedAt { get; set; }
    }
}
