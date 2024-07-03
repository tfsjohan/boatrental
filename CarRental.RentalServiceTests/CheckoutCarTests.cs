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

        var repository = new Mock<ICarRentalsRepository>();
        repository
            .Setup(x => x.GetBookingsForCarAtDate(registrationPlate, It.IsAny<DateTime>()))
            .Returns(new List<Rental>());

        var priceService = new Mock<IPriceService>();

        var service = new Mock<CarRentalService>(repository.Object, priceService.Object)
        {
            CallBase = true
        };

        var request = new CarCheckoutRequest(
            "123",
            registrationPlate,
            "456",
            CarTypeEnum.Compact,
            DateTime.Now,
            1000
        );

        // Act
        service.Object.CheckoutCar(request);

        // Assert
        service.Verify(x => x.IsCarAvailable(registrationPlate), Times.Once);
    }

    [Fact]
    public void Checkout_Should_ThrowException_When_CarIsNotAvailable()
    {
        // Arrange 
        const string registrationPlate = "ABC123";

        var repository = new Mock<ICarRentalsRepository>();
        repository
            .Setup(x => x.GetBookingsForCarAtDate(It.IsAny<string>(), It.IsAny<DateTime>()))
            .Returns([
                new Rental()
                {
                    BookingNumber = "123",
                    CarRegistrationPlate = registrationPlate,
                    CheckoutDate = DateTime.Now.AddHours(-1),
                    CarType = CarTypeEnum.Compact,
                    CustomerId = "456",
                    Odometer = 100
                }
            ]);

        var service = new CarRentalServiceBuilder()
            .WithCarRentalsRepository(repository.Object)
            .Build();

        var request = new CarCheckoutRequest(
            "123",
            registrationPlate,
            "456",
            CarTypeEnum.Compact,
            DateTime.Now,
            1000
        );

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => service.CheckoutCar(request));
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
        var exception = Assert.Throws<InvalidOperationException>(() => service.CheckoutCar(request));
        Assert.Contains("Booking number already used", exception.Message);
    }

    [Fact]
    public void Checkout_Should_SaveCarRental()
    {
        // Arrange 
        const string registrationPlate = "ABC123";

        var repository = new Mock<ICarRentalsRepository>();
        repository
            .Setup(x => x.GetBookingsForCarAtDate(registrationPlate, It.IsAny<DateTime>()))
            .Returns(new List<Rental>());

        var service = new CarRentalServiceBuilder()
            .WithCarRentalsRepository(repository.Object)
            .Build();

        var request = new CarCheckoutRequest(
            "123",
            registrationPlate,
            "456",
            CarTypeEnum.Compact,
            DateTime.Now,
            1000
        );

        // Act
        service.CheckoutCar(request);

        // Assert
        repository.Verify(x => x.SaveCarRental(It.IsAny<Rental>()), Times.Once);
    }
}