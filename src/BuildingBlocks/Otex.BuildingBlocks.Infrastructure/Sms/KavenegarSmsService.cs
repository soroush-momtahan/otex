using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Otex.BuildingBlocks.Infrastructure.Sms.OptionsSetups;

namespace Otex.BuildingBlocks.Infrastructure.Sms;

public class KavenegarSmsService(HttpClient httpClient, IOptions<KavenegarOptions> options, ILogger<KavenegarSmsService> logger) : ISmsService
{
    private readonly KavenegarOptions _options = options.Value;

    public async Task<bool> SendOtpAsync(string mobile, string token, string templateName)
    {
        try
        {
            // ساخت URL مخصوص متد Lookup
            // مستندات: https://api.kavenegar.com/v1/{API-KEY}/verify/lookup.json
            string requestUrl = $"{_options.ApiKey}/verify/lookup.json?receptor={mobile}&token={token}&template={templateName}";

            HttpResponseMessage response = await httpClient.GetAsync(requestUrl);

            if (response.IsSuccessStatusCode)
            {
                // در پروژه‌های واقعی بهتر است بادی ریسپانس را چک کنید تا status=200 باشد
                return true; 
            }
            
            string errorContent = await response.Content.ReadAsStringAsync();
            logger.LogError("Kavenegar Error: {Error}", errorContent);
            return false;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Exception in sending SMS");
            return false;
        }
    }
}
