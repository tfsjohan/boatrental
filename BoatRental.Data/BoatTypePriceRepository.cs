using BoatRental.CommonTypes;

namespace BoatRental.Data;

public class BoatTypePriceRepository : IBoatTypePriceRepository
{
    private readonly Dictionary<BoatTypeEnum, BoatTypePriceDetails> _carTypePriceDetails = new()
    {
        { BoatTypeEnum.IseoSuper, new BoatTypePriceDetails(BoatTypeEnum.IseoSuper, 800, 0.0m) },
        { BoatTypeEnum.Dolceriva, new BoatTypePriceDetails(BoatTypeEnum.Dolceriva, 100, 15.0m) },
        { BoatTypeEnum.Diable, new BoatTypePriceDetails(BoatTypeEnum.Diable, 1400, 25.0m) }
    };

    public BoatTypePriceDetails GetPriceDetails(BoatTypeEnum boatType)
    {
        return _carTypePriceDetails[boatType];
    }
}