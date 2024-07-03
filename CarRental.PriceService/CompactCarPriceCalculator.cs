using CarRental.Data;

namespace CarRental.PriceService;

public class CompactCarPriceCalculator : IPriceCalculator
{
    public decimal CalculatePrice(CarTypePriceDetails details, uint days, uint kilometers)
    {
        return details.PricePerDay * days;
    }
}