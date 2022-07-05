using Microsoft.EntityFrameworkCore;
using OnlineShop.Api.Data;
using OnlineShop.Api.Entities;
using OnlineShop.Api.Repositories.Contracts;

namespace OnlineShop.Api.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly OnlineShopDbContext onlineShopDbContext;

    public ProductRepository(OnlineShopDbContext onlineShopDbContext)
    {
        this.onlineShopDbContext = onlineShopDbContext;
    }
    
    public async Task<IEnumerable<Product>> GetItems()
    {
        var products = await this.onlineShopDbContext.Products.ToListAsync();
        return products;
    }

    public async Task<IEnumerable<ProductCategory>> GetCategories()
    {
        var categories = await this.onlineShopDbContext.ProductCategories.ToListAsync();
        return categories;
    }

    public async Task<Product> GetItem(int id)
    {
        var product = await onlineShopDbContext.Products.FindAsync(id);
        return product;
    }

    public async Task<ProductCategory> GetCategory(int id)
    {
        var category = await onlineShopDbContext.ProductCategories.SingleOrDefaultAsync(c => c.Id == id);
        return category;
    }
}