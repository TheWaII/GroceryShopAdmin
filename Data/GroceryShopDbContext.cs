using Microsoft.EntityFrameworkCore;

namespace GroceryShopAdmin.Data;

public class GroceryShopDbContext : DbContext
{
    private readonly IConfiguration _configuration;
    
    public DbSet<Category>? Categories { get; set; }
    public DbSet<Product>? Products { get; set; }
    public DbSet<Transaction>? Transactions { get; set; }
    
    public GroceryShopDbContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlite(_configuration.GetConnectionString("DatabaseConnectionString"));
    }
}