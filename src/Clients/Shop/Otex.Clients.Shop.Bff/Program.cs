using Duende.Bff;
using Duende.Bff.AccessTokenManagement;
using Duende.Bff.DynamicFrontends;
using Duende.Bff.Yarp;
using Microsoft.AspNetCore.DataProtection;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = BffAuthenticationSchemes.BffCookie;
    options.DefaultChallengeScheme = BffAuthenticationSchemes.BffOpenIdConnect;
    options.DefaultSignOutScheme = BffAuthenticationSchemes.BffOpenIdConnect;
});

builder.Services.AddBff()
    .AddRemoteApis()
    .ConfigureOpenIdConnect(options =>
    {
        options.Authority = builder.Configuration["services:identity-api:https:0"] ?? "https://localhost:7001";
        options.ClientId = "bff_client";
        options.ClientSecret = "secret";
        options.ResponseType = "code";
        options.ResponseMode = "query";

        options.GetClaimsFromUserInfoEndpoint = true;
        options.MapInboundClaims = false;
        options.SaveTokens = true;

        options.Scope.Clear();
        options.Scope.Add("openid");
        options.Scope.Add("profile");
        options.Scope.Add("api1");
        options.Scope.Add("offline_access");

        options.TokenValidationParameters = new()
        {
            NameClaimType = "name",
            RoleClaimType = "role"
        };
    })
    .ConfigureCookies(options =>
    {
        options.Cookie.Name = "__Host-bff";
        options.Cookie.SameSite = SameSiteMode.Strict;
    });
        
builder.Services.AddAuthorization();

// اضافه کردن سرویس YARP Forwarder برای پروکسی کردن صفحات به سرور Node.js (SSR)
builder.Services.AddHttpForwarder();

builder.Services.AddDataProtection()
    .SetApplicationName("BFF");

var app = builder.Build();

app.MapDefaultEndpoints();

app.UseDefaultFiles();
app.MapStaticAssets();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseBff();
app.UseAuthorization();

app.MapBffManagementEndpoints();

// 4. هدایت درخواست‌های API به Gateway (که توسط Aspire Yarp ایجاد شده)
// فرض می‌کنیم در Aspire اسم gateway شما "web-gateway" است.
app.MapRemoteBffApiEndpoint("/api", new Uri(builder.Configuration["services:web-gateway:https:0"] ?? "http://localhost:4000"))
    .WithAccessToken(RequiredTokenType.User);

// 5. تغییر حیاتی برای Angular SSR
// حذف app.MapFallbackToFile("/index.html"); و استفاده از Forwarder
if (app.Environment.IsDevelopment())
{
    // در حالت توسعه، انگولار روی پورت 4200 اجرا می‌شود
    app.MapForwarder("/{**catch-all}", "https://localhost:4200");
}
else
{
    // در حالت پروداکشن، سرور Node.js (SSR) انگولار باید اجرا شود (مثلاً روی پورت 4000)
    app.MapForwarder("/{**catch-all}", "https://localhost:4000");
}


app.Run();