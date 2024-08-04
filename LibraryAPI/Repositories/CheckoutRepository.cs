using System.Data;
using Dapper;
using LibraryAPI.Models;

namespace LibraryAPI.Repositories;

public class CheckoutRepository(IDbConnection connection) : ICheckoutRepository
{
    // Get Checkouts
    public async Task<IEnumerable<Checkout>> GetCheckoutsAsync(int? libraryId = null)
    {
        return await connection.QueryAsync<Checkout>(
            "spGetCheckouts",
            new { LibraryID = libraryId },
            commandType: CommandType.StoredProcedure);
    }
    
    // Add Checkout
    public async Task<bool> AddCheckoutAsync(Checkout checkout)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@StudentID", checkout.StudentId);
        parameters.Add("@BookID", checkout.BookId);
        parameters.Add("@LibraryID", checkout.LibraryId);
        parameters.Add("@CheckoutDate", checkout.CheckoutDate);

        var result = await connection.ExecuteScalarAsync<int>(
            "spAddCheckout",
            parameters,
            commandType: CommandType.StoredProcedure
        );
        
        return result > 0;
    }
    
    // Return Checkout
    public async Task<bool> ReturnCheckoutAsync(ReturnCheckoutDto returnCheckoutDto)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@CheckoutId", returnCheckoutDto.CheckoutId);
        parameters.Add("@ReturnDate", returnCheckoutDto.ReturnDate);

        var result = await connection.ExecuteScalarAsync<int>(
            "spReturnCheckout",
            parameters,
            commandType: CommandType.StoredProcedure
        );

        return result > 0;
    }
}