namespace BoatRental.RentalService;

public class InvalidOdometerException()
    : InvalidOperationException("Invalid odometer value. It must be more or equal than the odometer value at checkout.")
{
}