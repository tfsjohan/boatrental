using CarRental.Data;
using CarRental.PriceService;
using CarRental.RentalService;
using Moq;

namespace CarRental.RentalServiceTests;

public class CarRentalServiceBuilder
{
    private ICarRentalsRepository _carRentalsRepository;
    private IPriceService _priceService;

    public CarRentalServiceBuilder()
    {
        _carRentalsRepository = new Mock<ICarRentalsRepository>().Object;
        _priceService = new Mock<IPriceService>().Object;
    }

    public CarRentalServiceBuilder WithCarRentalsRepository(ICarRentalsRepository carRentalsRepository)
    {
        _carRentalsRepository = carRentalsRepository;
        return this;
    }

    public CarRentalServiceBuilder WithPriceService(IPriceService priceService)
    {
        _priceService = priceService;
        return this;
    }

    public CarRentalService Build()
    {
        return new CarRentalService(_carRentalsRepository, _priceService);
    }
}