using Microsoft.EntityFrameworkCore;
using OnlineShop.Api.Data;
using OnlineShop.Api.Entities;
using OnlineShop.Api.Repositories.Contracts;
using OnlineShop.Models.Dtos;

namespace OnlineShop.Api.Repositories;

public class ShoppingCartRepository : IShoppingCartRepository
{
    private readonly OnlineShopDbContext _onlineShopDbContext;

    public ShoppingCartRepository(OnlineShopDbContext onlineShopDbContext)
    {
        _onlineShopDbContext = onlineShopDbContext;
    }

    private async Task<bool> CartItemExists(int cartId, int productId)
    {
        return await _onlineShopDbContext.CartItems.AnyAsync(c=> c.CartId == cartId && c.ProductId == productId);
    }

    public async Task<CartItem> AddItem(CartItemToAddDto cartItemToAddDto)
    {
        if (await CartItemExists(cartItemToAddDto.CartId, cartItemToAddDto.ProductId) == false)
        {
            var item = await (from product in _onlineShopDbContext.Products
                where product.Id == cartItemToAddDto.ProductId
                select new CartItem
                {
                    CartId = cartItemToAddDto.CartId,
                    ProductId = product.Id,
                    Qty = cartItemToAddDto.Qty
                }).SingleOrDefaultAsync();
            if (item != null)
            {
                var result = await _onlineShopDbContext.CartItems.AddAsync(item);
                await _onlineShopDbContext.SaveChangesAsync();
                return result.Entity;
            }
        }
        return null!;
    }

    public Task<CartItem> UpdateQty(int id, CartItemQtyUpdateDto cartItemQtyUpdateDto)
    {
        throw new NotImplementedException();
    }

    public Task<CartItem> DeleteItem(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<CartItem> GetItem(int id)
    {
        return (await (from cart in _onlineShopDbContext.Carts
            join cartItem in _onlineShopDbContext.CartItems
                on cart.Id equals cartItem.CartId
            where cartItem.Id == id
            select new CartItem
            {
                Id = cartItem.Id,
                ProductId = cartItem.ProductId,
                Qty = cartItem.Qty,
                CartId = cartItem.Id
            }).SingleOrDefaultAsync())!;
    }

    public async Task<IEnumerable<CartItem>> GetItems(int userId)
    {
        return await (from cart in _onlineShopDbContext.Carts
            join cartItem in _onlineShopDbContext.CartItems
                on cart.Id equals cartItem.CartId
            where cart.UserId == userId
            select new CartItem
            {
                Id = cartItem.Id,
                ProductId = cartItem.ProductId,
                Qty = cartItem.Qty,
                CartId = cartItem.Id
            }).ToListAsync();
    }
}