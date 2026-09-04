using FluentValidation;
using Otex.Micros.Identity.Domain.Identity.Errors;

namespace Otex.Micros.Identity.Application.Identity.Features.Commands.SendOtp;

public sealed class SendOtpCommandValidation : AbstractValidator<SendOtpCommand>
{
    public SendOtpCommandValidation()
    {
        RuleFor(x => x.Mobile)
            .NotEmpty()
            .WithMessage(IdentityErrors.MobileNotValid.Description)
            .Length(11)
            .WithMessage(IdentityErrors.MobileNotValid.Description);
    }
}