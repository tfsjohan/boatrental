using CarRental.CommonTypes;

namespace CarRental.PriceService;

public interface IPriceService
{
    decimal CalculatePrice(CarTypeEnum carType, uint days, uint kilometers);
}