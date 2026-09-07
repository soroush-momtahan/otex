using Hydro;
using MediatR;
using Otex.BuildingBlocks.Domain.Errors;
using Otex.BuildingBlocks.Domain.Results;
using Otex.BuildingBlocks.Razor.Extensions;
using Otex.Micros.Identity.Application.Identity.Features.Commands.SendOtpForNewUser;
using Otex.Micros.Identity.Razor.Features.SignOn.Enums;

namespace Otex.Micros.Identity.Razor.Features.SignOn;

public record SignOnInputModel(
    string? Mobile,
    string? Password,
    string? ConfirmPassword,
    string? FirstName,
    string? LastName,
    string? ReturnUrl,
    string? OtpCode);

public record SignOnViewModel(
    SignOnStep? CurrentStep,
    SignOnStep? PreviousStep,
    bool IsExistingUser,
    List<Error> Errors)
{
    public bool HasError => Errors.Any();
}

public class SignOn : HydroComponent
{
    private readonly ISender _sender;

    public SignOn(ISender sender)
    {
        _sender = sender;
        Console.WriteLine("Hello World!");
    }

    public SignOnViewModel ViewModel { get; set; } = new(
        SignOnStep.InsertMobileNumber,
        SignOnStep.InsertMobileNumber,
        false,
        []);

    public SignOnInputModel InputModel { get; set; } = new(
        string.Empty,
        string.Empty,
        string.Empty,
        string.Empty,
        string.Empty,
        string.Empty,
        string.Empty);

    public async Task Submitting()
    {
        Console.WriteLine("Helllllo");
        ViewModel = ViewModel with { Errors = [] };
        // ViewModel = ViewModel with { CurrentStep = SignOnStep.SetPassword };
        if (ViewModel.CurrentStep == SignOnStep.InsertMobileNumber)
        {
            Result<SendOtpForNewUserResult> mobileExistenceOrError =
                await _sender.Send(new SendOtpForNewUserCommand(InputModel.Mobile!));

            if (mobileExistenceOrError.IsFailure)
            {
                List<Error> errors = mobileExistenceOrError.GetErrors();
                ViewModel = ViewModel with { Errors = errors };
                // ViewModel.Errors.AddRange(errors);
            }
            else if (mobileExistenceOrError.Value.IsUserExist)
            {
                ViewModel = ViewModel with
                {
                    IsExistingUser = true,
                    CurrentStep = SignOnStep.VerifyMethod
                };
            }
            else
            {
                ViewModel = ViewModel with
                {
                    IsExistingUser = false,
                    CurrentStep = SignOnStep.SetPassword
                };
            }
        }
    }

    public void BackStep()
    {
        var newPreviousStep = ViewModel.PreviousStep == SignOnStep.SetFullName
            ? SignOnStep.SetPassword
            : SignOnStep.InsertMobileNumber;

        ViewModel = ViewModel with
        {
            CurrentStep = ViewModel.PreviousStep,
            PreviousStep = newPreviousStep
        };
    }
}