using BoatRental.CommonTypes;
using BoatRental.Data;
using BoatRental.PriceService;
using BoatRental.RentalService;
using Moq;

namespace BoatRental.RentalServiceTests;

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
        uint expectedDistanceDriven
    )
    {
        // Arrange
        const string bookingNumber = "123";
        var rental = new Rental
        {
            BookingNumber = bookingNumber,
            BoatRegistrationNumber = "ABC123",
            CustomerId = "456",
            BoatType = BoatTypeEnum.IseoSuper,
            CheckoutDate = DateTime.UtcNow.AddDays(-1),
            EngineHours = initialOdometer
        };

        var repository = new Mock<IBoatRentalsRepository>();
        repository
            .Setup(x => x.GetBoatRental(rental.BookingNumber))
            .Returns(rental);

        var service = new BoatRentalServiceBuilder()
            .WithBoatRentalsRepository(repository.Object)
            .Build();

        var request = new BoatReturnRequest(
            "123",
            DateTime.UtcNow,
            returnOdometer
        );

        // Act
        var response = service.ReturnBoat(request);

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
            BoatRegistrationNumber = "ABC123",
            CustomerId = "456",
            BoatType = BoatTypeEnum.IseoSuper,
            CheckoutDate = checkoutDate,
            EngineHours = 0
        };

        var repository = new Mock<IBoatRentalsRepository>();
        repository
            .Setup(x => x.GetBoatRental(rental.BookingNumber))
            .Returns(rental);

        var service = new BoatRentalServiceBuilder()
            .WithBoatRentalsRepository(repository.Object)
            .Build();

        var request = new BoatReturnRequest(
            bookingNumber,
            returnDate,
            0
        );

        // Act
        var response = service.ReturnBoat(request);

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
            BoatRegistrationNumber = "ABC123",
            CustomerId = "456",
            BoatType = BoatTypeEnum.IseoSuper,
            CheckoutDate = checkoutDate,
            EngineHours = 0
        };

        var repository = new Mock<IBoatRentalsRepository>();
        repository
            .Setup(x => x.GetBoatRental(rental.BookingNumber))
            .Returns(rental);

        var service = new BoatRentalServiceBuilder()
            .WithBoatRentalsRepository(repository.Object)
            .Build();

        var request = new BoatReturnRequest(
            bookingNumber,
            returnDate,
            0
        );

        // Act
        var response = service.ReturnBoat(request);

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
            BoatRegistrationNumber = "ABC123",
            CustomerId = "456",
            BoatType = BoatTypeEnum.IseoSuper,
            CheckoutDate = checkoutDate,
            EngineHours = 0
        };

        var repository = new Mock<IBoatRentalsRepository>();
        repository
            .Setup(x => x.GetBoatRental(rental.BookingNumber))
            .Returns(rental);

        var service = new BoatRentalServiceBuilder()
            .WithBoatRentalsRepository(repository.Object)
            .Build();

        var request = new BoatReturnRequest(
            bookingNumber,
            returnDate,
            0
        );

        // Act & Assert
        Assert.Throws<InvalidReturnDateException>(() => service.ReturnBoat(request));
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
            BoatRegistrationNumber = "ABC123",
            CustomerId = "456",
            BoatType = BoatTypeEnum.IseoSuper,
            CheckoutDate = DateTime.UtcNow.AddDays(-2),
            EngineHours = checkoutOdometer
        };

        var repository = new Mock<IBoatRentalsRepository>();
        repository
            .Setup(x => x.GetBoatRental(rental.BookingNumber))
            .Returns(rental);

        var service = new BoatRentalServiceBuilder()
            .WithBoatRentalsRepository(repository.Object)
            .Build();

        var request = new BoatReturnRequest(
            bookingNumber,
            DateTime.UtcNow,
            returnOdometer
        );

        // Act & Assert
        Assert.Throws<InvalidOdometerException>(() => service.ReturnBoat(request));
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
            BoatRegistrationNumber = "ABC123",
            CustomerId = "456",
            BoatType = BoatTypeEnum.IseoSuper,
            CheckoutDate = DateTime.Parse("2024-06-20"),
            EngineHours = initialOdometer
        };

        var repository = new Mock<IBoatRentalsRepository>();
        repository
            .Setup(x => x.GetBoatRental(rental.BookingNumber))
            .Returns(rental);

        var priceService = new Mock<IPriceService>();
        priceService.Setup(x => x.CalculatePrice(
                rental.BoatType,
                3,
                distanceDriven
            ))
            .Returns(100);

        var service = new BoatRentalServiceBuilder()
            .WithBoatRentalsRepository(repository.Object)
            .WithPriceService(priceService.Object)
            .Build();

        var request = new BoatReturnRequest(
            bookingNumber,
            DateTime.Parse("2024-06-23"),
            initialOdometer + distanceDriven
        );

        // Act
        var response = service.ReturnBoat(request);

        // Assert
        Assert.NotEqual(0, response.TotalCost);
    }

    [Fact]
    public void ReturnCar_Should_UpdateBoatRentalData()
    {
        // Arrange
        const string bookingNumber = "123";

        var rental = new Rental
        {
            BookingNumber = bookingNumber,
            BoatRegistrationNumber = "ABC123",
            CustomerId = "456",
            BoatType = BoatTypeEnum.IseoSuper,
            CheckoutDate = DateTime.UtcNow.AddDays(-1),
            EngineHours = 0
        };

        var repository = new Mock<IBoatRentalsRepository>();
        repository
            .Setup(x => x.GetBoatRental(rental.BookingNumber))
            .Returns(rental);

        var service = new BoatRentalServiceBuilder()
            .WithBoatRentalsRepository(repository.Object)
            .Build();

        var request = new BoatReturnRequest(
            bookingNumber,
            DateTime.UtcNow,
            10
        );

        // Act
        service.ReturnBoat(request);

        // Assert
        Assert.Equal(request.Odometer, rental.ReturnOdometer);
        Assert.Equal(request.ReturnDate, rental.ReturnDate);
    }

    [Fact]
    public void ReturnCar_Should_ThrowException_When_CarIsAlreadyReturned()
    {
        // Arrange
        const string bookingNumber = "123";

        var rental = new Rental
        {
            BookingNumber = bookingNumber,
            BoatRegistrationNumber = "ABC123",
            CustomerId = "456",
            BoatType = BoatTypeEnum.IseoSuper,
            CheckoutDate = DateTime.UtcNow.AddDays(-1),
            EngineHours = 0,
            ReturnDate = DateTime.UtcNow,
            ReturnOdometer = 10
        };

        var repository = new Mock<IBoatRentalsRepository>();
        repository
            .Setup(x => x.GetBoatRental(rental.BookingNumber))
            .Returns(rental);

        var service = new BoatRentalServiceBuilder()
            .WithBoatRentalsRepository(repository.Object)
            .Build();

        var request = new BoatReturnRequest(
            bookingNumber,
            DateTime.UtcNow,
            10
        );

        // Act & Assert

        Assert.Throws<CarAlreadyReturnedException>(() => service.ReturnBoat(request));
    }
}