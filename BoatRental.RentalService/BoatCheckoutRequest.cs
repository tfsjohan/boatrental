using BoatRental.CommonTypes;

namespace BoatRental.RentalService;

public record BoatCheckoutRequest(
    string BookingNumber,
    string BoatRegistrationNumber,
    string CustomerId,
    BoatTypeEnum BoatType,
    DateTime CheckoutDate,
    uint EngineHours
);