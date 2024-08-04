using LibraryAPI.Models;
using LibraryAPI.Repositories;

namespace LibraryAPI.Services;

public class CheckoutService(ICheckoutRepository checkoutRepository) : ICheckoutService
{
    // Get Active Checkouts
    public async Task<IEnumerable<CheckoutDto>> GetActiveCheckoutsAsync(int? libraryId = null)
    {
        var checkouts=  await checkoutRepository.GetCheckoutsAsync(libraryId);
        var activeCheckouts = checkouts.Where(checkout => checkout.ReturnDate == null);
        return activeCheckouts.Select(checkout => new CheckoutDto
        {
            CheckoutId = checkout.CheckoutId,
            StudentId = checkout.StudentId,
            BookId = checkout.BookId
        });
    }
    
    // Add Checkout
    public async Task<bool> AddCheckoutAsync(AddCheckoutDto checkoutDto)
    {
        var checkout = new Checkout
        {
            StudentId = checkoutDto.StudentId,
            BookId = checkoutDto.BookId,
            LibraryId = checkoutDto.LibraryId,
            CheckoutDate = DateTime.UtcNow
        };
        
        return await checkoutRepository.AddCheckoutAsync(checkout);
    }
    
    // Return Checkout
    public async Task<bool> ReturnCheckoutAsync(ReturnCheckoutDto returnCheckoutDto)
    {
        returnCheckoutDto.ReturnDate = DateTime.UtcNow;
        return await checkoutRepository.ReturnCheckoutAsync(returnCheckoutDto);
    }
}