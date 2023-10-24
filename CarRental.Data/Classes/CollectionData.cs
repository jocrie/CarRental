using CarRental.Common.Classes;
using CarRental.Common.Enums;
using CarRental.Common.Extensions;
using CarRental.Common.Interfaces;
using CarRental.Data.Interfaces;

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
        _vehicles.Add(new Car(NextVehicleId, "MNO345", "Yamaha", 30000, VehicleTypes.Motorcycle, 0.5, 50));

        /*_bookings.Add(new Booking(NextBookingId, _vehicles[0], (Customer)_persons[0], new DateOnly(2023, 9, 9)));
        _bookings.Add(new Booking(NextBookingId, _vehicles[0], (Customer)_persons[0], new DateOnly(2023, 9, 9))); *//*Ska inte processas eftersom billen inte tillgänglig*//*
        _bookings.Add(new Booking(NextBookingId, _vehicles[1], (Customer)_persons[1], new DateOnly(2023, 9, 10), 100, new DateOnly(2023, 9, 11)));
        _bookings.Add(new Booking(NextBookingId, _vehicles[1], (Customer)_persons[1], new DateOnly(2023, 9, 12), 100, new DateOnly(2023, 9, 16)));
        _bookings.Add(new Booking(NextBookingId, _vehicles[1], (Customer)_persons[1], new DateOnly(2023, 9, 20), 100, new DateOnly(2023, 9, 25)));
        _bookings.Add(new Booking(NextBookingId, _vehicles[1], (Customer)_persons[1], new DateOnly(2023, 9, 20), 100, new DateOnly(2023, 9, 25)));


        *//*Process seed Bookings*//*
        foreach (var b in _bookings)
        {
            if (b == null || b.Vehicle is null || b.Customer is null) continue;

            var booking = RentVehicle(b.Vehicle.Id, b.Customer.Id);

            ReturnVehicle(b.Vehicle.Id);
        }*/
    }

    public void Add<T>(T item)
    {
        if (item is Customer customer)
        {
            _persons.Add((IPerson)customer);
        }
        if (item is Car car)
        {
            _vehicles.Add(car);
        }
        if (item is Motorcycle motorcycle)
        {
            _vehicles.Add(motorcycle);
        }
        if (item is Booking booking)
        {
            _bookings.Add(booking);
        }
    }

    //TEST ONLY
    public void RemoveAvehicle(int index)
    {
       _vehicles.RemoveAt(index);
    }

    public IEnumerable<IPerson> GetPersons() => _persons;
    public IEnumerable<IVehicle> GetVehicles(VehicleStatuses status = default)
    {
        if (status is 0)
            return _vehicles;
        else
            return _vehicles.Where(v => v.VehicleStatus.Equals(status));
    }
    public IEnumerable<IBooking> GetBookings() => _bookings;

    public IBooking? RentVehicle(int vehicleId, int customerId)
    {
        var vehicle = GetVehicles().SingleOrDefault(v => v.Id == vehicleId);
        var customer = GetPersons().SingleOrDefault(p => p.Id == customerId);
        if (vehicle == null || customer == null) return null;
        vehicle.VehicleStatus = VehicleStatuses.Booked;
        DateOnly dateRented = DateOnly.FromDateTime(DateTime.Now);
        var newBooking = new Booking(NextBookingId, vehicle, (Customer)customer, dateRented, vehicle.Odometer);
        
        return newBooking;
    }

    public void ReturnVehicle(int vehicleId, int drivenKm, DateOnly? returnDate)
    {
        var vehicle = GetVehicles().SingleOrDefault(v => v.Id == vehicleId);
        var booking = GetBookings().SingleOrDefault(b => (b.Vehicle == vehicle) && (b.BookingClosed == false));
        if(vehicle is null || booking is null || returnDate is null) return;

        booking.ProcessReturn(drivenKm, (DateOnly)returnDate);
        vehicle.VehicleStatus = VehicleStatuses.Available;
    }

    
}
