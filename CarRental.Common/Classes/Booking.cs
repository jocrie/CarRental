using CarRental.Common.Interfaces;
using CarRental.Common.Classes;
namespace CarRental.Common.Classes;

public class Booking : IBooking
{
    public IVehicle Vehicle { get; }
    public IPerson Person { get; }
    public int? OdometerRented { get; set; } = null;
    public int? OdometerReturned { get; set; } = null;
    public DateOnly DateRented { get; init; }
    public DateOnly? DateReturned { get; set; } = null;
    public int? DrivenKm { get; set; } = null;
    

    // public double Cost { get; }

    public bool BookingClosed { get; } = false;

    public Booking(IVehicle vehicle, IPerson person, DateOnly dateRented, int? drivenKm = null, DateOnly? dateReturned = null)
    {
        Vehicle = vehicle;
        Person = person;
        DateRented = dateRented;
        DrivenKm = drivenKm;
        DateReturned = dateReturned;
        
    }

    public void RentCar(IBooking booking)
    {
        OdometerRented = booking.Vehicle.Odometer;
        booking.Vehicle.VehicleStatus = Enums.VehicleStatuses.Booked;
    }
}
