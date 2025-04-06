using BoatRental.CommonTypes;
using BoatRental.Data;
using BoatRental.PriceService;
using BoatRental.RentalService;
using Moq;

namespace BoatRental.RentalServiceTests;

public class CheckoutBoatTests
{
    [Fact]
    public void Checkout_Should_MakeSureCarIsAvailable()
    {
        // Arrange 
        const string registrationPlate = "ABC123";

        var repository = new BoatRentalsRepository();

        var priceService = new Mock<IPriceService>();

        var service = new BoatRentalService(repository, priceService.Object);

        var existingRequest = new BoatCheckoutRequest(
            "123",
            registrationPlate,
            "456",
            BoatTypeEnum.IseoSuper,
            DateTime.UtcNow.AddHours(-1),
            1000
        );
        service.CheckoutBoat(existingRequest);

        // Act
        var nextRequest = new BoatCheckoutRequest(
            "321",
            registrationPlate,
            "654",
            BoatTypeEnum.IseoSuper,
            DateTime.UtcNow,
            1000
        );

        // Assert
        Assert.Throws<CarUnavailableException>(() => service.CheckoutBoat(nextRequest));
    }

    [Fact]
    public void Checkout_Should_ThrowException_When_BookingNumberUsed()
    {
        // Arrange 
        const string bookingNumber = "123";
        var rental = new Rental()
        {
            BookingNumber = bookingNumber,
            BoatRegistrationNumber = "ABC123",
            CheckoutDate = DateTime.Now.AddHours(-1),
            BoatType = BoatTypeEnum.IseoSuper,
            CustomerId = "456",
            EngineHours = 100
        };

        var repository = new Mock<IBoatRentalsRepository>();
        repository
            .Setup(x => x.GetBoatRental(bookingNumber))
            .Returns(rental);

        var service = new BoatRentalServiceBuilder()
            .WithBoatRentalsRepository(repository.Object)
            .Build();

        var request = new BoatCheckoutRequest(
            bookingNumber,
            "CBA321",
            "456",
            BoatTypeEnum.Dolceriva,
            DateTime.Now,
            1000
        );

        // Act & Assert
        Assert.Throws<BookingNumberAlreadyUsedException>(() => service.CheckoutBoat(request));
    }

    [Fact]
    public void Checkout_Should_SaveBoatRental()
    {
        // Arrange 
        const string registrationPlate = "ABC123";
        const string bookingNumber = "123";

        var repository = new BoatRentalsRepository();

        var service = new BoatRentalServiceBuilder()
            .WithBoatRentalsRepository(repository)
            .Build();

        var request = new BoatCheckoutRequest(
            bookingNumber,
            registrationPlate,
            "456",
            BoatTypeEnum.IseoSuper,
            DateTime.Now,
            1000
        );

        // Act
        service.CheckoutBoat(request);

        // Assert
        var rental = repository.GetBoatRental(bookingNumber);
        Assert.NotNull(rental);
    }
}