using BoatRental.Data;
using BoatRental.PriceService;
using BoatRental.RentalApi;
using BoatRental.RentalService;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IBoatRentalsRepository, BoatRentalsRepository>();
builder.Services.AddSingleton<IBoatTypePriceRepository, BoatTypePriceRepository>();
builder.Services.AddTransient<IPriceService, PriceService>();
builder.Services.AddTransient<IBoatRentalService, BoatRentalService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/checkoutboat", (
        [FromBody] RentalApiModels.CheckoutRequest request,
        IBoatRentalService rentalService
    ) =>
    {
        try
        {
            rentalService.CheckoutBoat(new BoatCheckoutRequest(
                BookingNumber: request.BookingNumber,
                CarRegistrationPlate: request.BoatRegistrationNumber,
                CustomerId: request.CustomerId,
                BoatType: request.BoatType,
                CheckoutDate: DateTime.UtcNow,
                Odometer: request.EngineHours
            ));

            return Results.Ok(new RentalApiModels.CheckoutResponse()
            {
                BookingNumber = request.BookingNumber,
                BoatRegistrationNumber = request.BoatRegistrationNumber,
                CustomerId = request.CustomerId
            });
        }
        catch (Exception ex)
        {
            return Results.BadRequest(ex.Message);
        }
    })
    .WithName("CheckoutBoat")
    .WithOpenApi();

app.MapPost("/returnboat", (
        [FromBody] RentalApiModels.ReturnRequest request,
        IBoatRentalService rentalService
    ) =>
    {
        try
        {
            var response = rentalService.ReturnBoat(new BoatReturnRequest(
                BookingNumber: request.BookingNumber,
                ReturnDate: DateTime.UtcNow,
                Odometer: request.EngineHours
            ));

            return Results.Ok(
                new RentalApiModels.ReturnResponse()
                {
                    BookingNumber = response.BookingNumber,
                    BoatRegistrationNumber = response.CarRegistrationPlate,
                    EngineHoursUsed = response.DistanceDriven,
                    TotalCost = response.TotalCost
                }
            );
        }
        catch (Exception ex)
        {
            return Results.BadRequest(ex.Message);
        }
    })
    .WithName("ReturnBoat")
    .WithOpenApi();

app.Run();