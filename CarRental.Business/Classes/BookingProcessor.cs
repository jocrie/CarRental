using CarRental.Common.Classes;
using CarRental.Common.Enums;
using CarRental.Common.Extensions;
using CarRental.Common.Interfaces;
using CarRental.Data.Classes;
using CarRental.Data.Interfaces;
using System.Runtime.Intrinsics.X86;

namespace CarRental.Business.Classes;

public class BookingProcessor
{
    private readonly IData _data;

    public bool Processing { get; private set; } = false;

    public BookingProcessor(IData data) => _data = data;

    public IEnumerable<Customer> GetCustomers()
    {
        return _data.Get<IPerson>(null).Cast<Customer>();
    }

    public Customer? GetCustomer(int customerId)
    {
        return GetCustomers().SingleOrDefault(c => c.Id == customerId);
    }

    public IEnumerable<IVehicle> GetVehicles(VehicleStatuses status = default)
    {
        if (status != default) return _data.Get<IVehicle>(v => v.VehicleStatus == status);
        else return _data.Get<IVehicle>(null);
    }

    public IVehicle? GetVehicle(int vehicleId) 
    {
        return _data.Single<IVehicle>(v => v.Id == vehicleId);
    }

    public IEnumerable<IBooking> GetBookings()
    {
        return _data.Get<IBooking>(null);

    }

    public void AddCustomer(int ssn, string firstName, string lastName)
    {
        var newCustomer = new Customer(_data.NextPersonId, ssn, lastName, firstName);
        _data.Add(newCustomer);
    }

    public void AddVehicle(string newRegNo, string newMake, int? newOdometer, VehicleTypes newVehicleType, double? newCostKm, int? newCostDay)
    {
        if (newOdometer is null || newCostKm is null || newCostDay is null) return;
        IVehicle newVehicle;
        if (newVehicleType == VehicleTypes.Motorcycle) 
        {
            newVehicle = new Motorcycle(_data.NextVehicleId, newRegNo, newMake, (int)newOdometer, newVehicleType, (double)newCostKm, (int)newCostDay);
        }
        else
        {
            newVehicle = new Car(_data.NextVehicleId, newRegNo, newMake, (int)newOdometer, newVehicleType, (double)newCostKm, (int)newCostDay);
        }

        _data.Add(newVehicle);
    }

    public async Task<List<IBooking>> RentVehicle(int vehicleId, int customerId)
    {
        Processing = true;
        await Task.Delay(2000);
        var newBooking = _data.RentVehicle(vehicleId, customerId);
        _data.Add(newBooking);
        Processing = false;
        return _data.Get<IBooking>(null).ToList();
    }

    public void ReturnVehicle(int vehicleId, int? drivenKm, DateTime? returnDate)
    {
        if (drivenKm is null || returnDate is null) return;
        _data.ReturnVehicle(vehicleId, (int)drivenKm, DateOnly.FromDateTime((DateTime)returnDate));
    }

    public string[] VehicleStatusNames => _data.VehicleStatusNames;
    public string[] VehicleTypeNames => _data.VehicleTypeNames;
    public VehicleTypes GetVehicleType(string name) => _data.GetVehicleType(name);

    //FOR TESTING
    /*public void RemoveCar(int carIndexToRemove)
    {
        _data.RemoveAvehicle(carIndexToRemove);
    }*/

}
