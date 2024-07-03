using CarRental.Data;
using CarRental.PriceService;
using CarRental.RentalApi;
using CarRental.RentalService;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<ICarRentalsRepository, CarRentalRepository>();
builder.Services.AddSingleton<ICarTypePriceRepository, CarTypePriceRepository>();
builder.Services.AddTransient<IPriceService, PriceService>();
builder.Services.AddTransient<ICarRentalService, CarRentalService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/checkoutcar", ([FromBody] RentalApiModels.CheckoutRequest request, ICarRentalService rentalService) =>
    {
        try
        {
            rentalService.CheckoutCar(new CarCheckoutRequest(
                BookingNumber: request.BookingNumber,
                CarRegistrationPlate: request.CarRegistrationPlate,
                CustomerId: request.CustomerId,
                CarType: request.CarType,
                CheckoutDate: DateTime.UtcNow,
                Odometer: request.Odometer
            ));

            return Results.Ok(new RentalApiModels.CheckoutResponse()
            {
                BookingNumber = request.BookingNumber,
                CarRegistrationPlate = request.CarRegistrationPlate,
                CustomerId = request.CustomerId
            });
        }
        catch (InvalidOperationException ex)
        {
            return Results.BadRequest(ex.Message);
        }
    })
    .WithName("CheckoutCar")
    .WithOpenApi();

app.MapGet("/returncar", ([FromBody] RentalApiModels.ReturnRequest request, ICarRentalService rentalService) =>
    {
        try
        {
            var response = rentalService.ReturnCar(new CarReturnRequest(
                BookingNumber: request.BookingNumber,
                ReturnDate: DateTime.UtcNow,
                Odometer: request.Odometer
            ));

            return Results.Ok(
                new RentalApiModels.ReturnResponse()
                {
                    BookingNumber = response.BookingNumber,
                    CarRegistrationPlate = response.CarRegistrationPlate,
                    DistanceDriven = response.DistanceDriven,
                    TotalCost = response.TotalCost
                }
            );
        }
        catch (InvalidOperationException ex)
        {
            return Results.BadRequest(ex.Message);
        }
    })
    .WithName("ReturnCar")
    .WithOpenApi();

app.Run();