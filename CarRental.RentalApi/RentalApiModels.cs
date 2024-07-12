using CarRental.CommonTypes;

namespace CarRental.RentalApi;

public class RentalApiModels
{
    public class CheckoutRequest
    {
        public required string BookingNumber { get; set; }

        public required string CarRegistrationPlate { get; set; }

        public required string CustomerId { get; set; }

        public required CarTypeEnum CarType { get; set; }

        public required uint Odometer { get; set; }
    }

    public class ReturnRequest
    {
        public required string BookingNumber { get; set; }

        public required uint Odometer { get; set; }
    }

    public class CheckoutResponse
    {
        public required string BookingNumber { get; set; }
        public required string CarRegistrationPlate { get; set; }
        public required string CustomerId { get; set; }
    }

    public class ReturnResponse
    {
        public required string BookingNumber { get; set; }
        public required string CarRegistrationPlate { get; set; }
        public required uint DistanceDriven { get; set; }
        public required decimal TotalCost { get; set; }
    }
}