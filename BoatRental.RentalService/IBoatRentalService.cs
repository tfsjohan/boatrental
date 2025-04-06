namespace BoatRental.RentalService;

public interface IBoatRentalService
{
    void CheckoutBoat(BoatCheckoutRequest request);

    CarReturnResponse ReturnBoat(BoatReturnRequest request);

    uint CalculateRentalDays(DateTime checkoutDate, DateTime returnDate);
}