namespace CarRental.RentalService;

public class CarAlreadyReturnedException()
    : InvalidOperationException("Car has already been returned.");