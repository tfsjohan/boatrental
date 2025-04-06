using BoatRental.Data;

namespace BoatRental.PriceService;

public class IseoSuperPriceCalculator : IPriceCalculator
{
    public decimal CalculatePrice(BoatTypePriceDetails details, uint days, uint engineHours)
    {
        return details.PricePerDay * days;
    }
}