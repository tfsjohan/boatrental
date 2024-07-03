using CarRental.Data;
using CarRental.PriceService;
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

app.MapGet("/checkoutcar", ([FromBody] CarCheckoutRequest request, ICarRentalService rentalService) =>
    {
        try
        {
            rentalService.CheckoutCar(request);

            return Results.Ok($"Car {request.CarRegistrationPlate} checked out for customer {request.CustomerId}");
        }
        catch (InvalidOperationException ex)
        {
            return Results.BadRequest(ex.Message);
        }
    })
    .WithName("CheckoutCar")
    .WithOpenApi();

app.MapGet("/returncar", ([FromBody] CarReturnRequest request, ICarRentalService rentalService) =>
    {
        try
        {
            var response = rentalService.ReturnCar(request);

            return Results.Ok(
                $"Car {response.CarRegistrationPlate} returned. Total price for rental is {response.TotalCost:C}");
        }
        catch (InvalidOperationException ex)
        {
            return Results.BadRequest(ex.Message);
        }
    })
    .WithName("ReturnCar")
    .WithOpenApi();

app.Run();