using CarRental.CommonTypes;
using CarRental.Data;
using CarRental.PriceService;
using CarRental.RentalService;
using Moq;

namespace CarRental.RentalServiceTests;

public class CheckoutCarTests
{
    [Fact]
    public void Checkout_Should_MakeSureCarIsAvailable()
    {
        // Arrange 
        const string registrationPlate = "ABC123";

        var repository = new CarRentalsRepository();

        var priceService = new Mock<IPriceService>();

        var service = new CarRentalService(repository, priceService.Object);

        var existingRequest = new CarCheckoutRequest(
            "123",
            registrationPlate,
            "456",
            CarTypeEnum.Compact,
            DateTime.UtcNow.AddHours(-1),
            1000
        );
        service.CheckoutCar(existingRequest);

        // Act
        var nextRequest = new CarCheckoutRequest(
            "321",
            registrationPlate,
            "654",
            CarTypeEnum.Compact,
            DateTime.UtcNow,
            1000
        );

        // Assert
        Assert.Throws<CarUnavailableException>(() => service.CheckoutCar(nextRequest));
    }

    [Fact]
    public void Checkout_Should_ThrowException_When_BookingNumberUsed()
    {
        // Arrange 
        const string bookingNumber = "123";
        var rental = new Rental()
        {
            BookingNumber = bookingNumber,
            CarRegistrationPlate = "ABC123",
            CheckoutDate = DateTime.Now.AddHours(-1),
            CarType = CarTypeEnum.Compact,
            CustomerId = "456",
            Odometer = 100
        };

        var repository = new Mock<ICarRentalsRepository>();
        repository
            .Setup(x => x.GetCarRental(bookingNumber))
            .Returns(rental);

        var service = new CarRentalServiceBuilder()
            .WithCarRentalsRepository(repository.Object)
            .Build();

        var request = new CarCheckoutRequest(
            bookingNumber,
            "CBA321",
            "456",
            CarTypeEnum.StationWagon,
            DateTime.Now,
            1000
        );

        // Act & Assert
        Assert.Throws<BookingNumberAlreadyUsedException>(() => service.CheckoutCar(request));
    }

    [Fact]
    public void Checkout_Should_SaveCarRental()
    {
        // Arrange 
        const string registrationPlate = "ABC123";
        const string bookingNumber = "123";

        var repository = new CarRentalsRepository();

        var service = new CarRentalServiceBuilder()
            .WithCarRentalsRepository(repository)
            .Build();

        var request = new CarCheckoutRequest(
            bookingNumber,
            registrationPlate,
            "456",
            CarTypeEnum.Compact,
            DateTime.Now,
            1000
        );

        // Act
        service.CheckoutCar(request);

        // Assert
        var rental = repository.GetCarRental(bookingNumber);
        Assert.NotNull(rental);
    }
}