using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Otex.BuildingBlocks.Infrastructure.Sms.OptionsSetups;

public class KavenegarOptionsSetup(IConfiguration configuration) : IConfigureOptions<KavenegarOptions> 
{
    public void Configure(KavenegarOptions options)
    {
        configuration.GetSection(nameof(KavenegarOptions)).Bind(options);
    }
}
