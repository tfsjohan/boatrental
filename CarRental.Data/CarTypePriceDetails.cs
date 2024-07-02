using CarRental.CommonTypes;

namespace CarRental.Data;

public record CarTypePriceDetails(
    CarTypeEnum CarType,
    decimal PricePerDay,
    decimal PricePerKm,
    decimal? Factor1,
    decimal? Factor2
);