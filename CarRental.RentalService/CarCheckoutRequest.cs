using CarRental.CommonTypes;

namespace CarRental.RentalService;

public record CarCheckoutRequest(
    string BookingNumber,
    string CarRegistrationPlate,
    string CustomerId,
    CarTypeEnum CarType,
    DateTime CheckoutDate,
    uint Odometer
);