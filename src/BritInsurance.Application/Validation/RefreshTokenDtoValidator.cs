using BritInsurance.Application.Dto;
using FluentValidation;

namespace BritInsurance.Application.Validation
{

    public class RefreshTokenDtoValidator : AbstractValidator<RefreshTokenDto>
    {
        public RefreshTokenDtoValidator()
        {
            RuleFor(x => x.RefreshToken)
                .NotEmpty().WithMessage("Refresh Token is required.");
        }
    }
}
