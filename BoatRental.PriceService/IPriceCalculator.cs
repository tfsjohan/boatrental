using BoatRental.Data;

namespace BoatRental.PriceService;

public interface IPriceCalculator
{
    decimal CalculatePrice(BoatTypePriceDetails details, uint hours, uint engineHours);
}