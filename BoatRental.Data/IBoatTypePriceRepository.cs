using BoatRental.CommonTypes;

namespace BoatRental.Data;

public interface IBoatTypePriceRepository
{
    BoatTypePriceDetails GetPriceDetails(BoatTypeEnum boatType);
}
