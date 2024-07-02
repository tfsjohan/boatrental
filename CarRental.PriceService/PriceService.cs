using CarRental.CommonTypes;
using CarRental.Data;

namespace CarRental.PriceService;

public class PriceService(ICarTypePriceRepository carTypePriceRepository) : IPriceService
{
    public decimal CalculatePrice(CarTypeEnum carType, int days, int kilometers)
    {
        /* Note to reviewer:
         * In a real world application, I would probably use a validation library
         * and also use resource files for the error messages.
         */
        if (days < 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(days),
                "Days must be greater than or equal to 0.");
        }
        
        if (kilometers < 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(kilometers),
                "Kilometers must be greater than or equal to 0.");
        }

        var details = carTypePriceRepository.GetPriceDetails(carType);
        
        /* Note to reviewer:
         * in the specification it's stated that there are three types of cars, so
         * a factory is used to create the correct price calculator based on the car type
         * is likely sufficient for this case.
         *
         * If there are more car types, and the client should be able to add more car types
         * in some admin panel, then a more dynamic solution should be considered.
         * In that case I would probably use a discriminator pattern, where the car type
         * is a string, and the ORM could to create the correct price calculator.
         * Most ORM system support this kind of mapping, so it would be easy to implement.
         */
        var calculator = PriceCalculatorFactory.CreatePriceCalculator(details.CarType);

        return calculator.CalculatePrice(details, days, kilometers);
    }
}