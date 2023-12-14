using System.Text.Json;
using GroceryShopAdmin.Data;
using Microsoft.EntityFrameworkCore;

namespace GroceryShopAdmin.Pages;

public partial class Shop
{
    private List<Product> _products = new();
    private GroceryShopDbContext? _context;

    private Product? NewProduct { get; set; }

    private Product? ProductToUpdate { get; set; }


    protected override Task OnInitializedAsync()
    {
        return Task.Run(ShowProducts);
    }

    private async Task ShowProducts()
    {
        _context ??= await GroceryShopContextFactory.CreateDbContextAsync();

        if (_context is not null)
        {
            _products = await _context.Products!.ToListAsync();
        }
    }

    private async Task BuyProduct(Product product)
    {
        _context ??= await GroceryShopContextFactory.CreateDbContextAsync();

        _context.Transactions!.Add(new Transaction()
        {
            TransactionDate = DateTime.Now,
            ProductId = product.ProductId,
        });

        await _context.SaveChangesAsync();

        if (product?.Quantity is 1)
        {
            _context.Products!.Remove(product);
            await _context.SaveChangesAsync();
        }
        else if (product is not null)
        {
            _context.Products!
                .FirstOrDefault(x => x.ProductId == product.ProductId)!
                .Quantity--;

            _context.Products!.Update(product);
            await _context.SaveChangesAsync();
        }
    }

    private static string GetImage(Product product)
    {
        var client = new HttpClient();
        var request = new HttpRequestMessage(HttpMethod.Get,
            $"https://api.pexels.com/v1/search?query={product.Name}&per_page=1");
        request.Headers.Add("Authorization", "EYE65v9fOsryAvRsW1GCjX2K0YfkG0A5pWx29hn74pWVRkHVVEdWEdaT");
        request.Headers.Add("Cookie",
            "__cf_bm=OU8GhYbz0L1FYwpSDZop6bombsdpV1Tb_S7y3oJ6RmI-1702138209-1-AefQJLJksTbry4B5g/d4MGalxzdFB96AxTJ3AtgNcIAMSBEMWOMJ/uR+Aa+xyVhT2RpTUzFNH3fydU9p4dnDWJU=");
        var response = client.Send(request);

        var jsonString = response.Content.ReadAsStringAsync().Result;
        var document = JsonDocument.Parse(jsonString).RootElement;

        var tinyUrl = document
            .GetProperty("photos")[0]
            .GetProperty("src")
            .GetProperty("tiny")
            .GetString();

        var imageString = new Uri(tinyUrl!).GetLeftPart(UriPartial.Path);
        
        return imageString;
    }
}