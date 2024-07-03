using System.ComponentModel.DataAnnotations;
using CarRental.CommonTypes;

namespace CarRental.RentalApi;

public class RentalApiModels
{
    public class CheckoutRequest
    {
        [Required] public required string BookingNumber { get; set; }

        [Required] public required string CarRegistrationPlate { get; set; }

        [Required] public required string CustomerId { get; set; }

        [Required] public required CarTypeEnum CarType { get; set; }

        [Required] public required uint Odometer { get; set; }
    }

    public class ReturnRequest
    {
        [Required] public required string BookingNumber { get; set; }

        [Required] public required uint Odometer { get; set; }
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