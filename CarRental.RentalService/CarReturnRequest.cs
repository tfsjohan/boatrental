namespace CarRental.RentalService;

public record CarReturnRequest(
    string BookingNumber,
    DateTime ReturnDate,
    uint Odometer
);