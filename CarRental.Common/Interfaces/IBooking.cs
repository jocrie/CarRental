using CarRental.Common.Classes;

namespace CarRental.Common.Interfaces;

public interface IBooking
{
    public int Id { get; init; }
    //public int VehicleId { get; init; }
    //public int PersonId { get; init; }
    //public string RegNo { get; }
    //public string Customer { get; }
    public IVehicle Vehicle { get; init; }
    public Customer Customer { get; init; }
    public int? OdometerRented { get; }
    public int? OdometerReturned { get; }
    public DateOnly DateRented { get; init; }
    public DateOnly? DateReturned { get; }
    public int? DrivenKm { get; init; }
    public double Cost { get; }
    public bool BookingClosed { get; }
    public bool BookingValid { get; }
    public void ValidateBooking();
    public void ProcessRentingRequest();
    public void ProcessReturningRequest();
}
