using BoatRental.Data;

namespace BoatRental.PriceService;

public class DiablePriceCalculator : IPriceCalculator
{
    // Diable: Pris = (basTimHyra * antalTimmar * 2,1) + (basGangTidPris * antalGangTimmar * 4) + skeppare 
    public decimal CalculatePrice(BoatTypePriceDetails details, uint hours, uint engineHours)
    {
        return details.PricePerHour * hours * 2.1M + details.PricePerEngineHour * engineHours + details.SkipperPrice;
    }
}