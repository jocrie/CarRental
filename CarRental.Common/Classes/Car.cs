using CarRental.Common.Enums;
using CarRental.Common.Interfaces;

namespace CarRental.Common.Classes;

public class Car : IVehicle
{
    public int VehicleId { get; init; }
    public string RegNo { get; init; }
    public string Make { get; init; }
    public int Odometer { get; set; }
    public VehicleTypes VehicleType { get; init; }
    public VehicleStatuses VehicleStatus { get; set; } = VehicleStatuses.Available;
    public double CostKm { get; init; }
    public double CostDay { get; init; }

    public Car(int vehicleId, string regNo, string make, int odometer, VehicleTypes vehicleType, double costKm, double costDay)
        => (VehicleId, RegNo, Make, Odometer, VehicleType, CostKm, CostDay) = (vehicleId, regNo, make, odometer, vehicleType, costKm, costDay);
}
