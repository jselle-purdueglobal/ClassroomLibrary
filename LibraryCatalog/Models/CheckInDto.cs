using System;

namespace LibraryCatalog.Models;

public class CheckInDto
{
    public int CheckoutId { get; set; }
    public DateTime ReturnDate { get; set; }
}