using System.Data.Entity;

namespace GroceryShopAdmin.Data;

public class Transaction: DbContext
{
    public int TransactionId { get; set; }
    
    public DateTime TransactionDate { get; set; }

    public int ProductId { get; set; }
    
    public Product? Product { get; set; }
}