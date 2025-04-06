namespace BoatRental.RentalService;

public class BookingNumberAlreadyUsedException()
    : InvalidOperationException("Booking number has already been used.")
{
}