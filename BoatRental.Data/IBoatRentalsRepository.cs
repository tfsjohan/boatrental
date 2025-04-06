namespace BoatRental.Data;

public interface IBoatRentalsRepository
{
    IEnumerable<Rental> GetRentals();

    Rental GetBoatRental(string bookingNumber);

    void SaveBoatRental(Rental rental);

    IEnumerable<Rental> GetBookingsForBoatAtDate(string boatRegistrationNumber, DateTime date);
}