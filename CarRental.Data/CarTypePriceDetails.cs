using CarRental.CommonTypes;

namespace CarRental.Data;

public record CarTypePriceDetails(
    CarTypeEnum CarType,
    decimal PricePerDay,
    decimal PricePerKm
);