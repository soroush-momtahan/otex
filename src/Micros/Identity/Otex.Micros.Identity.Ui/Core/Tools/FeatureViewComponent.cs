using Microsoft.AspNetCore.Mvc;

namespace Otex.Micros.Identity.Ui.Core.Tools;

public abstract class FeatureViewComponent : ViewComponent
{
    // // این متد جایگزین متد View() پیش‌فرض می‌شود
    protected IViewComponentResult FeatureView<TModel>(TModel model)
    {
        //     var type = this.GetType();
        //     
        //     // // ۱. استخراج نام کلاس بدون پسوند ViewComponent (مثال: ProductCard)
        //     // var componentName = type.Name.EndsWith("ViewComponent") 
        //     //     ? type.Name.Substring(0, type.Name.Length - 13) 
        //     //     : type.Name;
        //     
        //     var componentName = type.Name;
        //     if (componentName.EndsWith("ViewComponent"))
        //         componentName = componentName.Replace("ViewComponent", "");
        //     else if (componentName.EndsWith("Initializer")) // یا Presenter
        //         componentName = componentName.Replace("Initializer", "");
        //
        //     // ۲. پیدا کردن مسیر دقیق پوشه از روی Namespace کلاس!
        //     var fullNamespace = type.Namespace ?? "";
        //     var featuresKeyword = "Features"; // کلمه کلیدی پوشه اصلی شما
        //     var featureIndex = fullNamespace.IndexOf(featuresKeyword);
        //
        //     if (featureIndex >= 0)
        //     {
        //         // خروجی: Features/Products/ProductCard
        //         var relativePath = fullNamespace.Substring(featureIndex).Replace(".", "/");
        //         
        //         // خروجی نهایی: ~/Features/Products/ProductCard/ProductCard.cshtml
        //         var viewPath = $"~/{relativePath}/{componentName}.cshtml";
        //         
        //         return View(viewPath, model);
        //     }
        //
        //     // فال‌بک به حالت پیش‌فرض (اگر کلاسی خارج از پوشه Features بود)
        //     return View(model); 

        var type = this.GetType();

        var componentName = type.Name;
        if (componentName.EndsWith("ViewComponent"))
            componentName = componentName.Replace("ViewComponent", "");
        else if (componentName.EndsWith("Initializer")) // یا Presenter
            componentName = componentName.Replace("Initializer", "");

        var fullNamespace = type.Namespace ?? "";

        // ۱. لیست پوشه‌های هدف را اینجا تعریف می‌کنیم
        var targetFolders = new[] { "Features", "Pages", "Core" };

        // ۲. Namespace را بر اساس نقطه جدا می‌کنیم
        // مثال: ["Otex", "Micros", "Identity", "Ui", "Features", "Products", "ProductCard"]
        var namespaceSegments = fullNamespace.Split('.');

        // ۳. پیدا کردن اولین بخشی که نام آن جزء لیست پوشه‌های هدف ماست
        var rootIndex = Array.FindIndex(namespaceSegments, segment => targetFolders.Contains(segment));

        if (rootIndex >= 0)
        {
            // ۴. جدا کردن قطعات از پوشه هدف به بعد و اتصال آن‌ها با اسلش
            // خروجی هدف: قطعات ["Features", "Products", "ProductCard"]
            var pathSegments = namespaceSegments.Skip(rootIndex);
            var relativePath = string.Join("/", pathSegments);

            // خروجی نهایی: ~/Features/Products/ProductCard/ProductCard.cshtml
            // یا: ~/Pages/Shared/MyComponent/MyComponent.cshtml
            // یا: ~/Core/Common/Alert/Alert.cshtml
            var viewPath = $"~/{relativePath}/{componentName}.cshtml";

            return View(viewPath, model);
        }

        // فال‌بک به حالت پیش‌فرض (اگر کلاسی خارج از این پوشه‌ها بود)
        return View(model);
    }
}