using FluentValidation;

namespace AdLerBackend.Application.AdminPanel.GetAdminToken;

public class GetAdminTokenCommandValidator : AbstractValidator<GetAdminTokenCommand>
{
    public GetAdminTokenCommandValidator()
    {
        RuleFor(v => v.UserName)
            .NotEmpty().WithMessage("Username is required");

        RuleFor(v => v.Password)
            .NotEmpty().WithMessage("Password is required");
    }
}