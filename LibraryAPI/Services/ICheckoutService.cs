using LibraryAPI.Models;

namespace LibraryAPI.Services;

public interface ICheckoutService
{
    Task<IEnumerable<CheckoutDto>> GetActiveCheckoutsAsync(int? libraryId = null);
    Task<bool> AddCheckoutAsync(AddCheckoutDto checkout);
    Task<bool> ReturnCheckoutAsync(ReturnCheckoutDto returnCheckoutDto);
}