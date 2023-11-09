using CarRental.Common.Classes;
using CarRental.Common.Enums;
using CarRental.Common.Interfaces;
using CarRental.Data.Interfaces;
using System.Reflection;

namespace CarRental.Data.Classes;

public class CollectionData : IData
{
    readonly List<IPerson> _persons = new List<IPerson>();
    readonly List<IVehicle> _vehicles = new List<IVehicle>();
    readonly List<IBooking> _bookings = new List<IBooking>();

    /*Generate ids automatically*/
    public int NextVehicleId => _vehicles.Count.Equals(0) ? 1 : _vehicles.Max(i => i.Id) + 1;
    public int NextPersonId => _persons.Count.Equals(0) ? 1 : _persons.Max(i => i.Id) + 1;
    public int NextBookingId => _bookings.Count.Equals(0) ? 1 : _bookings.Max(i => i.Id) + 1;

    public CollectionData() => SeedData();

    public string[] VehicleStatusNames => Enum.GetNames(typeof(VehicleStatuses));
    public string[] VehicleTypeNames => Enum.GetNames(typeof(VehicleTypes));
    public VehicleTypes GetVehicleType(string name) => (VehicleTypes)Enum.Parse(typeof(VehicleTypes), name);

    void SeedData()
    {
        _persons.Add(new Customer(NextPersonId, 12345, "Doe", "John"));
        _persons.Add(new Customer(NextPersonId, 98765, "Doe", "Jane"));

        _vehicles.Add(new Car(NextVehicleId, "ABC123", "Volvo", 10000, VehicleTypes.Combi, 1, 200));
        _vehicles.Add(new Car(NextVehicleId, "DEF456", "Saab", 20000, VehicleTypes.Sedan, 1, 100));
        _vehicles.Add(new Car(NextVehicleId, "GHI789", "Tesla", 1000, VehicleTypes.Sedan, 3, 100));
        _vehicles.Add(new Car(NextVehicleId, "JKL012", "Jeep", 5000, VehicleTypes.Van, 1.5, 300));
        _vehicles.Add(new Motorcycle(NextVehicleId, "MNO345", "Yamaha", 30000, VehicleTypes.Motorcycle, 0.5, 50));
    }

    public List<T> Reflection<T>() where T : class
    {
        var collectionProperty = GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance)
            .FirstOrDefault(f => f.FieldType == typeof(List<T>) && f.IsInitOnly)
            ?? throw new InvalidOperationException("Unsupported type");

        var value = collectionProperty.GetValue(this) ?? throw new InvalidDataException("No data found");
        return (List<T>)value;
    }

    public List<T> Get<T>(Func<T, bool>? expression) where T : class
    {
        var collection = Reflection<T>().AsQueryable();
        if (expression is null) return collection.ToList();

        return collection.Where(expression).ToList();
    }

    public T? Single<T>(Func<T, bool> expression) where T : class
    {
        var collection = Reflection<T>().AsQueryable();
        var item = collection.SingleOrDefault(expression);

        return item ?? throw new InvalidOperationException("More than one or no matching item found.");
    }

    public void Add<T>(T item) where T : class
    {
        Reflection<T>().Add(item);
    }

    public IBooking? RentVehicle(int vehicleId, int customerId)
    {
        var vehicle = Single<IVehicle>(v => v.Id == vehicleId);
        var customer = Single<IPerson>(p => p.Id == customerId);
        if (vehicle is null || customer is null || vehicle.Odometer is null) return null;
        vehicle.VehicleStatus = VehicleStatuses.Booked;
        DateOnly dateRented = DateOnly.FromDateTime(DateTime.Now);
        var newBooking = new Booking(NextBookingId, vehicle, (Customer)customer, dateRented, (int)vehicle.Odometer);
        
        return newBooking;
    }

    public void ReturnVehicle(int vehicleId, int drivenKm, DateOnly? returnDate)
    {
        var vehicle = Single<IVehicle>(v => v.Id == vehicleId);
        var booking = Single<IBooking>(b => (b.Vehicle == vehicle) && (b.BookingClosed == false));
        if(vehicle is null || booking is null || returnDate is null) return;

        booking.ProcessReturn(drivenKm, (DateOnly)returnDate);
        vehicle.VehicleStatus = VehicleStatuses.Available;
    }

    //TEST ONLY
/*    public void RemoveAvehicle(int index)
    {
        _vehicles.RemoveAt(index);
    }*/

}
