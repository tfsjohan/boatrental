using CarRental.CommonTypes;

namespace CarRental.Data;

public record Rental(
    string BookingNumber, 
    string CarRegistrationPlate, 
    string CustomerId, 
    CarTypeEnum CarType, 
    DateTime CheckoutDate, 
    int Odometer, 
    DateTime? ReturnDate, 
    int? ReturnOdometer
);