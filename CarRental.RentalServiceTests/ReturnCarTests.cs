using CarRental.CommonTypes;
using CarRental.Data;
using CarRental.PriceService;
using CarRental.RentalService;
using Moq;

namespace CarRental.RentalServiceTests;

public class ReturnCarTests
{
    [Theory]
    [
        InlineData(1000, 2000, 1000),
        InlineData(1000, 3000, 2000),
        InlineData(1000, 1000, 0)
    ]
    public void ReturnCar_Should_CalculateDistanceDriven(
        uint initialOdometer,
        uint returnOdometer,
        uint expectedDistanceDriven)
    {
        // Arrange
        const string bookingNumber = "123";
        var rental = new Rental
        {
            BookingNumber = bookingNumber,
            CarRegistrationPlate = "ABC123",
            CustomerId = "456",
            CarType = CarTypeEnum.Compact,
            CheckoutDate = DateTime.UtcNow.AddDays(-1),
            Odometer = initialOdometer
        };

        var repository = new Mock<ICarRentalsRepository>();
        repository
            .Setup(x => x.GetCarRental(rental.BookingNumber))
            .Returns(rental);
        repository
            .Setup(x => x.GetBookingsForCarAtDate(It.IsAny<string>(), It.IsAny<DateTime>()))
            .Returns([]);

        var service = new CarRentalServiceBuilder()
            .WithCarRentalsRepository(repository.Object)
            .Build();

        var request = new CarReturnRequest(
            "123",
            DateTime.UtcNow,
            returnOdometer
        );

        // Act
        var response = service.ReturnCar(request);

        // Assert
        Assert.Equal(expectedDistanceDriven, response.DistanceDriven);
    }

    [Fact]
    public void ReturnCar_Should_CalculateNumberOfFullRentalDays()
    {
        // Arrange
        const string bookingNumber = "123";
        const uint rentalDays = 3;
        var checkoutDate = DateTime.Parse("2024-06-20");
        var returnDate = checkoutDate.AddDays(rentalDays);

        var rental = new Rental
        {
            BookingNumber = bookingNumber,
            CarRegistrationPlate = "ABC123",
            CustomerId = "456",
            CarType = CarTypeEnum.Compact,
            CheckoutDate = checkoutDate,
            Odometer = 0
        };

        var repository = new Mock<ICarRentalsRepository>();
        repository
            .Setup(x => x.GetCarRental(rental.BookingNumber))
            .Returns(rental);
        repository
            .Setup(x => x.GetBookingsForCarAtDate(It.IsAny<string>(), It.IsAny<DateTime>()))
            .Returns([]);

        var service = new CarRentalServiceBuilder()
            .WithCarRentalsRepository(repository.Object)
            .Build();

        var request = new CarReturnRequest(
            "123",
            returnDate,
            0
        );

        // Act
        var response = service.ReturnCar(request);

        // Assert
        Assert.Equal(rentalDays, response.FullDaysRented);
    }

    [Fact]
    public void ReturnCar_Should_CheckoutAndReturnSameDayShouldCountAsOneFullDay()
    {
        // Arrange
        const string bookingNumber = "123";
        var checkoutDate = DateTime.Parse("2024-06-20T12:00:00");
        var returnDate = DateTime.Parse("2024-06-20T14:00:00");

        var rental = new Rental
        {
            BookingNumber = bookingNumber,
            CarRegistrationPlate = "ABC123",
            CustomerId = "456",
            CarType = CarTypeEnum.Compact,
            CheckoutDate = checkoutDate,
            Odometer = 0
        };

        var repository = new Mock<ICarRentalsRepository>();
        repository
            .Setup(x => x.GetCarRental(rental.BookingNumber))
            .Returns(rental);
        repository
            .Setup(x => x.GetBookingsForCarAtDate(It.IsAny<string>(), It.IsAny<DateTime>()))
            .Returns([]);

        var service = new CarRentalServiceBuilder()
            .WithCarRentalsRepository(repository.Object)
            .Build();

        var request = new CarReturnRequest(
            "123",
            returnDate,
            0
        );

        // Act
        var response = service.ReturnCar(request);

        // Assert
        Assert.Equal((uint)1, response.FullDaysRented);
    }

    [Fact]
    public void ReturnCar_Should_ValidateThatReturnDateIsAfterCheckoutDate()
    {
        // Arrange
        const string bookingNumber = "123";
        var checkoutDate = DateTime.Parse("2024-06-20");
        var returnDate = DateTime.Parse("2024-06-01");

        var rental = new Rental
        {
            BookingNumber = bookingNumber,
            CarRegistrationPlate = "ABC123",
            CustomerId = "456",
            CarType = CarTypeEnum.Compact,
            CheckoutDate = checkoutDate,
            Odometer = 0
        };

        var repository = new Mock<ICarRentalsRepository>();
        repository
            .Setup(x => x.GetCarRental(rental.BookingNumber))
            .Returns(rental);
        repository
            .Setup(x => x.GetBookingsForCarAtDate(It.IsAny<string>(), It.IsAny<DateTime>()))
            .Returns([]);

        var service = new CarRentalServiceBuilder()
            .WithCarRentalsRepository(repository.Object)
            .Build();

        var request = new CarReturnRequest(
            "123",
            returnDate,
            0
        );

        // Act & Assert
        Assert.Throws<ArgumentException>(() => service.ReturnCar(request));
    }

