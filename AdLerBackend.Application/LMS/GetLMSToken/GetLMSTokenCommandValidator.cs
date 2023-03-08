using FluentValidation;

namespace AdLerBackend.Application.LMS.GetLMSToken;

public class GetLMSTokenCommandValidator : AbstractValidator<GetLMSTokenCommand>
{
    public GetLMSTokenCommandValidator()
    {
        RuleFor(v => v.UserName)
            .NotEmpty().WithMessage("Username is required");

        RuleFor(v => v.Password)
            .NotEmpty().WithMessage("Password is required");
    }
}