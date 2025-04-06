namespace BoatRental.RentalService;

public interface IBoatRentalService
{
    void CheckoutBoat(BoatCheckoutRequest request);

    CarReturnResponse ReturnBoat(BoatReturnRequest request);

    uint CalculateRentalHours(DateTime checkoutDate, DateTime returnDate);
}