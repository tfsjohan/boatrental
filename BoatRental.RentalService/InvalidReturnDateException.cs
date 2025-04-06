namespace BoatRental.RentalService;

public class InvalidReturnDateException()
    : ArgumentException("Invalid return date. It must be after the checkout date.")
{
}