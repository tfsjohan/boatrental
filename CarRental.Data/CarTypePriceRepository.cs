using CarRental.CommonTypes;

namespace CarRental.Data;

public class CarTypePriceRepository : ICarTypePriceRepository
{
    private readonly Dictionary<CarTypeEnum, CarTypePriceDetails> _carTypePriceDetails = new()
    {
        { CarTypeEnum.Compact, new CarTypePriceDetails(CarTypeEnum.Compact, 800, 0.0m) },
        { CarTypeEnum.StationWagon, new CarTypePriceDetails(CarTypeEnum.StationWagon, 100, 15.0m) },
        { CarTypeEnum.Truck, new CarTypePriceDetails(CarTypeEnum.Truck, 1400, 25.0m) }
    };

    public CarTypePriceDetails GetPriceDetails(CarTypeEnum carType)
    {
        return _carTypePriceDetails[carType];
    }
}