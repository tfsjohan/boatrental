using CarRental.Data;

namespace CarRental.PriceService;

public class StationWagonPriceCalculator : IPriceCalculator
{
    public decimal CalculatePrice(CarTypePriceDetails details, int days, int kilometers)
    {
        return details.PricePerDay * days * 1.3M + details.PricePerKm * kilometers;
    }
}