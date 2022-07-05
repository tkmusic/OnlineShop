using OnlineShop.Models.Dtos;

namespace OnlineShop.Web.Services.Contracts;

public interface IProductService
{
    Task<IEnumerable<ProductDto>?> GetItems();
    Task<ProductDto?> GetItem(int id);
}