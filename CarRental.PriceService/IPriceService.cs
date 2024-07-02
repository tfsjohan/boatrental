using CarRental.CommonTypes;

namespace CarRental.PriceService;

public interface IPriceService
{
    decimal CalculatePrice(CarTypeEnum carType, int days, int kilometers);
}