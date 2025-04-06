namespace BoatRental.RentalServiceTests;

public class CalculateRentalDaysTests
{
    [Fact]
    public void CalculateRentalDate_Should_ThrowIfReturnDateIsBeforeCheckoutDate()
    {
        // Arrange
        var service = new BoatRentalServiceBuilder().Build();

        // Act & Assert
        Assert.Throws<ArgumentException>(() => service.CalculateRentalDays(
            DateTime.UtcNow.AddDays(1),
            DateTime.UtcNow
        ));
    }

    [Theory]
    [
        InlineData("2024-06-20T08:00:00", "2024-06-20T12:00:00", (uint)1),
        InlineData("2024-06-20T08:00:00", "2024-06-21T08:00:00", (uint)1),
        InlineData("2024-06-20T08:00:00", "2024-06-22T08:00:00", (uint)2),
        InlineData("2024-06-29T23:00:00", "2024-07-01T08:00:00", (uint)2),
    ]
    public void CalculateRentalDate_Should_ReturnCorrectRentalDays(string checkoutDate, string returnDate,
        uint expectedDays)
    {
        // Arrange
        var service = new BoatRentalServiceBuilder().Build();

        // Act
        var days = service.CalculateRentalDays(
            DateTime.Parse(checkoutDate),
            DateTime.Parse(returnDate)
        );

        // Assert
        Assert.Equal(expectedDays, days);
    }
}