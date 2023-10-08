using CarRental.Common.Interfaces;
using CarRental.Common.Enums;

namespace CarRental.Common.Classes;

public class Booking : IBooking
{
    public int VehicleId { get; init; }
    public int PersonId { get; init; }
    public string RegNo { get; private set; } = string.Empty;
    public string Customer { get; private set; } = string.Empty;
    public int? OdometerRented { get; private set; } = null;
    public int? OdometerReturned { get; private set; } = null;
    public DateOnly DateRented { get; init; }
    public DateOnly? DateReturned { get; private set; }
    public int? DrivenKm { get; init; } = null;
    public double Cost { get; private set; }
    public bool BookingClosed { get; private set; } = false;
    public bool BookingValid { get; private set; } = true;

    public Booking(int vehicleId, int personId, DateOnly dateRented, int? drivenKm = null, DateOnly? dateReturned = default)
    {
        VehicleId = vehicleId;
        PersonId = personId;
        DateRented = dateRented;
        DrivenKm = drivenKm;
        DateReturned = dateReturned;
    }

    public void InvalidateBooking()
    {
        BookingValid = false;
    }

    public void RentVehicle(IVehicle vehicle, Customer customer)
    {
        if (vehicle.VehicleStatus == VehicleStatuses.Booked)
        {
            BookingValid = false;
            return;
        } 
        OdometerRented = vehicle.Odometer;
        RegNo = vehicle.RegNo;
        Customer = $"{customer.LastName} {customer.FirstName} ({customer.Ssn})";
        vehicle.VehicleStatus = VehicleStatuses.Booked;
    }

    public void ReturnVehicle(IVehicle vehicle)
    {
        if (DrivenKm == null || !DateReturned.HasValue) return;
        int daysDifference = (DateReturned.Value.DayNumber - DateRented.DayNumber) + 1;
        OdometerReturned = OdometerRented + DrivenKm;
        Cost = vehicle.CostKm * (double)DrivenKm + vehicle.CostDay * daysDifference;
        BookingClosed = true;
        vehicle.VehicleStatus = VehicleStatuses.Available;
        if (!OdometerReturned.HasValue) return;
        vehicle.Odometer = (int)OdometerReturned;
    }
}
