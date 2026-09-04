namespace Otex.Micros.Identity.Application.Identity.Services;

public interface IOtpSenderService
{
    Task SendOtpAsync(string mobile, string code);
}