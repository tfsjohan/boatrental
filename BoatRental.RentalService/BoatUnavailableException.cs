namespace BoatRental.RentalService;

public class BoatUnavailableException()
    : InvalidOperationException("The boat is not available for rental. It might already be rented out.")
{
}