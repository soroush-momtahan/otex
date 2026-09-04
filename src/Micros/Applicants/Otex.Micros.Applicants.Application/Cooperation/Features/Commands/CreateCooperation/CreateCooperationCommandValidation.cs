using FluentValidation;
using Otex.Micros.Applicants.Domain.Cooperation.Errors;
using Otex.Micros.Applicants.Domain.Cooperation.ValueObjects;

namespace Otex.Micros.Applicants.Application.Cooperation.Features.Commands.CreateCooperation;

internal sealed class CreateCooperationCommandValidation : AbstractValidator<CreateCooperationCommand>
{
    public CreateCooperationCommandValidation()
    {
        RuleFor(command => command.FirstName)
            .MinimumLength(Fullname.MinLenght)
            .WithMessage(CooperationErrors.FirstnameIsTooShort.Description)
            .MaximumLength(Fullname.MaxLenght)
            .WithMessage(CooperationErrors.FirstnameIsTooLong.Description);
        
        RuleFor(command => command.LastName)
            .MinimumLength(Fullname.MinLenght)
            .WithMessage(CooperationErrors.LastnameIsTooShort.Description)
            .MaximumLength(Fullname.MaxLenght)
            .WithMessage(CooperationErrors.LastnameIsTooShort.Description);

        RuleFor(command => command.Mobile)
            .Must(x => x.Length == Mobile.ValidLenght)
            .WithMessage(CooperationErrors.MobileNotValid.Description);

        RuleFor(command => command.TypeOfActivity)
            .IsInEnum()
            .WithMessage(CooperationErrors.ActivityIsOutOfRang.Description);
    }
}