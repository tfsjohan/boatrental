namespace BoatRental.RentalService;

public interface IBoatRentalService
{
    void CheckoutBoat(BoatCheckoutRequest request);

    BoatReturnResponse ReturnBoat(BoatReturnRequest request);

    uint CalculateRentalHours(DateTime checkoutDate, DateTime returnDate);
}