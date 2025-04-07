namespace BoatRental.RentalService;

public class InvalidEngineHoursException()
    : InvalidOperationException("Invalid odometer value. It must be more or equal than the odometer value at checkout.")
{
}