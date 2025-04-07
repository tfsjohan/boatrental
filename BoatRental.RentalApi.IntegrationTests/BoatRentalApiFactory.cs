using BoatRental.RentalApi;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BoatRental.RentalApi.IntegrationTests;

public class BoatRentalApiFactory : WebApplicationFactory<Program>
{
    protected override IHost CreateHost(IHostBuilder builder)
    {
        // Configure the services for testing if needed
        builder.ConfigureServices(services =>
        {
            // You can replace services with test doubles here if needed
            // Example: services.AddScoped<IBoatRentalsRepository, TestBoatRentalsRepository>();
        });

        return base.CreateHost(builder);
    }
}