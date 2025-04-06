using BoatRental.CommonTypes;

namespace BoatRental.RentalApi;

public class RentalApiModels
{
    public class CheckoutRequest
    {
        public required string BookingNumber { get; set; }

        public required string BoatRegistrationNumber { get; set; }

        public required string CustomerId { get; set; }

        public required BoatTypeEnum BoatType { get; set; }

        public required uint EngineHours { get; set; }
    }

    public class ReturnRequest
    {
        public required string BookingNumber { get; set; }

        public required uint EngineHours { get; set; }
    }

    public class CheckoutResponse
    {
        public required string BookingNumber { get; set; }
        public required string BoatRegistrationNumber { get; set; }
        public required string CustomerId { get; set; }
    }

    public class ReturnResponse
    {
        public required string BookingNumber { get; set; }
        public required string BoatRegistrationNumber { get; set; }
        public required uint EngineHoursUsed { get; set; }
        public required decimal TotalCost { get; set; }
    }
}