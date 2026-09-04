using Microsoft.AspNetCore.Mvc;
using Otex.Micros.Identity.Ui.Core.Tools;
using Otex.Micros.Identity.Ui.Features.Products.Data;

namespace Otex.Micros.Identity.Ui.Features.Products.Components.ProductList;

public record GetProductListQuery(
    List<GetProductListQuery.ProductDto> ProductDtos)
{
    public record ProductDto(int Id,string Name, string Price, int? Like);
}

public record ProductListViewModel(
    List<GetProductListQuery.ProductDto> Data,
    bool IsLoading,
    bool HasError,
    string ErrorMessage);

[ViewComponent(Name = "ProductList")]
public class ProductListInitializer(ProductService productService) : FeatureViewComponent
{
    public IViewComponentResult Invoke()
    {
        #region MediatR

        var data = productService.GetProducts();

        var productDtos = data
            .Select(x => 
                new GetProductListQuery.ProductDto(x.Id, x.Name, x.Price, x.Like))
            .ToList();

        GetProductListQuery query = new GetProductListQuery(productDtos);

        #endregion

        var productListViewModel = new ProductListViewModel(
            query.ProductDtos, false, false, null);
        return FeatureView(productListViewModel);
    }
}

public class ProductListController : Controller
{
    
}