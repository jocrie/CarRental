using CarRental.Common.Classes;
using CarRental.Common.Enums;
using CarRental.Common.Interfaces;
using System.Linq.Expressions;

namespace CarRental.Data.Interfaces;

public interface IData
{
    List<T> Get<T>(Expression<Func<T, bool>>? expression) where T : class;
    T? Single<T>(Expression<Func<T, bool>>? expression) where T : class;

    public IEnumerable<IPerson> GetPersons();
    public IEnumerable<IVehicle> GetVehicles(VehicleStatuses status = default);
    public IEnumerable<IBooking> GetBookings();
    public int NextVehicleId { get; }
    public int NextPersonId { get; }
    public int NextBookingId { get; }
    public void Add<T>(T item);

    IBooking? RentVehicle(int vehicleId, int customerId);
    void ReturnVehicle(int vehicleId, int drivenKm, DateOnly? returnDate);

    public string[] VehicleStatusNames => Enum.GetNames(typeof(VehicleStatuses));
    public string[] VehicleTypeNames => Enum.GetNames(typeof(VehicleTypes));
    public VehicleTypes GetVehicleType(string name) => (VehicleTypes)Enum.Parse(typeof(VehicleTypes), name);

    //For Test
    public void RemoveAvehicle(int index);
}
