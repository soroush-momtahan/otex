using Hydro.Configuration;
using Microsoft.Extensions.FileProviders;
using Otex.BuildingBlocks.Application;
using Otex.BuildingBlocks.Infrastructure;
using Otex.Micros.Identity.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddHydro();

builder
    .AddApplicationConfiguration(
        assembliesToScanRequestHandlers: Otex.Micros.Identity.Application.AssemblyReference.ApplicationAssembly)
    .AddInfrastructureConfiguration()
    .AddIdentityMicro("identity-db", "redis");

var app = builder.Build();



// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(builder.Environment.ContentRootPath, "Features")),
    RequestPath = "/features" // آدرسی که تو مرورگر صدا می‌زنیم
});

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(builder.Environment.ContentRootPath, "Core")),
    RequestPath = "/core"
});

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(builder.Environment.ContentRootPath, "Pages")),
    RequestPath = "/Pages"
});
app.UseRouting();


app.UseAuthorization();


app.MapStaticAssets();
app.MapRazorPages()
    .WithStaticAssets();
app.UseHydro();

app.Run();