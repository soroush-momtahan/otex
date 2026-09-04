using FluentValidation;
using Otex.Micros.Identity.Domain.Identity.Errors;

namespace Otex.Micros.Identity.Application.Identity.Features.Commands.VerifyMobile;

internal sealed class VerifyMobileCommandValidation : AbstractValidator<VerifyMobileCommand>
{
    public VerifyMobileCommandValidation()
    {
        RuleFor(x => x.Mobile)
            .NotEmpty()
            .WithMessage(IdentityErrors.MobileNotValid.Description)
            .Length(11)
            .WithMessage(IdentityErrors.MobileNotValid.Description);

        RuleFor(x => x.Code)
            .NotEmpty()
            .WithMessage(IdentityErrors.VerifyCodeIsEmpty.Description);
    }
}