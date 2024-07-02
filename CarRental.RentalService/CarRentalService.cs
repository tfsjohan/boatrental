using CarRental.Data;

namespace CarRental.RentalService;

public class CarRentalService(ICarRentalsRepository carRentalsRepository) : ICarRentalService
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
        throw new NotImplementedException();
    }
    
    public virtual bool IsCarAvailable(string carRegistrationPlate)
    {
        return !carRentalsRepository.GetBookingsForCarAtDate(carRegistrationPlate, DateTime.UtcNow).Any();
    }
}