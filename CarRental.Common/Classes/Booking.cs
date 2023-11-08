using CarRental.Common.Interfaces;
using CarRental.Common.Enums;
using CarRental.Common.Extensions;

namespace CarRental.Common.Classes;

public class Booking : IBooking
{
    public int Id { get; init; }
    public IVehicle Vehicle { get; init; }
    public Customer Customer { get; init; }
    public int OdometerRented { get; init; }
    public int? OdometerReturned { get; private set; } = null;
    public DateOnly DateRented { get; init; }
    public DateOnly? DateReturned { get; private set; }
    public int? DrivenKm { get; private set; } = null;
    public double Cost { get; private set; }
    public bool BookingClosed { get; private set; } = false;
    public bool BookingValid => !(Vehicle is null || Customer is null);

    public Booking(int id, IVehicle vehicle, Customer customer, DateOnly dateRented, int odometerRented)
    {
        Id = id;
        Vehicle = vehicle;
        Customer = customer;
        DateRented = dateRented;
        OdometerRented = odometerRented;
    }

    public void ProcessReturn(int drivenKm, DateOnly returnDate)
    {
        DrivenKm = drivenKm;
        int daysDifference = DateRented.Duration(returnDate);
        OdometerReturned = OdometerRented + DrivenKm;
        if (Vehicle.CostKm is null || Vehicle.CostDay is null) return;
        Cost = (double)Vehicle.CostKm * (double)DrivenKm + (double)Vehicle.CostDay * daysDifference;
        BookingClosed = true;
        DateReturned = returnDate;
        if (!OdometerReturned.HasValue) return;
        Vehicle.Odometer = (int)OdometerReturned;
        Vehicle.VehicleStatus = VehicleStatuses.Available;
    }
}
