using MediatR;
using Microsoft.AspNetCore.Mvc;
using Otex.BuildingBlocks.Application.Caching;
using Otex.BuildingBlocks.Domain.Errors;
using Otex.BuildingBlocks.Domain.Results;
using Otex.BuildingBlocks.Razor.Extensions;
using Otex.Micros.Identity.Application.Identity.Features.Commands.SendOtp;
using Otex.Micros.Identity.Application.Identity.Features.Commands.SendOtpForNewUser;
using Otex.Micros.Identity.Ui.Core.Tools;
using Otex.Micros.Identity.Ui.Features.Login.Enums;

namespace Otex.Micros.Identity.Ui.Features.Login;

public record LoginInputModel(
    string? Mobile,
    string? Password,
    string? ConfirmPassword,
    string? FirstName,
    string? LastName,
    string? ReturnUrl,
    string? OtpCode
    );

public record LoginViewModel(
    LoginInputModel Input,
    SignOnStep? CurrentStep,
    SignOnStep? PreviousStep,
    bool IsExistingUser,
    SignOnAction? Action,
    List<Error> Errors)
{
    public bool HasError => Errors.Any();
}

[Route("htmx/[controller]")]
public class LoginController(
    ISender sender,
    ICacheService cacheService) : Controller
{
    [HttpPost("process")]
    public async Task<IActionResult> Process([FromForm] LoginInputModel input)
    {
        if (input.Action == SignOnAction.Back)
        {
            input = input with { CurrentStep = input.PreviousStep };
            return RenderComponent(input);
        }
        
        if (input.CurrentStep == SignOnStep.InsertMobileNumber)
        {
            Result<SendOtpForNewUserResult> mobileExistenceOrError =
                await sender.Send(new SendOtpForNewUserCommand(input.Mobile!));
            
            return mobileExistenceOrError.Match(
                onSuccess: result =>
                {
                    if (result.IsUserExist)
                    {
                        return RenderComponent(input with
                        {
                            IsExistingUser = result.IsUserExist,
                            CurrentStep = SignOnStep.VerifyMethod
                        });
                    }
                    return RenderComponent(input with
                    {
                        IsExistingUser = result.IsUserExist,
                        CurrentStep = SignOnStep.OtpVerify,
                        PreviousStep = SignOnStep.InsertMobileNumber
                    });
                },
                onFailure: err => RenderComponent(input, err)
            );
        }
        
        if (input is { CurrentStep: SignOnStep.VerifyMethod, Action: SignOnAction.OtpVerifyMethod })
        {
            Result sendOtpOrError = await sender.Send(new SendOtpCommand(input.Mobile!));
            return sendOtpOrError.Match(
                onSuccess: () => RenderComponent(input with
                {
                    CurrentStep = SignOnStep.OtpVerify, 
                    PreviousStep = SignOnStep.VerifyMethod
                }),
                onFailure: (err) => RenderComponent(input, err)
            );
        }
        
        if (input is { CurrentStep: SignOnStep.VerifyMethod, Action: SignOnAction.PasswordVerifyMethod })
        {
            
        }

        if (input.CurrentStep == SignOnStep.OtpVerify)
        {
            
        }

        if (input.CurrentStep == SignOnStep.SetPassword)
        {
            
        }

        if (input.CurrentStep == SignOnStep.SetFullName)
        {
            
        }
        
        
        

        // // پردازش مرحله ۲ (رمز عبور)
        // if (input.CurrentStep == 2)
        // {
        //     // // real step 2
        //     var code = await cacheService.GetAsync<string>($"Otp:{input.Mobile}");
        //     Result<VerifyMobileResult> verifyMobileOrError =
        //         await sender.Send(new VerifyMobileCommand(code!, input.Mobile!));
        //     if (verifyMobileOrError.IsFailure)
        //     {
        //         errors.Add(verifyMobileOrError.Error);
        //     }
        //
        //     input = input with { IsExistingUser = verifyMobileOrError.Value.IsNewUser, CurrentStep = 2 };
        //
        //     if (string.IsNullOrWhiteSpace(input.Password))
        //     {
        //         errors.Add(new Error("Password", "رمز عبور الزامی است.", ErrorType.Validation));
        //         return RenderComponent(input, errors);
        //     }
        //
        //     if (input.IsExistingUser)
        //     {
        //         // لاگین کاربر قدیمی
        //         // var loginResult = await _authService.LoginAsync(input.Mobile, input.Password);
        //         // if (!loginResult.IsSuccess)
        //         // {
        //         //     errors.Add(new Error("Auth", "رمز عبور اشتباه است."));
        //         //     return RenderComponent(input, errors);
        //         // }
        //
        //         // لاگین موفق! ریدایرکت به Duende IdentityServer (ReturnUrl)
        //         // اینجا HTMX رو مجبور می‌کنیم کل صفحه رو ریدایرکت کنه
        //         Response.Headers.Add("HX-Redirect", input.ReturnUrl ?? "/");
        //         return Ok();
        //     }
        //     else
        //     {
        //         // // کاربر جدید <- برو مرحله ۳ برای گرفتن نام
        //         // if (input.Password != input.ConfirmPassword)
        //         // {
        //         //     errors.Add(new Error("Password", "تکرار رمز عبور تطابق ندارد.", ErrorType.Validation));
        //         //     return RenderComponent(input, errors);
        //         // }
        //         //
        //         // input = input with { CurrentStep = 3 };
        //     }
        // }
        // // پردازش مرحله ۳ (ثبت نام نهایی)
        // else if (input.CurrentStep == 3)
        // {
        //     if (string.IsNullOrWhiteSpace(input.FirstName) || string.IsNullOrWhiteSpace(input.LastName))
        //     {
        //         errors.Add(new Error("Name", "نام و نام خانوادگی الزامی است.", ErrorType.Validation));
        //         return RenderComponent(input, errors);
        //     }
        //
        //     // ثبت نام نهایی
        //     // await _authService.RegisterAsync(input);
        //     Response.Headers.Add("HX-Redirect", input.ReturnUrl ?? "/");
        //     return Ok();
        // }

        return RenderComponent(input);
    }

    // متد کمکی برای رندر مجدد ViewComponent
    private IActionResult RenderComponent(LoginInputModel input, List<Error>? errors = null)
    {
        var vm = new LoginViewModel(input, errors ?? []);
        return ViewComponent("Login", new { viewModel = vm });
    }
}

[ViewComponent(Name = "Login")]
public class LoginInitializer : FeatureViewComponent
{
    public IViewComponentResult Invoke(LoginViewModel? viewModel = null, string? returnUrl = null)
    {
        // اگر فرم سابمیت شده و کنترلر این کامپوننت رو صدا زده، دیتای کنترلر رو برگردون
        if (viewModel != null)
        {
            return FeatureView(viewModel);
        }

        // اگر لود اولیه صفحه است، یک فرم خالی بساز
        var initialInput = new LoginInputModel(
            null,
            null,
            null,
            null,
            null,
            returnUrl,
            SignOnStep.InsertMobileNumber,
            SignOnStep.InsertMobileNumber,
            false,
            null,
            string.Empty
            );

        var initialViewModel = new LoginViewModel(initialInput, new List<Error>());

        return FeatureView(initialViewModel);
    }
}