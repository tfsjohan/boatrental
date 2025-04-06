using BoatRental.CommonTypes;

namespace BoatRental.Data;

public record BoatTypePriceDetails(
    BoatTypeEnum BoatType,
    decimal PricePerDay,
    decimal PricePerKm
);