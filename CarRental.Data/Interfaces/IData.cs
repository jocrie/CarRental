using CarRental.Common.Enums;
using CarRental.Common.Interfaces;

namespace CarRental.Data.Interfaces;

public interface IData
{
    public IEnumerable<IPerson> GetPersons();
    public IEnumerable<IVehicle> GetVehicles(VehicleStatuses status = default);
    public IEnumerable<IBooking> GetBookings();
}
