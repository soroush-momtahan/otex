using FluentValidation;
using Otex.Micros.Identity.Domain.Identity.Errors;

namespace Otex.Micros.Identity.Application.Identity.Features.Commands.SendOtpForNewUser;

internal sealed class SendOtpForNewUserValidation : AbstractValidator<SendOtpForNewUserCommand>
{
    public SendOtpForNewUserValidation()
    {
        RuleFor(x => x.Mobile)
            .NotEmpty()
            .WithMessage(IdentityErrors.MobileNotValid.Description)
            .Length(11)
            .WithMessage(IdentityErrors.MobileNotValid.Description);
    }
}