using CarRental.CommonTypes;
using CarRental.Data;
using Moq;

namespace CarRental.PriceServiceTests;

public class CalculatePriceTests
{
    [Theory]
    [
        InlineData(CarTypeEnum.Compact, 100, 10,
            1, 1,
            100),
        InlineData(CarTypeEnum.Compact, 100, 10,
            3, 100,
            300),
        InlineData(CarTypeEnum.Compact, 200, 0,
            2, 100,
            400),
        InlineData(CarTypeEnum.StationWagon, 100, 10,
            1, 1,
            140),
        InlineData(CarTypeEnum.StationWagon, 100, 10,
            3, 100,
            1390),
        InlineData(CarTypeEnum.StationWagon, 300, 10,
            3, 5,
            1220),
        InlineData(CarTypeEnum.Truck, 300, 10,
            5, 30,
            2700),
        InlineData(CarTypeEnum.Truck, 100, 10,
            3, 100,
            1950),
        InlineData(CarTypeEnum.Truck, 500, 10,
            1, 100,
            2250)
    ]
    public void CalculatePrice_Should_CalculateCorrectPriceForCompactCar(
        CarTypeEnum carType,
        int pricePerDay,
        int pricePerKilometer,
        int days,
        int kilometers,
        decimal expectedPrice
    )
    {
        // Arrange
        var carTypePriceDetails = new CarTypePriceDetails(
            carType,
            pricePerDay,
            pricePerKilometer
        );

        var priceRepository = new Mock<ICarTypePriceRepository>();
        priceRepository.Setup(x => x.GetPriceDetails(carType)).Returns(carTypePriceDetails);

        var priceService = new PriceService.PriceService(priceRepository.Object);

        // Act
        var actualPrice = priceService.CalculatePrice(carType, days, kilometers);

        // Assert
        Assert.Equal(expectedPrice, actualPrice);
    }

    [Fact]
    public void CalculatePrice_Should_ThrowError_When_NumberOfDaysIsNegative()
    {
        // Arrange
        var carTypePriceDetails = new CarTypePriceDetails(
            CarTypeEnum.Compact,
            1,
            1
        );

        var priceRepository = new Mock<ICarTypePriceRepository>();
        priceRepository
            .Setup(x => x.GetPriceDetails(carTypePriceDetails.CarType))
            .Returns(carTypePriceDetails);

        var priceService = new PriceService.PriceService(priceRepository.Object);
        
        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => 
            priceService.CalculatePrice(carTypePriceDetails.CarType, -1, 1));
    }
    
    [Fact]
    public void CalculatePrice_Should_ThrowError_When_NumberOfKilometersIsNegative()
    {
        // Arrange
        var carTypePriceDetails = new CarTypePriceDetails(
            CarTypeEnum.Compact,
            1,
            1
        );

        var priceRepository = new Mock<ICarTypePriceRepository>();
        priceRepository
            .Setup(x => x.GetPriceDetails(carTypePriceDetails.CarType))
            .Returns(carTypePriceDetails);

        var priceService = new PriceService.PriceService(priceRepository.Object);
        
        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => 
            priceService.CalculatePrice(carTypePriceDetails.CarType, 10, -1));
    }
}