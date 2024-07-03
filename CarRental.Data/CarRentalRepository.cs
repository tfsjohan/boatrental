namespace CarRental.Data;

public class CarRentalRepository : ICarRentalsRepository
{
    private readonly Dictionary<string, Rental> _repository = new();

    public IEnumerable<Rental> GetRentals()
    {
        return _repository.Values.OrderBy(x => x.CheckoutDate);
    }

    public Rental GetCarRental(string bookingNumber)
    {
        return _repository[bookingNumber];
    }

    public void SaveCarRental(Rental rental)
    {
        _repository[rental.BookingNumber] = rental;
    }

    public IEnumerable<Rental> GetBookingsForCarAtDate(string carRegistrationPlate, DateTime date)
    {
        // We are assuming that the checkout date is always before the return date
        return _repository.Values
            .Where(r => r.CarRegistrationPlate == carRegistrationPlate)
            .Where(r => r.CheckoutDate <= date && (r.ReturnDate == null || r.ReturnDate >= date))
            .OrderBy(r => r.CheckoutDate);
    }
}