namespace BoatRental.Data;

public class BoatRentalsRepository : IBoatRentalsRepository
{
    private readonly Dictionary<string, Rental> _repository = new();

    public IEnumerable<Rental> GetRentals()
    {
        return _repository.Values.OrderBy(x => x.CheckoutDate);
    }

    public Rental GetBoatRental(string bookingNumber)
    {
        return _repository[bookingNumber];
    }

    public void SaveBoatRental(Rental rental)
    {
        _repository[rental.BookingNumber] = rental;
    }

    public IEnumerable<Rental> GetBookingsForBoatAtDate(string boatRegistrationNumber, DateTime date)
    {
        // We are assuming that the checkout date is always before the return date
        return _repository.Values
            .Where(r => r.BoatRegistrationNumber == boatRegistrationNumber)
            .Where(r => r.CheckoutDate <= date && (r.ReturnDate == null || r.ReturnDate >= date))
            .OrderBy(r => r.CheckoutDate);
    }
}