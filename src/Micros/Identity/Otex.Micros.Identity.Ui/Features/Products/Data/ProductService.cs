namespace Otex.Micros.Identity.Ui.Features.Products.Data;

public class ProductService
{
    private List<Product> products { get; set; } =
    [
        new Product
        {
            Id = 1,
            Name = "لنت جلو پراید",
            Price = "120",
            Like = 12
        },
        new Product
        {
            Id = 2,
            Name = "لنت جلو پژو",
            Price = "1300",
            Like = 10
        },
        new Product
        {
            Id = 3,
            Name = "لنت جلو سمند",
            Price = "1400",
            Like = 12
        },
        new Product
        {
            Id = 4,
            Name = "لنت جلو ساینا",
            Price = "2500",
            Like = 14
        }
    ];

    public List<Product> GetProducts()
    {
        return products;
        
    }

    public Product? GetProduct(int id)
    {
        var product = products
            .FirstOrDefault(p => p.Id == id);
        return product;
    }

    public void AddLike(int id)
    {
        var product = GetProduct(id);
        product.Like++;
    }
}