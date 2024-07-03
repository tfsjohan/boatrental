using CarRental.CommonTypes;

namespace CarRental.Data;

public class Rental()
{
    public required string BookingNumber { get; set; }
    public required string CarRegistrationPlate { get; set; }
    public required string CustomerId { get; set; }
    public required CarTypeEnum CarType { get; set; }
    public required DateTime CheckoutDate { get; set; }
    public required int Odometer { get; set; }
    public DateTime? ReturnDate { get; set; }
    public int? ReturnOdometer { get; set; }
};