using CarRental.Common.Classes;
using CarRental.Common.Enums;
using CarRental.Common.Interfaces;
using CarRental.Data.Classes;
using CarRental.Data.Interfaces;
using System.Runtime.Intrinsics.X86;

namespace CarRental.Business.Classes;

public class BookingProcessor
{
    private readonly IData _data;

    public BookingProcessor(IData data) => _data = data;

    public IEnumerable<Customer> GetCustomers()
    {
        return _data.GetPersons().Cast<Customer>();
    }

    public IEnumerable<IVehicle> GetVehicles(VehicleStatuses status = default)
    {
        return _data.GetVehicles(status);
    }

    public IEnumerable<IBooking> GetBookings()
    {
        return _data.GetBookings();
    }

    public void AddCustomer(int ssn, string firstName, string lastName)
    {
        var newCustomer = new Customer(_data.NextPersonId, ssn, lastName, firstName);
        _data.Add(newCustomer);
    }

    public void AddVehicle(string newRegNo, string newMake, int newOdometer, VehicleTypes newVehicleType, double newCostKm, int newCostDay)
    {
        IVehicle newVehicle;
        if (newVehicleType == VehicleTypes.Motorcycle) 
        {
            newVehicle = new Motorcycle(_data.NextVehicleId, newRegNo, newMake, newOdometer, newVehicleType, newCostKm, newCostKm);
        }
        else
        {
            newVehicle = new Car(_data.NextVehicleId, newRegNo, newMake, newOdometer, newVehicleType, newCostKm, newCostKm);
        }

        _data.Add(newVehicle);
    }

    public void RemoveCar(int carIndexToRemove)
    {
        _data.RemoveAvehicle(carIndexToRemove);
    }

}
