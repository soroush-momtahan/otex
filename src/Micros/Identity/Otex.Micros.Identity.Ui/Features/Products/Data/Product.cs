namespace Otex.Micros.Identity.Ui.Features.Products.Data;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Price { get; set; }
    public int? Like { get; set; }
    

    public void AddLike()
    {
        Like++;
    }
}