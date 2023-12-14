using System.Data.Entity;

namespace GroceryShopAdmin.Data;

public class Category : DbContext
{
    public int CategoryId { get; set; }

    public List<Product>? Products { get; set; }
    
    public string? Name { get; set; }
}
