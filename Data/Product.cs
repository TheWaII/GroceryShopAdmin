namespace GroceryShopAdmin.Data;

using System.Data.Entity;

public class Product : DbContext
{
    public int ProductId { get; set; }
    
    public string? Name { get; set; }
    
    public decimal Price { get; set; }
     
    public int Quantity { get; set; }
    
    public int CategoryId { get; set; }
    
    public List<Category>? Categories { get; set; }
}