using FluentValidation;

namespace WebApi.DTO.Product_DTOs
{
    public class ProductUpdateDto
    {
        public string? Name { get; set; }
        public decimal Price { get; set; }
        public int Count { get; set; }
        public bool IsFeatured { get; set; }
    }
    public class ProductUpdateDtoValidator : AbstractValidator<ProductCreateDto>
    {
        public ProductUpdateDtoValidator()
        {
            RuleFor(p => p.Name).NotEmpty().WithMessage("Name can not be empty").MaximumLength(30).WithMessage("Max character: 30");
            RuleFor(p => p.Description).NotEmpty().WithMessage("Description can not be empty")
                .MinimumLength(50).WithMessage("Minimum allowed character length is 50")
                .MaximumLength(1000).WithMessage("Maximum allowed character length is 1000");
            RuleFor(p => p.Price).NotEmpty().WithMessage("Price can not be empty")
                .GreaterThan(10).WithMessage("Price can not be lower than 5");
            RuleFor(p => p.DiscountPercent).LessThan(0).WithMessage("Discount percent can not be minus")
                .GreaterThan(90).WithMessage("Discount percent can not be higher than 90%");
            RuleFor(p => p.Count).LessThan(10).WithMessage("You need to have at least 10 items");
        }
    }
}