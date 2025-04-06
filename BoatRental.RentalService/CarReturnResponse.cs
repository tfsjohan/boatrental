namespace BoatRental.RentalService;

public record CarReturnResponse(
    string BookingNumber,
    string CarRegistrationPlate,
    string CustomerId,
    DateTime CheckoutDate,
    DateTime ReturnDate,
    uint FullDaysRented,
    uint OdometerAtCheckout,
    uint OdometerAtReturn,
    uint DistanceDriven,
    decimal TotalCost
);