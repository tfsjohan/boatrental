using BoatRental.CommonTypes;
using BoatRental.Data;
using BoatRental.PriceService;
using Moq;

namespace BoatRental.PriceServiceTests;

public class CalculatePriceTests
{
    [Theory]
    [
        InlineData(BoatTypeEnum.IseoSuper, 100, 10,
            1, 1,
            100),
        InlineData(BoatTypeEnum.IseoSuper, 100, 10,
            3, 100,
            300),
        InlineData(BoatTypeEnum.IseoSuper, 200, 0,
            2, 100,
            400),
        InlineData(BoatTypeEnum.Dolceriva, 100, 10,
            1, 1,
            140),
        InlineData(BoatTypeEnum.Dolceriva, 100, 10,
            3, 100,
            1390),
        InlineData(BoatTypeEnum.Dolceriva, 300, 10,
            3, 5,
            1220),
        InlineData(BoatTypeEnum.Diable, 300, 10,
            5, 30,
            2700),
        InlineData(BoatTypeEnum.Diable, 100, 10,
            3, 100,
            1950),
        InlineData(BoatTypeEnum.Diable, 500, 10,
            1, 100,
            2250)
    ]
    public void CalculatePrice_Should_CalculateCorrectPriceForCompactCar(
        BoatTypeEnum boatType,
        int pricePerDay,
        int pricePerKilometer,
        uint days,
        uint kilometers,
        decimal expectedPrice
    )
    {
        // Arrange
        var carTypePriceDetails = new BoatTypePriceDetails(
            boatType,
            pricePerDay,
            pricePerKilometer,
            0
        );

        var priceRepository = new Mock<IBoatTypePriceRepository>();
        priceRepository
            .Setup(x => x.GetPriceDetails(boatType))
            .Returns(carTypePriceDetails);

        var priceService = new PriceService.PriceService(priceRepository.Object);

        // Act
        var actualPrice = priceService.CalculatePrice(boatType, days, kilometers);

        // Assert
        Assert.Equal(expectedPrice, actualPrice);
    }

    [Fact]
    public void PriceCalculatorFactory_Should_ThrowOnInvalidCarType()
    {
        Assert.Throws<ArgumentException>(
            () => PriceCalculatorFactory.CreatePriceCalculator((BoatTypeEnum)100)
        );
    }

    [Fact]
    public void PriceCalculatorFactory_Should_ReturnCorrectCalculatorForCarType()
    {
        Assert.IsAssignableFrom<IseoSuperPriceCalculator>(
            PriceCalculatorFactory.CreatePriceCalculator(BoatTypeEnum.IseoSuper)
        );

        Assert.IsAssignableFrom<DolcerivaPriceCalculator>(
            PriceCalculatorFactory.CreatePriceCalculator(BoatTypeEnum.Dolceriva)
        );

        Assert.IsAssignableFrom<DiablePriceCalculator>(
            PriceCalculatorFactory.CreatePriceCalculator(BoatTypeEnum.Diable)
        );
    }
}