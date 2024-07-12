namespace CarRental.RentalService;

public class BookingNumberAlreadyUsedException()
    : InvalidOperationException("Booking number has already been used.")
{
}