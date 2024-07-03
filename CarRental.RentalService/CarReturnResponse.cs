namespace CarRental.RentalService;

public record CarReturnResponse(
    string BookingNumber,
    string CarRegistrationPlate,
    string CustomerId,
    DateTime CheckoutDate,
    DateTime ReturnDate,
    int FullDaysRented,
    int OdometerAtCheckout,
    int OdometerAtReturn,
    int DistanceDriven,
    decimal TotalCost
);