using GroceryShopAdmin.Data;
using Microsoft.EntityFrameworkCore;

namespace GroceryShopAdmin.Pages;

public partial class Main
{
    private GroceryShopDbContext? _context;

    private bool ShowCreate { get; set; }
    private bool ShowEdit { get; set; }
    private Product? NewProduct { get; set; }

    private Product? ProductToUpdate { get; set; }

    private List<Product> _products = new();

    private List<Category> _categories = new();

    protected override async Task OnInitializedAsync()
    {
        await Task.Run(() => ShowCreate = false);
        await Task.Run(ShowProducts);
    }

    private void ShowCreateForm()
    {
        ShowCreate = true;
        NewProduct = new Product();
    }

    private string? GetCategory(Product product)
    {
        return _categories
            .FirstOrDefault(c => c.CategoryId == product.CategoryId)?.Name;
    }

    private async Task ShowProducts()
    {
        _context ??= await GroceryShopContextFactory.CreateDbContextAsync();

        if (_context is not null)
        {
            _products = await _context.Products!.ToListAsync();
            _categories = await _context.Categories!.ToListAsync();
        }
    }

    //CREATE
    private async Task AddNewProduct()
    {
        _context ??= await GroceryShopContextFactory.CreateDbContextAsync();

        var getProduct = _context.Products!
            .FirstOrDefault(p => p.Name == NewProduct!.Name);

        if (NewProduct is not null)
        {
            if (getProduct is not null)
            {
                if (getProduct.Price != NewProduct.Price)
                    getProduct.Price = NewProduct.Price;

                if (getProduct.CategoryId != NewProduct.CategoryId)
                    getProduct.CategoryId = NewProduct.CategoryId;

                getProduct.Quantity++;
                _context?.Products!.Update(getProduct);
            }
            else
            {
                NewProduct.Quantity = 1;
                _context?.Products!.Add(NewProduct);
                _context?.SaveChangesAsync();
            }
        }

        ShowCreate = false;
        await ShowProducts();
    }

    //READ
    private async Task ShowUpdateForm(Product product)
    {
        _context ??= await GroceryShopContextFactory.CreateDbContextAsync();
        if (_context is not null)
        {
            ProductToUpdate = _context.Products!
                .FirstOrDefault(p => p.ProductId == product.ProductId);

            ShowEdit = true;
        }
    }

    //UPDATE
    private async Task UpdateProduct()
    {
        _context ??= await GroceryShopContextFactory.CreateDbContextAsync();

        if (_context is not null)
        {
            if (ProductToUpdate is not null)
            {
                _context.Products?.Update(ProductToUpdate);
                await _context.SaveChangesAsync();
            }

            ShowEdit = false;
        }
    }

    //DELETE
    public async Task DeleteProduct(Product? product)
    {
        _context ??= await GroceryShopContextFactory.CreateDbContextAsync();
        
        if (_context is not null)
        {
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


        await ShowProducts();
    }
}