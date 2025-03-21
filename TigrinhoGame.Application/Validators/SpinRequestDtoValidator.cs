using FluentValidation;
using TigrinhoGame.Application.DTOs;

namespace TigrinhoGame.Application.Validators
{
    public class SpinRequestDtoValidator : AbstractValidator<SpinRequestDto>
    {
        public SpinRequestDtoValidator()
        {
            RuleFor(x => x.PlayerId)
                .NotEmpty().WithMessage("Player ID is required");

            RuleFor(x => x.BetAmount)
                .NotEmpty().WithMessage("Bet amount is required")
                .GreaterThan(0).WithMessage("Bet amount must be greater than zero")
                .LessThanOrEqualTo(20).WithMessage("Bet amount cannot exceed 20.00")
                .PrecisionScale(4, 2, false).WithMessage("Bet amount cannot have more than 2 decimal places");
        }
    }
} 