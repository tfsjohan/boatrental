namespace CarRental.RentalService;

public interface ICarRentalService
{
    void CheckoutCar(CarCheckoutRequest request);

    CarReturnResponse ReturnCar(CarReturnRequest request);

    uint CalculateRentalDays(DateTime checkoutDate, DateTime returnDate);
}