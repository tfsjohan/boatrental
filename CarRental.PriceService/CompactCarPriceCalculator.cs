using CarRental.Data;

namespace CarRental.PriceService;

public class CompactCarPriceCalculator : IPriceCalculator
{
    public decimal CalculatePrice(CarTypePriceDetails details, int days, int kilometers)
    {
        return details.PricePerDay * days;
    }
}