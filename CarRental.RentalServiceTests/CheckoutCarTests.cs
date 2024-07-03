using CarRental.CommonTypes;
using CarRental.Data;
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

        var service = new Mock<CarRentalService>(repository.Object)
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
    public void Checkout_Should_ThrowException_WhenCarIsNotAvailable()
    {
        // Arrange 
        const string registrationPlate = "ABC123";

        var repository = new Mock<ICarRentalsRepository>();

        var service = new Mock<CarRentalService>(repository.Object)
        {
            CallBase = true
        };
        service.Setup(x => x.IsCarAvailable(registrationPlate)).Returns(false);

        var request = new CarCheckoutRequest(
            "123",
            registrationPlate,
            "456",
            CarTypeEnum.Compact,
            DateTime.Now,
            1000
        );

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => service.Object.CheckoutCar(request));
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