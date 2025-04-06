using BoatRental.CommonTypes;
using BoatRental.Data;

namespace BoatRental.PriceService;

public class PriceService(IBoatTypePriceRepository boatTypePriceRepository) : IPriceService
{
    public decimal CalculatePrice(BoatTypeEnum boatType, uint hours, uint engineHours)
    {
        var details = boatTypePriceRepository.GetPriceDetails(boatType);

        /* Note to reviewer:
         * In the specification it's stated that there are three types of cars, so
         * a factory is used to create the correct price calculator based on the car type
         * is likely sufficient for this case.
         *
         * If there are more car types, or if the client should be able to add more car types
         * in some admin panel, then a more dynamic solution should be considered.
         * In that case I would probably use a discriminator pattern, where the car type
         * is a string, and the ORM could to create the correct price calculator.
         * Most ORM system support this kind of mapping, so it would be easy to implement.
         *
         * I like the use of subclasses, because it makes it really flexible to have
         * very different types of calculations for different car types. Like,
         * a regular car is based on days and distance, but a convertable might have
         * different pricing based on season or a weather forecast.
         */
        var calculator = PriceCalculatorFactory.CreatePriceCalculator(details.BoatType);

        return calculator.CalculatePrice(details, hours, engineHours);
    }
}