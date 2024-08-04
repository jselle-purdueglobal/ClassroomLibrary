using LibraryAPI.Models;
using LibraryAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace LibraryAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CheckoutsController(ICheckoutService checkoutService) : ControllerBase
{
    // Get Active Checkouts
    [HttpGet("active/{libraryId:int}")]
    public async Task<ActionResult<IEnumerable<CheckoutDto>>> GetCheckouts(int libraryId)
    {
        var checkouts = await checkoutService.GetActiveCheckoutsAsync(libraryId);
        return Ok(checkouts);
    }
    
    // Add Checkout
    [HttpPost]
    public async Task<ActionResult<AddCheckoutDto>> AddCheckout([FromBody] AddCheckoutDto checkout)
    {
        var result = await checkoutService.AddCheckoutAsync(checkout);
        if (result)
        {
            return Ok(new { Message = "Checkout successful" });
        }
        
        return BadRequest(new { Message = "Checkout failed" });
    }
    
    // Return Checkout
    [HttpPost("return")]
    public async Task<ActionResult<ReturnCheckoutDto>> ReturnCheckout([FromBody] ReturnCheckoutDto returnCheckoutDto)
    {
        var result = await checkoutService.ReturnCheckoutAsync(returnCheckoutDto);
        if (result)
        {
            return Ok(new { Message = "Return successful" });
        }

        return BadRequest(new { Message = "Checkout Return failed" });
    }
    
}