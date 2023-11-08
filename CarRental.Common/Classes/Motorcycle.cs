using CarRental.Common.Enums;
using CarRental.Common.Interfaces;

namespace CarRental.Common.Classes;

public sealed class Motorcycle : Vehicle
{
    public Motorcycle(int id, string regNo, string make, int odometer, VehicleTypes vehicleType, double costKm, double costDay) 
        : base(id, regNo, make, odometer, vehicleType, costKm, costDay) { }

    public Motorcycle(IVehicle vehicle) : base(vehicle.Id, vehicle.RegNo, vehicle.Make, vehicle.Odometer ?? 0, vehicle.VehicleType, vehicle.CostKm ?? 0, vehicle.CostDay ?? 0) { }
}
