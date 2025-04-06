using BoatRental.Data;

namespace BoatRental.PriceService;

public class DolcerivaPriceCalculator : IPriceCalculator
{
    public decimal CalculatePrice(BoatTypePriceDetails details, uint days, uint engineHours)
    {
        return details.PricePerDay * days * 1.3M + details.PricePerKm * engineHours;
    }
}