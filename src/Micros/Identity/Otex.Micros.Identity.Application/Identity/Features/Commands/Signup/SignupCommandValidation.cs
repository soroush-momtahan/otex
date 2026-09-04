using FluentValidation;
using Otex.Micros.Identity.Application.Identity.Extensions;
using Otex.Micros.Identity.Domain.Identity.Errors;
using Otex.Micros.Identity.Domain.Identity.ValueObjects;

namespace Otex.Micros.Identity.Application.Identity.Features.Commands.Signup;

internal sealed class SignupCommandValidation : AbstractValidator<SignupCommand>
{
    public SignupCommandValidation()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .WithMessage(IdentityErrors.FirstnameIsEmpty.Description)
            .MinimumLength(FullName.MinLenght)
            .WithMessage(IdentityErrors.FirstnameIsTooShort.Description)
            .MaximumLength(FullName.MaxLenght)
            .WithMessage(IdentityErrors.FirstnameIsTooLong.Description);
        
        RuleFor(x => x.LastName)
            .NotEmpty()
            .WithMessage(IdentityErrors.LastnameIsEmpty.Description)
            .MinimumLength(FullName.MinLenght)
            .WithMessage(IdentityErrors.LastnameIsTooShort.Description)
            .MaximumLength(FullName.MaxLenght)
            .WithMessage(IdentityErrors.LastnameIsTooLong.Description);

        RuleFor(x => x.Password)
            .PasswordRules();
        
        RuleFor(x => x.ConfirmPassword)
            .Equal(x => x.Password)
        .WithMessage(IdentityErrors.ConfirmPasswordIsNotMatch.Description);
    }
}