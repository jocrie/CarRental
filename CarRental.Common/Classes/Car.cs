using CarRental.Common.Enums;

namespace CarRental.Common.Classes;

public sealed class Car : Vehicle
{
    public Car(int id, string regNo, string make, int odometer, VehicleTypes vehicleType, double costKm, double costDay) 
        : base(id, regNo, make, odometer, vehicleType, costKm, costDay) { }
}
