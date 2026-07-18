using System.ComponentModel.DataAnnotations;

namespace Otex.Micros.Identity.Api.Pages.Account.Login;

public class InputModel
{
    [Required(ErrorMessage = "شماره موبایل الزامی است")] 
    public string? Username { get; set; } // همان موبایل

    [Required(ErrorMessage = "رمز عبور الزامی است")] 
    public string? Password { get; set; }

    public string? ConfirmPassword { get; set; }
    
    [Required(ErrorMessage = "نام الزامی است"), MinLength(3, ErrorMessage = "نام باید حداقل 3 کاراکتر داشته باشد")]
    public string? FirstName { get; set; }
    public string? LastName { get; set; }

    public bool RememberLogin { get; set; }
    public string? ReturnUrl { get; set; }
    public string? Button { get; set; }
}