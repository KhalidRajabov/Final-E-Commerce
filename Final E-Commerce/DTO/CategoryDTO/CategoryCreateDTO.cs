using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace WebApi.DTO.CategoryDTO
{
    public class CategoryCreateDto
    {
        public string? Name { get; set; }
        public IFormFile? Photo { get; set; }
        public Nullable<int> ParentId { get; set; }
    }
    public class CategoryCreateDtoValidation : AbstractValidator<CategoryCreateDto>
    {
        public CategoryCreateDtoValidation()
        {
            RuleFor(c => c.Name).NotEmpty();
        }
    }
}