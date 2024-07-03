using CarRental.Data;

namespace CarRental.PriceService;

public interface IPriceCalculator
{
    decimal CalculatePrice(CarTypePriceDetails details, uint days, uint kilometers);
}