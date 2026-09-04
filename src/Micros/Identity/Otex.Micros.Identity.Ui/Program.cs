using Microsoft.Extensions.FileProviders;
using Otex.BuildingBlocks.Application;
using Otex.BuildingBlocks.Infrastructure;
using Otex.Micros.Identity.Infrastructure;
using Otex.Micros.Identity.Ui.Features.Products.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddControllers();
builder.Services.AddSingleton<ProductService>();
// // به جای AddServerSideBlazor اینو بنویس:
// builder.Services.AddRazorComponents()
//     .AddInteractiveServerComponents(); 

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
// app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

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

app.MapRazorPages()
    .WithStaticAssets();
app.MapControllers();
// app.MapBlazorHub();

app.Run();