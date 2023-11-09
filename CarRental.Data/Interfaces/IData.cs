using CarRental.Common.Enums;
using CarRental.Common.Interfaces;

namespace CarRental.Data.Interfaces;

public interface IData
{
    List<T> Get<T>(Func<T, bool>? expression) where T : class;
    T? Single<T>(Func<T, bool> expression) where T : class;
    public void Add<T>(T item) where T : class;
    public int NextVehicleId { get; }
    public int NextPersonId { get; }
    public int NextBookingId { get; }

    IBooking? RentVehicle(int vehicleId, int customerId);
    void ReturnVehicle(int vehicleId, int drivenKm, DateOnly? returnDate);

    public string[] VehicleStatusNames => Enum.GetNames(typeof(VehicleStatuses));
    public string[] VehicleTypeNames => Enum.GetNames(typeof(VehicleTypes));
    public VehicleTypes GetVehicleType(string name) => (VehicleTypes)Enum.Parse(typeof(VehicleTypes), name);

    //For Test
    //public void RemoveAvehicle(int index);
}
