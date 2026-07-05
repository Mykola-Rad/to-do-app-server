using FluentValidation;
using ToDoApp.Application.DTOs.Auth;

namespace ToDoApp.Application.Validators.Auth
{
    public class RefreshTokenRequestDtoValidator : AbstractValidator<RefreshTokenRequestDto>
    {
        public RefreshTokenRequestDtoValidator()
        {
            RuleFor(x => x.AccessToken)
                .NotEmpty().WithMessage("AccessToken cannot be empty.");

            RuleFor(x => x.RefreshToken)
                .NotEmpty().WithMessage("RefreshToken cannot be empty.");
        }
    }
}
