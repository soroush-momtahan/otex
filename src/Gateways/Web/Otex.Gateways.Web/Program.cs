using Microsoft.AspNetCore.RateLimiting;
using Otex.Gateways.Web.Middlewares;
using ServiceDefaults;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddServiceDiscovery();

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("fixed", opt =>
    {
        opt.PermitLimit = 100;
        opt.Window = TimeSpan.FromMinutes(1);
    });
    
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
});

var app = builder.Build();

app.UseMiddleware<RequestLoggingMiddleware>();

app.UseCors();
app.UseRateLimiter();


if (app.Environment.IsDevelopment())
{
   
}

app.MapReverseProxy(proxyPipeline =>
{
    // اضافه کردن Transform های سفارشی
    proxyPipeline.Use((context, next) =>
    {
        // مثلاً اضافه کردن یک هدر سفارشی به همه درخواست‌ها
        context.Request.Headers["X-Gateway-Version"] = "1.0.0";
        return next();
    });
    
    // استفاده از Service Discovery
    proxyPipeline.UseSessionAffinity();
    proxyPipeline.UseLoadBalancing();
    proxyPipeline.UsePassiveHealthChecks();
});

app.UseHttpsRedirection();

app.MapDefaultEndpoints();

app.Run();