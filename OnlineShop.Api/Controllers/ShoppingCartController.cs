using Microsoft.AspNetCore.Mvc;
using OnlineShop.Api.Extensions;
using OnlineShop.Api.Repositories.Contracts;
using OnlineShop.Models.Dtos;

namespace OnlineShop.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ShoppingCartController : ControllerBase
{
    private readonly IShoppingCartRepository _shoppingCartRepository;
    private readonly IProductRepository _productRepository;

    public ShoppingCartController(IShoppingCartRepository shoppingCartRepository, IProductRepository productRepository)
    {
        _shoppingCartRepository = shoppingCartRepository;
        _productRepository = productRepository;
    }

    [HttpGet]
    [Route("{userId}/GetItems")]
    public async Task<ActionResult<IEnumerable<CartItemDto>>> GetItems(int userId)
    {
        try
        {
            var cartItems = await _shoppingCartRepository.GetItems(userId);
            if (cartItems == null)  // TODO: remove Warning
            {
                return NoContent();
            }

            var products = await _productRepository.GetItems();
            if (products == null)
            {
                throw new Exception("No products exist in the System");
            }

            var cartItemsDto = cartItems.ConvertToDto(products);
            return Ok(cartItemsDto);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<CartItemDto>> GetItem(int id)
    {
        try
        {
            var cartItem = await _shoppingCartRepository.GetItem(id);
            if (cartItem == null)  // TODO: remove Warning
            {
                return NotFound();
            }

            var product = await _productRepository.GetItem(cartItem.ProductId);
            if (product == null)  // TODO: remove Warning
            {
                return NotFound();
            }

            var cartItemDto = cartItem.ConvertToDto(product);
            return Ok(cartItemDto);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
    [HttpPost]
    public async Task<ActionResult<CartItemDto>> PostItem([FromBody] CartItemToAddDto cartItemToAddDto)
    {
        try
        {
            var newCartItem = await _shoppingCartRepository.AddItem(cartItemToAddDto);
            if (newCartItem == null) // TODO: remove Warning
            {
                return NoContent();
            }

            var product = await _productRepository.GetItem(newCartItem.ProductId);

            if (product == null)
            {
                throw new Exception(
                    $"Something went Wrong when attempting to retrieve product(productId:({cartItemToAddDto.ProductId})");
            }

            var newCartItemDto = newCartItem.ConvertToDto(product);
            return CreatedAtAction(nameof(GetItem), new { id = newCartItemDto.Id }, newCartItemDto);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}