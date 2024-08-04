using LibraryAPI.Models;

namespace LibraryAPI.Repositories;

public interface ICheckoutRepository
{
    Task<IEnumerable<Checkout>> GetCheckoutsAsync(int? libraryId = null);
    Task<bool> AddCheckoutAsync(Checkout checkout);
    Task<bool> ReturnCheckoutAsync(ReturnCheckoutDto returnCheckoutDto);
}