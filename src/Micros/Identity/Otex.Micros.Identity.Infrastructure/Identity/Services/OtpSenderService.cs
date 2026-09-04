using Otex.Micros.Identity.Application.Identity.Services;

namespace Otex.Micros.Identity.Infrastructure.Identity.Services;

public class OtpSenderService : IOtpSenderService
{
    public Task SendOtpAsync(string mobile, string code)
    {
        Console.WriteLine(code);
        return Task.CompletedTask;
    }
}