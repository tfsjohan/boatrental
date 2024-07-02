using CarRental.CommonTypes;
using CarRental.Data;

namespace CarRental.PriceService;

public class PriceService(ICarTypePriceRepository carTypePriceRepository) : IPriceService
{
    public decimal CalculatePrice(CarTypeEnum carType, int days, int kilometers)
    {
        throw new NotImplementedException();
    }
    
}