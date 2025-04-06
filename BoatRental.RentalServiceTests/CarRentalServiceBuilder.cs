using BoatRental.Data;
using BoatRental.PriceService;
using BoatRental.RentalService;
using Moq;

namespace BoatRental.RentalServiceTests;

public class BoatRentalServiceBuilder
{
    private IBoatRentalsRepository _boatRentalsRepository;
    private IPriceService _priceService;

    public BoatRentalServiceBuilder()
    {
        _boatRentalsRepository = new Mock<IBoatRentalsRepository>().Object;
        _priceService = new Mock<IPriceService>().Object;
    }

    public BoatRentalServiceBuilder WithBoatRentalsRepository(IBoatRentalsRepository boatRentalsRepository)
    {
        _boatRentalsRepository = boatRentalsRepository;
        return this;
    }

    public BoatRentalServiceBuilder WithPriceService(IPriceService priceService)
    {
        _priceService = priceService;
        return this;
    }

    public BoatRentalService Build()
    {
        return new BoatRentalService(_boatRentalsRepository, _priceService);
    }
}