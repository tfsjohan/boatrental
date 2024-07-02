using CarRental.CommonTypes;

namespace CarRental.Data;

public class CarTypePriceRepository : ICarTypePriceRepository
{
    private readonly Dictionary<CarTypeEnum, CarTypePriceDetails> _carTypePriceDetails = new()
    {
        { CarTypeEnum.Compact, new CarTypePriceDetails(CarTypeEnum.Compact, 50, 0.0m) },
        { CarTypeEnum.StationWagon, new CarTypePriceDetails(CarTypeEnum.StationWagon, 70, 15.0m) },
        { CarTypeEnum.Truck, new CarTypePriceDetails(CarTypeEnum.Truck, 100, 20.0m) }
    };

    public CarTypePriceDetails GetPriceDetails(CarTypeEnum carType)
    {
        return _carTypePriceDetails[carType];
    }
}