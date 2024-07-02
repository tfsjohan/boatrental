using CarRental.Data;

namespace CarRental.PriceService;

public class TruckPriceCalculator : IPriceCalculator
{
    public decimal CalculatePrice(CarTypePriceDetails details, int days, int kilometers)
    {
        return details.PricePerDay * days * 1.5M + details.PricePerKm * kilometers * 1.5M;
    }
}