using BoatRental.Data;

namespace BoatRental.PriceService;

public class IseoSuperPriceCalculator : IPriceCalculator
{
    // Iseo Super: Pris = (basTimHyra * antalTimmar) + (basGangTidPris * antalGangTimmar) 
    public decimal CalculatePrice(BoatTypePriceDetails details, uint hours, uint engineHours)
    {
        return details.PricePerHour * hours * details.SkipperPrice * engineHours;
    }
}