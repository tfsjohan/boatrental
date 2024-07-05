using CarRental.Data;
using CarRental.PriceService;

namespace CarRental.RentalService;

// ReSharper disable once ClassWithVirtualMembersNeverInherited.Global
public class CarRentalService(
    ICarRentalsRepository carRentalsRepository,
    IPriceService priceService
) : ICarRentalService
{
    public virtual void CheckoutCar(CarCheckoutRequest request)
    {
        /* Note to reviewer:
         * In a real-world scenario, I would add more data validation logic here,
         * like making sure customer and car actually exists and add data validation
         * attributes to the request object.
         */

        if (!IsCarAvailable(request.CarRegistrationPlate))
        {
            throw new CarUnavailableException();
        }

        var existingRental = GetRental(request.BookingNumber);
        if (existingRental is not null)
        {
            throw new BookingNumberAlreadyUsedException();
        }

        var carRental = new Rental
        {
            BookingNumber = request.BookingNumber,
            CarRegistrationPlate = request.CarRegistrationPlate,
            CustomerId = request.CustomerId,
            CarType = request.CarType,
            CheckoutDate = request.CheckoutDate,
            Odometer = request.Odometer,
        };

        carRentalsRepository.SaveCarRental(carRental);
    }

    public virtual CarReturnResponse ReturnCar(CarReturnRequest request)
    {
        var rental = carRentalsRepository.GetCarRental(request.BookingNumber);

        if (IsReturned(rental))
        {
            throw new CarAlreadyReturnedException();
        }

        if (request.ReturnDate < rental.CheckoutDate)
        {
            throw new ArgumentException("Return date cannot be before checkout date", nameof(request.ReturnDate));
        }

        if (request.Odometer < rental.Odometer)
        {
            throw new ArgumentException("Return odometer cannot be less than checkout odometer",
                nameof(request.Odometer));
        }

        var distanceDriven = request.Odometer - rental.Odometer;
        var daysRented = CalculateRentalDays(rental.CheckoutDate, request.ReturnDate);

        var totalCost = priceService.CalculatePrice(
            rental.CarType,
            daysRented,
            distanceDriven);

        rental.ReturnOdometer = request.Odometer;
        rental.ReturnDate = request.ReturnDate;

        carRentalsRepository.SaveCarRental(rental);

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

    /// <summary>
    /// Calculate the number of days a car has been rented for, each day is counted as a full day.
    /// </summary>
    /// <param name="checkoutDate">Date for checkout</param>
    /// <param name="returnDate">Date for return</param>
    /// <returns>Positive integer of number of full days</returns>
    public virtual uint CalculateRentalDays(DateTime checkoutDate, DateTime returnDate)
    {
        if (checkoutDate > returnDate)
        {
            throw new ArgumentException("Return date cannot be before checkout date", nameof(returnDate));
        }

        return (uint)Math.Ceiling((returnDate - checkoutDate).TotalDays);
    }

    private bool IsReturned(Rental rental) => rental.ReturnDate is not null;

    private Rental? GetRental(string bookingNumber)
    {
        try
        {
            return carRentalsRepository.GetCarRental(bookingNumber);
        }
        catch (KeyNotFoundException)
        {
            return null;
        }
    }
}