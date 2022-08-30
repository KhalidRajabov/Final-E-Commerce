using Microsoft.AspNetCore.Http;

namespace WebApi.DTO.CategoryDTO
{
    public class CategoryCreateDto
    {
        public string? Name { get; set; }
        public IFormFile? Photo { get; set; }
    }
}
