namespace BoatRental.RentalService;

public class BoatAlreadyReturnedException()
    : InvalidOperationException("Boat has already been returned.");