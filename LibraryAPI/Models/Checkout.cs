namespace LibraryAPI.Models;

public class Checkout
{
    public int CheckoutId { get; set; }
    public int StudentId { get; set; }
    public int BookId { get; set; }
    public int LibraryId { get; set; }
    public DateTime CheckoutDate { get; set; }
    public DateTime? ReturnDate { get; set; }
}