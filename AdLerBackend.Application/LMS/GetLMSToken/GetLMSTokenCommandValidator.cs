using FluentValidation;

namespace AdLerBackend.Application.LMS.GetLMSToken;

public class GetLmsTokenCommandValidator : AbstractValidator<GetLmsTokenCommand>
{
    public GetLmsTokenCommandValidator()
    {
        RuleFor(v => v.UserName)
            .NotEmpty().WithMessage("Username is required");

        RuleFor(v => v.Password)
            .NotEmpty().WithMessage("Password is required");
    }
}