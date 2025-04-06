namespace BoatRental.RentalService;

public record BoatReturnRequest(
    string BookingNumber,
    DateTime ReturnDate,
    uint EngineHours
);