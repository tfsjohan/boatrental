using BoatRental.Data;

namespace BoatRental.PriceService;

public class DolcerivaPriceCalculator : IPriceCalculator
{
    // Dolceriva: Pris = (basTimHyra * antalTimmar * 1,3) + (basGangTidPris * antalGangTimmar * 1,5) 
    public decimal CalculatePrice(BoatTypePriceDetails details, uint hours, uint engineHours)
    {
        return details.PricePerHour * hours * 1.3M + details.PricePerEngineHour * engineHours * 1.5M;
    }
}