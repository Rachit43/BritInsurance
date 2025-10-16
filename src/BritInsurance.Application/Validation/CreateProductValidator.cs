using BritInsurance.Application.Dto;
using FluentValidation;

namespace BritInsurance.Application.Validation
{
    public class CreateProductValidator : AbstractValidator<CreateProductDto>
    {
        public CreateProductValidator()
        {
            RuleFor(x => x.ProductName)
                .NotEmpty().WithMessage("Product name is required.")
                .MaximumLength(255).WithMessage("Product can not have more than 255 characters.");

            RuleFor(x => x.Items)
                .Must(items => items != null && items.Length > 0).WithMessage("At least one item is required.");

            RuleForEach(x => x.Items)
                .Where(x => x != null)
                .ChildRules(items =>
                {
                    items.RuleFor(x => x.Quantity).GreaterThan(0)
                        .WithMessage("Item quantity must be greater than 0.");
                });
        }
    }
}
