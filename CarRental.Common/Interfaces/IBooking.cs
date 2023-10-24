using CarRental.Common.Classes;

namespace CarRental.Common.Interfaces;

public interface IBooking
{
    public int Id { get; init; }
    public IVehicle Vehicle { get; init; }
    public Customer Customer { get; init; }
    public int OdometerRented { get; init; }
    public int? OdometerReturned { get; }
    public DateOnly DateRented { get; init; }
    public DateOnly? DateReturned { get; }
    public int? DrivenKm { get; }
    public double Cost { get; }
    public bool BookingClosed { get; }
    public bool BookingValid { get; }
    public void ProcessReturn(int drivenKm, DateOnly returnDate);
}
