namespace BoatRental.RentalService;

public class CarUnavailableException()
    : InvalidOperationException("The car is not available for rental. It might already be rented out.")
{
}