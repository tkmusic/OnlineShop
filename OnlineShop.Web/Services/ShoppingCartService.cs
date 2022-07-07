using System.Net.Http.Json;
using OnlineShop.Models.Dtos;
using OnlineShop.Web.Services.Contracts;

namespace OnlineShop.Web.Services;

public class ShoppingCartService : IShoppingCartService
{
    private readonly HttpClient _httpClient;

    public ShoppingCartService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    
    public async Task<IEnumerable<CartItemDto>> GetItems(int userId)
    {
        try
        {
            var response = await _httpClient.GetAsync($"api/{userId}/GetItems");

            if (response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                {
                    return Enumerable.Empty<CartItemDto>();
                }

                return (await response.Content.ReadFromJsonAsync<IEnumerable<CartItemDto>>())!;
            }
            else
            {
                var message = response.Content.ReadAsStringAsync();
                throw new Exception($"Http status code:{response.StatusCode} Message: {message}");
            }
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<CartItemDto> AddItem(CartItemToAddDto cartItemToAddDto)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync<CartItemToAddDto>("api/ShoppingCart", cartItemToAddDto);
            if (response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                {
                    return default(CartItemDto)!;
                }

                return (await response.Content.ReadFromJsonAsync<CartItemDto>())!;
            }
            else
            {
                var message = await response.Content.ReadAsStringAsync();
                throw new Exception($"Http status code:{response.StatusCode} Message: {message}");
            }
        }
        catch (Exception)
        {
            throw;
        }
    }

    
}