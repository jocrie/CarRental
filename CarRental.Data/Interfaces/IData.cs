using CarRental.Common.Classes;
using CarRental.Common.Enums;
using CarRental.Common.Interfaces;

namespace CarRental.Data.Interfaces;

public interface IData
{
    public IEnumerable<IPerson> GetPersons();
    public IEnumerable<IVehicle> GetVehicles(VehicleStatuses status = default);
    public IEnumerable<IBooking> GetBookings();
    public int NextVehicleId { get; }
    public int NextPersonId { get; }
    public int NextBookingId { get; }
    public void Add<T>(T item);

    //For Test
    public void RemoveAvehicle(int index);
}
