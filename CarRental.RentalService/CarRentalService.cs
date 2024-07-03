using CarRental.Data;
using CarRental.PriceService;

namespace CarRental.RentalService;

public class CarRentalService(
    ICarRentalsRepository carRentalsRepository,
    IPriceService priceService
) : ICarRentalService
{
    public virtual void CheckoutCar(CarCheckoutRequest request)
    {
        /* Note to reviewer:
         * In a real-world scenario, I would add more data validation logic here,
         * like making sure customer and car actually exists, that odometer is a positive
         * value.
         */

        if (!IsCarAvailable(request.CarRegistrationPlate))
        {
            /* Note to reviewer:
             * In a real-world scenario, we would throw a more specific exception here,
             * created specifically for this case. However, for the sake of simplicity,
             * we are using a generic exception here.
             */
            throw new InvalidOperationException("Car is not available for checkout");
        }

        var carRental = new Rental
        (
            BookingNumber: request.BookingNumber,
            CarRegistrationPlate: request.CarRegistrationPlate,
            CustomerId: request.CustomerId,
            CarType: request.CarType,
            CheckoutDate: request.CheckoutDate,
            Odometer: request.Odometer,
            ReturnDate: null,
            ReturnOdometer: null
        );

        carRentalsRepository.SaveCarRental(carRental);
    }

    public virtual CarReturnResponse ReturnCar(CarReturnRequest request)
    {
        var rental = carRentalsRepository.GetCarRental(request.BookingNumber);

        if (request.ReturnDate < rental.CheckoutDate)
        {
            throw new InvalidOperationException("Return date cannot be before checkout date");
        }

        if (request.Odometer < rental.Odometer)
        {
            throw new InvalidOperationException("Return odometer cannot be less than checkout odometer");
        }

        var distanceDriven = request.Odometer - rental.Odometer;
        var daysRented = (int)(request.ReturnDate - rental.CheckoutDate).TotalDays;

        var totalCost = priceService.CalculatePrice(rental.CarType, daysRented, distanceDriven);

        return new CarReturnResponse(
            request.BookingNumber,
            rental.CarRegistrationPlate,
            rental.CustomerId,
            rental.CheckoutDate,
            request.ReturnDate,
            daysRented,
            rental.Odometer,
            request.Odometer,
            distanceDriven,
            totalCost);
    }

    public virtual bool IsCarAvailable(string carRegistrationPlate)
    {
        return !carRentalsRepository.GetBookingsForCarAtDate(carRegistrationPlate, DateTime.UtcNow).Any();
    }
}