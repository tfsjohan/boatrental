using BoatRental.Data;

namespace BoatRental.PriceService;

public class DiablePriceCalculator : IPriceCalculator
{
    public decimal CalculatePrice(BoatTypePriceDetails details, uint days, uint engineHours)
    {
        return details.PricePerDay * days * 1.5M + details.PricePerKm * engineHours * 1.5M;
    }
}