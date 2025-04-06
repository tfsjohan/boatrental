using BoatRental.CommonTypes;

namespace BoatRental.Data;

public record BoatTypePriceDetails(
    BoatTypeEnum BoatType,
    decimal PricePerHour,
    decimal PricePerEngineHour,
    decimal SkipperPrice
);