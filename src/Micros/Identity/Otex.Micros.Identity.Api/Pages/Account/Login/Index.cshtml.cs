using Duende.IdentityServer;
using Duende.IdentityServer.Events;
using Duende.IdentityServer.Services;
using Duende.IdentityServer.Stores;
using Otex.Micros.Identity.Api.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;


namespace Otex.Micros.Identity.Api.Pages.Account.Login;

[SecurityHeaders]
[AllowAnonymous]
public class Index : PageModel
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IIdentityServerInteractionService _interaction;
    private readonly IEventService _events;
    private readonly IAuthenticationSchemeProvider _schemeProvider;
    private readonly IIdentityProviderStore _identityProviderStore;

    public ViewModel View { get; set; } = default!;

    [BindProperty] public InputModel Input { get; set; } = default!;
    
    [BindProperty] public int CurrentStep { get; set; } = 1;
    [BindProperty] public bool IsExistingUser { get; set; }

    public Index(
        IIdentityServerInteractionService interaction,
        IAuthenticationSchemeProvider schemeProvider,
        IIdentityProviderStore identityProviderStore,
        IEventService events,
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _interaction = interaction;
        _schemeProvider = schemeProvider;
        _identityProviderStore = identityProviderStore;
        _events = events;
    }

    public async Task<IActionResult> OnGetAsync(string? returnUrl, CancellationToken ct)
    {
        await BuildModelAsync(returnUrl, ct);
        return Page();
    }

    // =================================================================
    // مرحله 1: دریافت و بررسی شماره موبایل
    // =================================================================
    public async Task<IActionResult> OnPostStep1Async(CancellationToken ct)
    {
        ModelState.Clear();
        CurrentStep = 1;

        if (string.IsNullOrWhiteSpace(Input.Username) || Input.Username.Length < 10)
        {
            ModelState.AddModelError(string.Empty, "شماره موبایل نامعتبر است.");
            await BuildModelAsync(Input.ReturnUrl, ct);
            return Page();
        }

        var user = await _userManager.FindByNameAsync(Input.Username);
        IsExistingUser = user != null;
        CurrentStep = 2; 

        await BuildModelAsync(Input.ReturnUrl, ct);
        return Page();
    }

    // =================================================================
    // مرحله 2: بررسی رمز (ورود مستقیم قدیمی‌ها / تایید رمز جدیدها)
    // =================================================================
    public async Task<IActionResult> OnPostStep2Async(CancellationToken ct)
    {
        ModelState.Clear();
        CurrentStep = 2;

        // چک کردن امنیتی مجدد کاربر جهت جلوگیری از دستکاری فرم در کلاینت
        var user = await _userManager.FindByNameAsync(Input.Username!);
        IsExistingUser = user != null;

        if (string.IsNullOrWhiteSpace(Input.Password) || Input.Password.Length < 6)
        {
            ModelState.AddModelError(string.Empty, "رمز عبور باید حداقل ۶ کاراکتر باشد.");
            await BuildModelAsync(Input.ReturnUrl, ct);
            return Page();
        }

        if (IsExistingUser)
        {
            // مسیر کاربر قدیمی -> ورود نهایی
            var result = await _signInManager.PasswordSignInAsync(Input.Username!, Input.Password!, isPersistent: true, lockoutOnFailure: true);
            if (result.Succeeded)
            {
                return await HandleSuccessfulLogin(user!, Input.ReturnUrl, ct);
            }
            
            ModelState.AddModelError(string.Empty, "رمز عبور وارد شده اشتباه است.");
            await BuildModelAsync(Input.ReturnUrl, ct);
            return Page();
        }
        else
        {
            // مسیر کاربر جدید -> بررسی تکرار رمز و رفتن به مرحله بعد
            if (Input.Password != Input.ConfirmPassword)
            {
                ModelState.AddModelError(string.Empty, "رمز عبور و تکرار آن یکسان نیستند.");
                await BuildModelAsync(Input.ReturnUrl, ct);
                return Page();
            }
            CurrentStep = 3;
            await BuildModelAsync(Input.ReturnUrl, ct);
            return Page();
        }
    }

    // =================================================================
    // مرحله 3: ثبت نام کاربر جدید و ورود
    // =================================================================
    public async Task<IActionResult> OnPostStep3Async(CancellationToken ct)
    {
        ModelState.Clear();
        CurrentStep = 3;

        // بررسی مجدد برای جلوگیری از دستکاری
        var existingUser = await _userManager.FindByNameAsync(Input.Username!);
        if (existingUser != null)
        {
            return RedirectToPage(new { returnUrl = Input.ReturnUrl }); // جلوگیری از هک
        }

        if (string.IsNullOrWhiteSpace(Input.FirstName) || string.IsNullOrWhiteSpace(Input.LastName))
        {
            ModelState.AddModelError(string.Empty, "وارد کردن نام و نام خانوادگی الزامی است.");
            await BuildModelAsync(Input.ReturnUrl, ct);
            return Page();
        }

        var newUser = new ApplicationUser 
        { 
            UserName = Input.Username, 
            PhoneNumber = Input.Username, 
            FirstName = Input.FirstName, 
            LastName = Input.LastName 
        };
        
        var createResult = await _userManager.CreateAsync(newUser, Input.Password!);

        if (!createResult.Succeeded)
        {
            foreach (var err in createResult.Errors) ModelState.AddModelError(string.Empty, err.Description);
            await BuildModelAsync(Input.ReturnUrl, ct);
            return Page();
        }

        await _signInManager.PasswordSignInAsync(Input.Username!, Input.Password!, isPersistent: true, lockoutOnFailure: false);
        return await HandleSuccessfulLogin(newUser, Input.ReturnUrl, ct);
    }

    // =================================================================
    // هندلر دکمه برگشت
    // =================================================================
    public async Task<IActionResult> OnPostBackAsync(int targetStep, CancellationToken ct)
    {
        ModelState.Clear(); // پاک کردن ارورها - مقادیر تایپ شده در Input باقی می‌مانند
        
        // تشخیص امنیتی مجدد هنگام برگشت
        if(!string.IsNullOrEmpty(Input.Username))
        {
            var user = await _userManager.FindByNameAsync(Input.Username);
            IsExistingUser = user != null;
        }

        CurrentStep = targetStep;
        await BuildModelAsync(Input.ReturnUrl, ct);
        return Page();
    }

    // =================================================================
    // متد کمکی: انجام عملیات موفقیت آمیز و ریدایرکت HTMX
    // =================================================================
    private async Task<IActionResult> HandleSuccessfulLogin(ApplicationUser user, string? returnUrl, CancellationToken ct)
    {
        var context = await _interaction.GetAuthorizationContextAsync(returnUrl, ct);
        await _events.RaiseAsync(new UserLoginSuccessEvent(user.UserName, user.Id, user.UserName, clientId: context?.Client.ClientId), ct);

        var redirectUrl = context != null ? (returnUrl ?? "~/") : (Url.IsLocalUrl(returnUrl) ? returnUrl : "~/");

        if (Request.Headers.ContainsKey("HX-Request"))
        {
            // استاندارد HTMX برای ریدایرکت کلاینت
            Response.Headers.Append("HX-Redirect", redirectUrl);
            return StatusCode(200); 
        }
        return Redirect(redirectUrl);
    }

    private async Task BuildModelAsync(string? returnUrl, CancellationToken ct)
    {
        Input ??= new InputModel();
        
        // 🔴 باگ اصلی اینجا بود! خط زیر باعث میشد تمام دیتای کاربر پاک شود. آن را حذف کردیم.
        // Input = new InputModel { ReturnUrl = returnUrl }; 

        Input.ReturnUrl = returnUrl;

        var context = await _interaction.GetAuthorizationContextAsync(returnUrl, ct);
        if (context?.IdP != null)
        {
            var scheme = await _schemeProvider.GetSchemeAsync(context.IdP);
            if (scheme != null)
            {
                var local = context.IdP == IdentityServerConstants.LocalIdentityProvider;
                View = new ViewModel { EnableLocalLogin = local };
                Input.Username = context.LoginHint;
                if (!local)
                {
                    View.ExternalProviders = [new ViewModel.ExternalProvider(authenticationScheme: context.IdP, displayName: scheme.DisplayName)];
                }
            }
            return;
        }

        var schemes = await _schemeProvider.GetAllSchemesAsync();
        var providers = schemes.Where(x => x.DisplayName != null)
            .Select(x => new ViewModel.ExternalProvider(authenticationScheme: x.Name, displayName: x.DisplayName ?? x.Name)).ToList();

        var dynamicSchemes = (await _identityProviderStore.GetAllSchemeNamesAsync(ct))
            .Where(x => x.Enabled)
            .Select(x => new ViewModel.ExternalProvider(authenticationScheme: x.Scheme, displayName: x.DisplayName ?? x.Scheme));
        providers.AddRange(dynamicSchemes);

        var allowLocal = true;
        var client = context?.Client;
        if (client != null)
        {
            allowLocal = client.EnableLocalLogin;
            if (client.IdentityProviderRestrictions != null && client.IdentityProviderRestrictions.Count != 0)
            {
                providers = providers.Where(provider => client.IdentityProviderRestrictions.Contains(provider.AuthenticationScheme)).ToList();
            }
        }

        View = new ViewModel
        {
            AllowRememberLogin = LoginOptions.AllowRememberLogin,
            EnableLocalLogin = allowLocal && LoginOptions.AllowLocalLogin,
            ExternalProviders = providers.ToArray()
        };
    }
}