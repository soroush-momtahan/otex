using MediatR;
using Microsoft.AspNetCore.Mvc;
using Otex.Micros.Identity.Ui.Core.Tools;
using Otex.Micros.Identity.Ui.Features.Products.Components.ProductList;
using Otex.Micros.Identity.Ui.Features.Products.Data;

namespace Otex.Micros.Identity.Ui.Features.Products.Components.ProductCard;

public record ProductCardViewModel(GetProductListQuery.ProductDto ProductDto, bool IsLiked);

[Route("htmx/product-card")]
public class ProductCardController(ProductService productService) : Controller
{
    
    [HttpPost("{id}/toggle-like")]
    public IActionResult ToggleLike(int id)
    {
        // ۱. اجرای منطق تجاری (Command)
        // await _mediator.Send(new ToggleProductLikeCommand(id));
        productService.AddLike(id);

        // ۲. واکشی مجدد دیتای همین یک محصول (Query)
        // (نیازی نیست کل لیست محصولات رو واکشی کنی، فقط همین یدونه کارت!)
        // var updatedProductDto = await _mediator.Send(new GetProductByIdQuery(id));

        // ۳. جادوی ASP.NET Core: رندر مجدد ViewComponent و برگشت آن به HTMX
        // پارامتر دوم دقیقا باید هم‌نام ورودی متد Invoke در کلاس ViewComponent شما باشد
        var product = productService.GetProduct(id);
        var productDto = new GetProductListQuery.ProductDto(product.Id, product.Name,  product.Price, product.Like);
        return ViewComponent("ProductCard", new { productDto, isLiked = true });
    }
}

[ViewComponent(Name = "ProductCard")]
public class ProductCardInitializer : FeatureViewComponent
{
    public IViewComponentResult Invoke(GetProductListQuery.ProductDto productDto, bool isLiked)
    {
        // منطق تبدیل ریال به تومان (حفظ Encapsulation در فرزند)
        var priceInToman = (int.Parse(productDto.Price) / 10).ToString("N0");

        var product = productDto with { Price = priceInToman };

        var viewModel = new ProductCardViewModel(product, isLiked);

        // آدرس‌دهی دقیق برای Feature Folder
        return FeatureView(viewModel);
    }
}