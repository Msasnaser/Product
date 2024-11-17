using FluentValidation;

namespace Product.Dtos
{
    public class ProductDtoValidation : AbstractValidator<ProductDtos>
    {
        public ProductDtoValidation()
        {
            // Name: 
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MinimumLength(5).WithMessage("Name must be at least 5 characters")
                .MaximumLength(30).WithMessage("Name cannot exceed 30 characters.");

            // Price:
            RuleFor(x => x.Price)
                .NotEmpty().WithMessage("Price is required.")
                .InclusiveBetween(20, 3000).WithMessage("Price must be between 20 and 3000.");

            // Description:
            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required.")
                .MinimumLength(10).WithMessage("Description must be at least 10 characters ");
        }

    }
}