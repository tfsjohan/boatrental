using CarRental.CommonTypes;

namespace CarRental.Data;

public interface ICarTypePriceRepository
{
    CarTypePriceDetails GetPriceDetails(CarTypeEnum carType);
}
