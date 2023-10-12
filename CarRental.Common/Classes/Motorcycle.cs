using CarRental.Common.Enums;
using CarRental.Common.Interfaces;

namespace CarRental.Common.Classes;

public class Motorcycle : IVehicle
{
    public int Id { get; init; }
    public string RegNo { get; init; }
    public string Make { get; init; }
    public int Odometer { get; set; }
    public VehicleTypes VehicleType { get; init; }
    public VehicleStatuses VehicleStatus { get; set; } = VehicleStatuses.Available;
    public double CostKm { get; init; }
    public double CostDay { get; init; }

    public Motorcycle(int id, string regNo, string make, int odometer, VehicleTypes vehicleType, double costKm, double costDay)
        => (Id, RegNo, Make, Odometer, VehicleType, CostKm, CostDay) = (id, regNo, make, odometer, vehicleType, costKm, costDay);
}
