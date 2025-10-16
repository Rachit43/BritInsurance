using BritInsurance.Application.Dto;
using FluentValidation;

namespace BritInsurance.Application.Validation
{
    public class UpdateProductValidator : AbstractValidator<UpdateProductDto>
    {
        public UpdateProductValidator()
        {
            RuleFor(x => x.ProductName)
                .NotEmpty().WithMessage("Product name is required.")
                .MaximumLength(255).WithMessage("Product name cannot exceed 255 characters.");

            // When IgnoreItems is false → require items and validate quantity
            When(x => !x.IgnoreItems, () =>
            {
                RuleFor(x => x.Items)
                    .NotNull().WithMessage("Items must not be null.")
                    .Must(items => items.Length > 0)
                    .WithMessage("At least one item is required.");

                RuleForEach(x => x.Items)
                    .Where(item => item != null)
                    .Must(item => item.Quantity > 0)
                    .WithMessage("Item quantity must be greater than 0.");
            });

            // When IgnoreItems is true → items must be null or empty
            When(x => x.IgnoreItems, () =>
            {
                RuleFor(x => x.Items)
                    .Must(items => items == null || items.Length == 0)
                    .WithMessage("Items must be empty when IgnoreItems is true.");
            });
        }
    }

}
