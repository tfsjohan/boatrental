using BoatRental.Data;

namespace BoatRental.PriceService;

public class DolcerivaPriceCalculator : IPriceCalculator
{
    public decimal CalculatePrice(BoatTypePriceDetails details, uint hours, uint engineHours)
    {
        return details.PricePerHour * hours * 1.3M + details.PricePerEngineHour * engineHours;
    }
}