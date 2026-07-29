using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Otex.BuildingBlocks.Api.ConfigureOptions.SwaggerOptions;

public class SwaggerGenOptionsSetup(IOptions<ApiInfoOptions.ApiInfoOptions> apiInfoOptions) : IConfigureOptions<SwaggerGenOptions>
{
    public ApiInfoOptions.ApiInfoOptions ApiInfoOptions { get; } = apiInfoOptions.Value;

    public void Configure(SwaggerGenOptions options)
    {
        options.SwaggerDoc(ApiInfoOptions.Version, new OpenApiInfo
        {
            Version = ApiInfoOptions.Version,
            Title = ApiInfoOptions.Title,
            Description = ApiInfoOptions.Description,
            TermsOfService = !string.IsNullOrEmpty(ApiInfoOptions.TermsOfServiceUrl)
                ? new Uri(ApiInfoOptions.TermsOfServiceUrl)
                : null,
            Contact = ApiInfoOptions.Contact is not null
                ? new OpenApiContact
                {
                    Name = ApiInfoOptions.Contact.Name,
                    Url = new Uri(ApiInfoOptions.Contact.Url ?? "")
                }
                : null,
            License = ApiInfoOptions.License is not null
                ? new OpenApiLicense
                {
                    Name = ApiInfoOptions.License.Name,
                    Url = new Uri(ApiInfoOptions.License.Url ?? "")
                }
                : null
        });
        
        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Please enter a valid token",
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            BearerFormat = "JWT",
            Scheme = "Bearer"
        });
        
        options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
        {
            [new OpenApiSecuritySchemeReference("bearer", document)] = []
        });
    }
}
