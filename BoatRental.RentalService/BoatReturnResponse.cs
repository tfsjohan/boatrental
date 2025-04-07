namespace BoatRental.RentalService;

public record BoatReturnResponse(
    string BookingNumber,
    string BoatRegistrationNumber,
    string CustomerId,
    DateTime CheckoutDate,
    DateTime ReturnDate,
    uint FullHoursRented,
    uint EngineHoursAtCheckout,
    uint EngineHoursAtReturn,
    uint EngineHoursUsed,
    decimal TotalCost
);