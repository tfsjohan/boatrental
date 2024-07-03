using CarRental.Data;

namespace CarRental.PriceService;

public class TruckPriceCalculator : IPriceCalculator
{
    public decimal CalculatePrice(CarTypePriceDetails details, uint days, uint kilometers)
    {
        return details.PricePerDay * days * 1.5M + details.PricePerKm * kilometers * 1.5M;
    }
}