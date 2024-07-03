using CarRental.Data;

namespace CarRental.PriceService;

public class StationWagonPriceCalculator : IPriceCalculator
{
    public decimal CalculatePrice(CarTypePriceDetails details, uint days, uint kilometers)
    {
        return details.PricePerDay * days * 1.3M + details.PricePerKm * kilometers;
    }
}