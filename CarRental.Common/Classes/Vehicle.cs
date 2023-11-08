using CarRental.Common.Enums;
using CarRental.Common.Interfaces;

namespace CarRental.Common.Classes
{
    public class Vehicle : IVehicle
    {
        public int Id { get; set; }
        public string RegNo { get; set; } = string.Empty;
        public string Make { get; set; } = string.Empty;
        public int? Odometer { get; set; } = null;
        public VehicleTypes VehicleType { get; set; }
        public VehicleStatuses VehicleStatus { get; set; } = VehicleStatuses.Available;
        public double? CostKm { get; set; } = null;
        public double? CostDay { get; set; } = null;
        protected Vehicle(int id, string regNo, string make, int odometer, VehicleTypes vehicleType, double costKm, double costDay)
        => (Id, RegNo, Make, Odometer, VehicleType, CostKm, CostDay) = (id, regNo, make, odometer, vehicleType, costKm, costDay);
        public Vehicle()
        {
        }
    }
}
