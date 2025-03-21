using FluentValidation;
using TigrinhoGame.Application.DTOs;

namespace TigrinhoGame.Application.Validators
{
    public class PlayerLoginDtoValidator : AbstractValidator<PlayerLoginDto>
    {
        public PlayerLoginDtoValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Invalid email format")
                .MaximumLength(100).WithMessage("Email cannot exceed 100 characters");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters long")
                .MaximumLength(50).WithMessage("Password cannot exceed 50 characters");
        }
    }
} 