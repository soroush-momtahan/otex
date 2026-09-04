using Microsoft.AspNetCore.Mvc;

namespace Otex.Micros.Identity.Ui.Core.Extensions;

public static class UrlHelperExtensions
{
    /// <summary>
    /// تولید آدرس کاملاً Type-Safe برای هندلرهای Razor Pages
    /// </summary>
    public static string? SafeHandler(this IUrlHelper url, string methodName)
    {
        // استخراج نام واقعی هندلر با حذف پیشوندها و پسوندهای استاندارد دات‌نت
        var handlerName = methodName
            .Replace("OnPost", "")
            .Replace("OnGet", "")
            .Replace("OnPut", "")
            .Replace("OnDelete", "")
            .Replace("Async", ""); // اگر متد تو Async باشد این کلمه را هم حذف می‌کند

        // پارامتر اول null یعنی "همین صفحه فعلی"
        return url.Page(null, handlerName);
    }
}