using BoatRental.CommonTypes;

namespace BoatRental.Data;

public class BoatTypePriceRepository : IBoatTypePriceRepository
{
    private readonly Dictionary<BoatTypeEnum, BoatTypePriceDetails> _boatTypePriceDetails = new()
    {
        { BoatTypeEnum.IseoSuper, new BoatTypePriceDetails(BoatTypeEnum.IseoSuper, 800, 0.0m, 0) },
        { BoatTypeEnum.Dolceriva, new BoatTypePriceDetails(BoatTypeEnum.Dolceriva, 100, 15.0m, 0) },
        { BoatTypeEnum.Diable, new BoatTypePriceDetails(BoatTypeEnum.Diable, 1400, 25.0m, 4000) }
    };

    public BoatTypePriceDetails GetPriceDetails(BoatTypeEnum boatType)
    {
        return _boatTypePriceDetails[boatType];
    }
}