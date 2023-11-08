using CarRental.Common.Enums;
using CarRental.Common.Interfaces;

namespace CarRental.Common.Classes;

public sealed class Car : Vehicle
{
    public Car(int id, string regNo, string make, int odometer, VehicleTypes vehicleType, double costKm, double costDay) 
        : base(id, regNo, make, odometer, vehicleType, costKm, costDay) { }

    public Car(IVehicle vehicle) : base(vehicle.Id, vehicle.RegNo, vehicle.Make, vehicle.Odometer ?? 0, vehicle.VehicleType, vehicle.CostKm ?? 0, vehicle.CostDay ?? 0) { }
}
