using Duende.IdentityServer.Models;

namespace Otex.Micros.Identity.Api;

public static class Config
{
    public static IEnumerable<IdentityResource> IdentityResources =>
    [
        new IdentityResources.OpenId(),
            new IdentityResources.Profile()
    ];

    public static IEnumerable<ApiScope> ApiScopes =>
    [
        new ApiScope("scope1"),
            new ApiScope("scope2"),
            new ApiScope("api1")
    ];

    public static IEnumerable<Client> Clients =>
    [
        new Client
            {
                ClientId = "bff_client",
                ClientName = "BFF for Angular App",
    
                // اینجا تفاوت اصلی است: کلاینت SPA نیازی به Secret نداشت، اما BFF یک کلاینت محرمانه است
                ClientSecrets = { new Secret("secret".Sha256()) },

                AllowedGrantTypes = GrantTypes.Code,
    
                // آدرس‌های BFF (فرض می‌کنیم BFF روی پورت 5002 اجرا می‌شود)
                RedirectUris =
                {
                    "https://localhost:5001/signin-oidc",
                },
                PostLogoutRedirectUris =
                {
                    "https://localhost:5001/signout-callback-oidc"
                },

                AllowedScopes = { "openid", "profile", "api1", "offline_access" },
    
                AllowOfflineAccess = true,
                RequirePkce = true
            }
    ];
}