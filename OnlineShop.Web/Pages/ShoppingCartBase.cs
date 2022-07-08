using Microsoft.AspNetCore.Components;
using OnlineShop.Models.Dtos;
using OnlineShop.Web.Services.Contracts;

namespace OnlineShop.Web.Pages;

public class ShoppingCartBase : ComponentBase
{
    [Inject]
    public IShoppingCartService ShoppingCartService { get; set; }
    
    public IEnumerable<CartItemDto> ShoppingCartItems { get; set; }
    public string ErrorMessage { get; set; }

    protected override async Task OnInitializedAsync()
    {
        try
        {
            ShoppingCartItems = await ShoppingCartService.GetItems(HardCoded.UserId);
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
        }
    }
}