using Otex.BuildingBlocks.Domain.Errors;
using Otex.Micros.Identity.Domain.Identity.Models;
using Otex.Micros.Identity.Domain.Identity.ValueObjects;

namespace Otex.Micros.Identity.Domain.Identity.Errors;

public class IdentityErrors
{
    public static readonly Error FirstnameIsTooShort = new(
        "Identity.FirstnameIsTooShort",
        $"نام نمی تواند کمتر از {FullName.MinLenght} حرف باشد.",
        ErrorType.Validation);
    
    public static readonly Error FirstnameIsTooLong = new(
        "Identity.FirstnameIsTooLong",
        $"نام نمی تواند بیشتر از {FullName.MaxLenght} حرف باشد.",
        ErrorType.Validation);
    
    public static readonly Error FirstnameIsEmpty = new(
        "Identity.FirstnameIsEmpty",
        "وارد کردن نام الزامی است.",
        ErrorType.Validation);
    
    public static readonly Error LastnameIsTooShort = new(
        "Identity.LastnameIsTooShort",
        $"نام خانوادگی نمی تواند کمتر از {FullName.MinLenght} حرف باشد.",
        ErrorType.Validation);
    
    public static readonly Error LastnameIsTooLong = new(
        "Identity.LastnameIsTooLong",
        $"نام خانوادگی نمی تواند بیشتر از {FullName.MaxLenght} حرف باشد.",
        ErrorType.Validation);
    
    public static readonly Error LastnameIsEmpty = new(
        "Identity.LastnameIsEmpty",
        "وارد کردن نام خانوادگی الزامی است.",
        ErrorType.Validation);

    public static readonly Error PasswordIsTooShort = new(
        "Identity.PasswordIsTooShort",
        $"پسورد باید حداقل {User.MinPasswordLength} کاراکتر باشد.",
        ErrorType.Validation);
    
    public static readonly Error PasswordIsTooLong = new(
        "Identity.PasswordIsTooLong",
        $"پسورد نباید بیشتر از {User.MaxPasswordLength} کاراکتر باشد.",
        ErrorType.Validation);
    
    public static readonly Error PasswordDoesNotContainUppercaseLetter = new(
        "Identity.PasswordDoesNotContainUppercaseLetter",
        "رمز عبور باید شامل حداقل یک حرف بزرگ انگلیسی باشد.",
        ErrorType.Validation);
    
    public static readonly Error PasswordDoesNotContainLowercaseLetter = new(
        "Identity.PasswordDoesNotContainLowercaseLetter",
        "رمز عبور باید شامل حداقل یک حرف کوچک انگلیسی باشد.",
        ErrorType.Validation);
    
    public static readonly Error PasswordDoesNotContainNumber = new(
        "Identity.PasswordDoesNotContainNumber",
        "رمز عبور باید حداقل شامل یک عدد (0-9) باشد.",
        ErrorType.Validation);
    
    public static readonly Error PasswordDoesNotContainSpecialLetter = new(
        "Identity.PasswordDoesNotContainSpecialLetter",
        "رمز عبور باید شامل حداقل یک کاراکتر ویژه (مانند !@#$%) باشد.",
        ErrorType.Validation);
    
    public static readonly Error PasswordRequiresUniqueChars = new(
        "Identity.PasswordRequiresUniqueChars",
        $"رمز عبور شما باید حداقل دارای {User.MinPasswordUniqueChars} کاراکتر متمایز باشد.",
        ErrorType.Validation);
    
    public static readonly Error ConfirmPasswordIsNotMatch = new(
        "Identity.ConfirmPasswordIsNotMatch",
        "تکرار رمز عبور با رمز عبور مطابقت ندارد.",
        ErrorType.Validation);
    
    public static readonly Error MobileNotValid = new(
        "Identity.MobileNotValid",
        $"شماره وارد شده معتبر نمی باشد.",
        ErrorType.Validation);
    public static readonly Error MobileNotValidLenght = new(
        "Identity.MobileNotValidLenght",
        $"شماره موبایل باید دقیقا برابر با 11 رقم باشد.",
        ErrorType.Validation);

    public static Error DuplicateMobile(string mobile) => new(
        "Identity.DuplicateMobile",
        $"شماره موبایل '{mobile}' قبلا ثبت شده است.",
        ErrorType.Validation);
    public static readonly Error VerifyCodeIsNotValid = new(
        "Identity.VerifyCodeIsNotValid",
        $"کد احراز هویت معتبر نمی باشد.",
        ErrorType.Validation);
    public static readonly Error VerifyCodeIsEmpty = new(
        "Identity.VerifyCodeIsEmpty",
        $"کد احراز نمی تواند خالی باشد.",
        ErrorType.Validation);

    public static readonly Error SendOtpFailed = new(
        "Identity.SendOtpFailed",
        "ارسال ناموفق بود ، لطفا کمی صبر کنید و دوباره تلاش بفرمایید.",
        ErrorType.Validation);

    public static readonly Error TempVerifyTokenInvalid = new(
        "Identity.TempVerifyTokenInvalid",
        "توکن احراز هویت معتبر نیست",
        ErrorType.Validation);
}