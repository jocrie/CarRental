using CarRental.Common.Classes;

namespace CarRental.Common.Interfaces;

public interface IBooking
{
    public int VehicleId { get; init; }
    public int PersonId { get; init; }
    public string RegNo { get; }
    public string Customer { get; }
    public int? OdometerRented { get; }
    public int? OdometerReturned { get; }
    public DateOnly DateRented { get; init; }
    public DateOnly? DateReturned { get; }
    public int? DrivenKm { get; init; }
    public double Cost { get; }
    public bool BookingClosed { get; }
    public bool BookingValid { get; }
    public void InvalidateBooking();
    public void RentVehicle(IVehicle vehicle, Customer customer);
    public void ReturnVehicle(IVehicle vehicle);
}
