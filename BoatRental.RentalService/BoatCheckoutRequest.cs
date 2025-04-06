using BoatRental.CommonTypes;

namespace BoatRental.RentalService;

public record BoatCheckoutRequest(
    string BookingNumber,
    string CarRegistrationPlate,
    string CustomerId,
    BoatTypeEnum BoatType,
    DateTime CheckoutDate,
    uint Odometer
);