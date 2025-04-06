using BoatRental.Data;

namespace BoatRental.PriceService;

public class DiablePriceCalculator : IPriceCalculator
{
    public decimal CalculatePrice(BoatTypePriceDetails details, uint hours, uint engineHours)
    {
        return details.PricePerHour * hours * 1.5M + details.PricePerEngineHour * engineHours * 1.5M;
    }
}