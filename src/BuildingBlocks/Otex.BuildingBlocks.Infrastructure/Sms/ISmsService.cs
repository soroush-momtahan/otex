namespace Otex.BuildingBlocks.Infrastructure.Sms;

public interface ISmsService
{
    Task<bool> SendOtpAsync(string mobile, string token, string templateName);
}