    [Fact]
    public void ReturnCar_Should_ValidateThatReturnDateOdometerIsMoreThanCheckoutOdometer()
    {
        // Arrange
        const string bookingNumber = "123";
        uint checkoutOdometer = 1000;
        uint returnOdometer = 500;

        var rental = new Rental
        {
            BookingNumber = bookingNumber,
            CarRegistrationPlate = "ABC123",
            CustomerId = "456",
            CarType = CarTypeEnum.Compact,
            CheckoutDate = DateTime.UtcNow.AddDays(-2),
            Odometer = checkoutOdometer
        };

        var repository = new Mock<ICarRentalsRepository>();
        repository
            .Setup(x => x.GetCarRental(rental.BookingNumber))
            .Returns(rental);
        repository
            .Setup(x => x.GetBookingsForCarAtDate(It.IsAny<string>(), It.IsAny<DateTime>()))
            .Returns([]);

        var service = new CarRentalServiceBuilder()
            .WithCarRentalsRepository(repository.Object)
            .Build();

        var request = new CarReturnRequest(
            "123",
            DateTime.UtcNow,
            returnOdometer
        );

        // Act & Assert
        Assert.Throws<ArgumentException>(() => service.ReturnCar(request));
    }

    [Fact]
    public void ReturnCar_Should_CalculateTotalCost()
    {
        // Arrange
        const string bookingNumber = "123";
        const uint initialOdometer = 1000;
        const uint distanceDriven = 100;

        var rental = new Rental
        {
            BookingNumber = bookingNumber,
            CarRegistrationPlate = "ABC123",
            CustomerId = "456",
            CarType = CarTypeEnum.Compact,
            CheckoutDate = DateTime.Parse("2024-06-20"),
            Odometer = initialOdometer
        };

        var repository = new Mock<ICarRentalsRepository>();
        repository
            .Setup(x => x.GetCarRental(rental.BookingNumber))
            .Returns(rental);
        repository
            .Setup(x => x.GetBookingsForCarAtDate(It.IsAny<string>(), It.IsAny<DateTime>()))
            .Returns([]);

        var priceService = new Mock<IPriceService>();
        priceService.Setup(x => x.CalculatePrice(
                rental.CarType,
                3,
                distanceDriven
            ))
            .Returns(100)
            .Verifiable();

        var service = new CarRentalServiceBuilder()
            .WithCarRentalsRepository(repository.Object)
            .WithPriceService(priceService.Object)
            .Build();

        var request = new CarReturnRequest(
            "123",
            DateTime.Parse("2024-06-23"),
            initialOdometer + distanceDriven
        );

        // Act
        var response = service.ReturnCar(request);

        // Assert
        priceService.Verify(x => x.CalculatePrice(
            rental.CarType,
            3,
            distanceDriven
        ), Times.Once);

        Assert.NotEqual(0, response.TotalCost);
    }

    [Fact]
    public void ReturnCar_Should_UpdateCarRentalData()
    {
        // Arrange
        const string bookingNumber = "123";

        var rental = new Rental
        {
            BookingNumber = bookingNumber,
            CarRegistrationPlate = "ABC123",
            CustomerId = "456",
            CarType = CarTypeEnum.Compact,
            CheckoutDate = DateTime.UtcNow.AddDays(-1),
            Odometer = 0
        };

        var repository = new Mock<ICarRentalsRepository>();
        repository
            .Setup(x => x.GetCarRental(rental.BookingNumber))
            .Returns(rental);
        repository
            .Setup(x => x.SaveCarRental(rental))
            .Verifiable();

        var service = new CarRentalServiceBuilder()
            .WithCarRentalsRepository(repository.Object)
            .Build();

        var request = new CarReturnRequest(
            bookingNumber,
            DateTime.UtcNow,
            10
        );

        // Act
        service.ReturnCar(request);

        // Assert
        Assert.Equal(request.Odometer, rental.ReturnOdometer);
        Assert.Equal(request.ReturnDate, rental.ReturnDate);
        repository.Verify(x => x.SaveCarRental(rental), Times.Once);
    }

    [Fact]
    public void ReturnCar_Should_ThrowException_When_CarIsAlreadyReturned()
    {
        // Arrange
        const string bookingNumber = "123";

        var rental = new Rental
        {
            BookingNumber = bookingNumber,
            CarRegistrationPlate = "ABC123",
            CustomerId = "456",
            CarType = CarTypeEnum.Compact,
            CheckoutDate = DateTime.UtcNow.AddDays(-1),
            Odometer = 0,
            ReturnDate = DateTime.UtcNow,
            ReturnOdometer = 10
        };

        var repository = new Mock<ICarRentalsRepository>();
        repository
            .Setup(x => x.GetCarRental(rental.BookingNumber))
            .Returns(rental);

        var service = new CarRentalServiceBuilder()
            .WithCarRentalsRepository(repository.Object)
            .Build();

        var request = new CarReturnRequest(
            bookingNumber,
            DateTime.UtcNow,
            10
        );

        // Act & Assert

        Assert.Throws<CarAlreadyReturnedException>(() => service.ReturnCar(request));
    }
}