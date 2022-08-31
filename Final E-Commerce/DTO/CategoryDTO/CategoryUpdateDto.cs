using Microsoft.AspNetCore.Http;

namespace WebApi.DTO.CategoryDTO
{
    public class CategoryUpdateDto
    {
        public string? Name { get; set; }
        public IFormFile? Photo { get; set; }
        public Nullable<int> ParentId { get; set; }
    }
}
