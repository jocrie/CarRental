using CarRental.Common.Interfaces;
using CarRental.Common.Enums;
using CarRental.Common.Extensions;

namespace CarRental.Common.Classes;

public class Booking : IBooking
{
    public int Id { get; init; }
    //public int VehicleId { get; init; }
    public IVehicle Vehicle { get; init; }
    public Customer Customer { get; init; }
    //public int PersonId { get; init; }
    //public string RegNo { get; private set; } = string.Empty;
    // public string Customer { get; private set; } = string.Empty;
    public int? OdometerRented { get; private set; } = null;
    public int? OdometerReturned { get; private set; } = null;
    public DateOnly DateRented { get; init; }
    public DateOnly? DateReturned { get; private set; }
    public int? DrivenKm { get; init; } = null;
    public double Cost { get; private set; }
    public bool BookingClosed { get; private set; } = false;
    public bool BookingValid { get; private set; } = false;

    public Booking(int id, IVehicle vehicle, Customer customer, DateOnly dateRented, int? drivenKm = null, DateOnly? dateReturned = default)
    {
        Id = id;
        Vehicle = vehicle;
        Customer = customer;
        DateRented = dateRented;
        DrivenKm = drivenKm;
        DateReturned = dateReturned;
    }

    public void ValidateBooking()
    {
        BookingValid = true;
    }

    public void RentVehicle()
    {
        if (Vehicle.VehicleStatus == VehicleStatuses.Booked)
        {
            BookingValid = false;
            return;
        } 
        OdometerRented = Vehicle.Odometer;
        // RegNo = vehicle.RegNo;
        // Customer = $"{customer.LastName} {customer.FirstName} ({customer.Ssn})";
        Vehicle.VehicleStatus = VehicleStatuses.Booked;
    }

    public void ReturnVehicle()
    {
        if (DrivenKm == null || !DateReturned.HasValue) return;
        int daysDifference = DateRented.Duration(DateReturned);
        OdometerReturned = OdometerRented + DrivenKm;
        Cost = Vehicle.CostKm * (double)DrivenKm + Vehicle.CostDay * daysDifference;
        BookingClosed = true;
        Vehicle.VehicleStatus = VehicleStatuses.Available;
        if (!OdometerReturned.HasValue) return;
        Vehicle.Odometer = (int)OdometerReturned;
    }
}
