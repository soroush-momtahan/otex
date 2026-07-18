using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Otex.Micros.Identity.Api.Pages;

public sealed class SecurityHeadersAttribute : ActionFilterAttribute
{
    public override void OnResultExecuting(ResultExecutingContext context)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));

        var result = context.Result;
        if (result is PageResult)
        {
            var headers = context.HttpContext.Response.Headers;
            
            // CSP: unsafe-eval حذف شده است (اگر دیدید JS شما از کار افتاد، فقط در آن صورت برگردانید)
            headers["Content-Security-Policy"] = "default-src 'self'; script-src 'self' https://cdn.jsdelivr.net; style-src 'self' 'unsafe-inline'; img-src 'self' data:; font-src 'self';";

            headers["X-Content-Type-Options"] = "nosniff";

            // 🔴 حتما باید SAMEORIGIN باشد تا iframe های داخلی Duende IdentityServer از کار نیفتند
            headers["X-Frame-Options"] = "SAMEORIGIN"; 

            headers["X-XSS-Protection"] = "1; mode=block";
            headers["Referrer-Policy"] = "strict-origin-when-cross-origin";
            
            
            // https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/X-Content-Type-Options
            if (!context.HttpContext.Response.Headers.ContainsKey("X-Content-Type-Options"))
            {
                context.HttpContext.Response.Headers.Append("X-Content-Type-Options", "nosniff");
            }

            // https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/X-Frame-Options
            if (!context.HttpContext.Response.Headers.ContainsKey("X-Frame-Options"))
            {
                context.HttpContext.Response.Headers.Append("X-Frame-Options", "DENY");
            }

            // https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Content-Security-Policy
            var csp =
                "default-src 'self'; object-src 'none'; frame-ancestors 'none'; sandbox allow-forms allow-same-origin allow-scripts; base-uri 'self';";
            // also consider adding upgrade-insecure-requests once you have HTTPS in place for production
            //csp += "upgrade-insecure-requests;";
            // also an example if you need client images to be displayed from twitter
            // csp += "img-src 'self' https://pbs.twimg.com;";

            // once for standards compliant browsers
            if (!context.HttpContext.Response.Headers.ContainsKey("Content-Security-Policy"))
            {
                context.HttpContext.Response.Headers.Append("Content-Security-Policy", csp);
            }

            // and once again for IE
            if (!context.HttpContext.Response.Headers.ContainsKey("X-Content-Security-Policy"))
            {
                context.HttpContext.Response.Headers.Append("X-Content-Security-Policy", csp);
            }

            // https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Referrer-Policy
            var referrer_policy = "no-referrer";
            if (!context.HttpContext.Response.Headers.ContainsKey("Referrer-Policy"))
            {
                context.HttpContext.Response.Headers.Append("Referrer-Policy", referrer_policy);
            }
        }
    }
}