using CarRental.CommonTypes;

namespace CarRental.PriceService;

public static class PriceCalculatorFactory
{
    public static IPriceCalculator CreatePriceCalculator(CarTypeEnum carType)
    {
        return carType switch
        {
            CarTypeEnum.Compact => new CompactCarPriceCalculator(),
            CarTypeEnum.StationWagon => new StationWagonPriceCalculator(),
            CarTypeEnum.Truck => new TruckPriceCalculator(),
            _ => throw new ArgumentException("Unknown car type", nameof(carType))
        };
    }
}