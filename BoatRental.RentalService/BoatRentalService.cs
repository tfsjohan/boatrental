using BoatRental.Data;
using BoatRental.PriceService;

namespace BoatRental.RentalService;

// ReSharper disable once ClassWithVirtualMembersNeverInherited.Global
public class BoatRentalService(
    IBoatRentalsRepository boatRentalsRepository,
    IPriceService priceService
) : IBoatRentalService
{
    public virtual void CheckoutBoat(BoatCheckoutRequest request)
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
            BoatRegistrationNumber = request.CarRegistrationPlate,
            CustomerId = request.CustomerId,
            BoatType = request.BoatType,
            CheckoutDate = request.CheckoutDate,
            EngineHours = request.Odometer,
        };

        boatRentalsRepository.SaveBoatRental(carRental);
    }

    public virtual CarReturnResponse ReturnBoat(BoatReturnRequest request)
    {
        var rental = boatRentalsRepository.GetBoatRental(request.BookingNumber);

        EnsureValidRequest(request, rental);

        var distanceDriven = request.Odometer - rental.EngineHours;
        var daysRented = CalculateRentalDays(rental.CheckoutDate, request.ReturnDate);

        var totalCost = priceService.CalculatePrice(
            rental.BoatType,
            daysRented,
            distanceDriven);

        rental.ReturnOdometer = request.Odometer;
        rental.ReturnDate = request.ReturnDate;

        boatRentalsRepository.SaveBoatRental(rental);

        return new CarReturnResponse(
            request.BookingNumber,
            rental.BoatRegistrationNumber,
            rental.CustomerId,
            rental.CheckoutDate,
            request.ReturnDate,
            daysRented,
            rental.EngineHours,
            request.Odometer,
            distanceDriven,
            totalCost);
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
            throw new ArgumentException(
                "Return date cannot be before checkout date",
                nameof(returnDate));
        }

        return (uint)Math.Ceiling((returnDate - checkoutDate).TotalDays);
    }

    private bool IsCarAvailable(string carRegistrationPlate)
    {
        return !boatRentalsRepository
            .GetBookingsForBoatAtDate(carRegistrationPlate, DateTime.UtcNow)
            .Any();
    }

    private static bool IsReturned(Rental rental) => rental.ReturnDate is not null;

    private Rental? GetRental(string bookingNumber)
    {
        try
        {
            return boatRentalsRepository.GetBoatRental(bookingNumber);
        }
        catch (KeyNotFoundException)
        {
            return null;
        }
    }

    private static void EnsureValidRequest(BoatReturnRequest request, Rental rental)
    {
        if (IsReturned(rental))
        {
            throw new CarAlreadyReturnedException();
        }
        
        if (request.ReturnDate < rental.CheckoutDate)
        {
            throw new InvalidReturnDateException();
        }

        if (request.Odometer < rental.EngineHours)
        {
            throw new InvalidOdometerException();
        }
    }
}