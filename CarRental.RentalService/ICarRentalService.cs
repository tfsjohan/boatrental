namespace CarRental.RentalService;

public interface ICarRentalService
{
    void CheckoutCar(CarCheckoutRequest request);
    
    CarReturnResponse ReturnCar(CarReturnRequest request);

    bool IsCarAvailable(string carRegistrationPlate);
}