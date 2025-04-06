using BoatRental.CommonTypes;

namespace BoatRental.PriceService;

public static class PriceCalculatorFactory
{
    public static IPriceCalculator CreatePriceCalculator(BoatTypeEnum boatType)
    {
        return boatType switch
        {
            BoatTypeEnum.IseoSuper => new IseoSuperPriceCalculator(),
            BoatTypeEnum.Dolceriva => new DolcerivaPriceCalculator(),
            BoatTypeEnum.Diable => new DiablePriceCalculator(),
            _ => throw new ArgumentException("Unknown car type", nameof(boatType))
        };
    }
}