using BoatRental.Data;
using BoatRental.PriceService;
using BoatRental.RentalService;
using Moq;

namespace BoatRental.RentalServiceTests;

public class CarRentalServiceBuilder
{
    private IBoatRentalsRepository _boatRentalsRepository;
    private IPriceService _priceService;

    public CarRentalServiceBuilder()
    {
        _boatRentalsRepository = new Mock<IBoatRentalsRepository>().Object;
        _priceService = new Mock<IPriceService>().Object;
    }

    public CarRentalServiceBuilder WithCarRentalsRepository(IBoatRentalsRepository boatRentalsRepository)
    {
        _boatRentalsRepository = boatRentalsRepository;
        return this;
    }

    public CarRentalServiceBuilder WithPriceService(IPriceService priceService)
    {
        _priceService = priceService;
        return this;
    }

    public BoatRentalService Build()
    {
        return new BoatRentalService(_boatRentalsRepository, _priceService);
    }
}