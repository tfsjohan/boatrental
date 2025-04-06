using BoatRental.CommonTypes;

namespace BoatRental.PriceService;

public interface IPriceService
{
    decimal CalculatePrice(BoatTypeEnum boatType, uint hours, uint engineHours);
}