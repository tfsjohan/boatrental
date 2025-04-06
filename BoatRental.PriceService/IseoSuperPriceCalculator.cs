using BoatRental.Data;

namespace BoatRental.PriceService;

public class IseoSuperPriceCalculator : IPriceCalculator
{
    public decimal CalculatePrice(BoatTypePriceDetails details, uint hours, uint engineHours)
    {
        return details.PricePerHour * hours;
    }
}