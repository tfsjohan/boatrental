using CarRental.CommonTypes;
using CarRental.Data;

namespace CarRental.PriceService;

public interface IPriceService
{
    decimal CalculatePrice(CarTypeEnum carType, int days, int kilometers);
}