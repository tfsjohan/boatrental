using BoatRental.CommonTypes;

namespace BoatRental.Data;

public class Rental()
{
    public required string BookingNumber { get; set; }
    public required string BoatRegistrationNumber { get; set; }
    public required string CustomerId { get; set; }
    public required BoatTypeEnum BoatType { get; set; }
    public required DateTime CheckoutDate { get; set; }
    public required uint EngineHours { get; set; }
    public DateTime? ReturnDate { get; set; }
    public uint? ReturnEngineHours { get; set; }
};