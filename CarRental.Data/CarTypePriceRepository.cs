using CarRental.CommonTypes;

namespace CarRental.Data;

public class CarTypePriceRepository : ICarTypePriceRepository
{
    private readonly Dictionary<CarTypeEnum, CarTypePriceDetails> _carTypePriceDetails = new()
    {
        { CarTypeEnum.Compact, new CarTypePriceDetails(CarTypeEnum.Compact, 50, 0.0m, null, null) },
        { CarTypeEnum.StationWagon, new CarTypePriceDetails(CarTypeEnum.StationWagon, 70, 0.3m, 1.3m, null) },
        { CarTypeEnum.Truck, new CarTypePriceDetails(CarTypeEnum.Truck, 100, 1.5m, 1.5m, 1.5m) }
    };

    public CarTypePriceDetails GetPriceDetails(CarTypeEnum carType)
    {
        return _carTypePriceDetails[carType];
    }
}