namespace CarRental.Data;

public interface ICarRentalsRepository
{
    Rental GetCarRental(string bookingNumber);
    
    void SaveCarRental(Rental rental);
    
    IEnumerable<Rental> GetBookingsForCarAtDate(string carRegistrationPlate, DateTime date);
}