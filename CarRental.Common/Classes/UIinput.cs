using CarRental.Common.Enums;
using CarRental.Common.Interfaces;

namespace CarRental.Common.Classes;

public class UIinput
{
    public VehicleStatuses vStatus = default;

    /* Error handling */
    public List<string> addCustomerErrors = new();
    public List<string> addVehicleErrors = new();
    public string rentReturnError = string.Empty;
    public string unforseenError = string.Empty;

    /* New customer */
    public bool inputErrorCustomer = false;
    public int LengthSsn = 5, minLengthName = 2;

    /* New vehicle */
    public bool inputErrorVehicle = false;
    public int LengthRegNo = 6, minLengthMake = 2;

    /* Rent and return */
    public int rentCustomerId = 1; //Select Customer with id 1 when page is loaded
    public int? rentDrivenKm = null;
    public int minDrivenKm = 1, maxDrivenKm = 10000;
    public DateTime? returnDate = DateTime.Now; //Set to todays date when page is loaded

    public IPerson NewCustomer { get; set; } = new Customer();
    public IVehicle NewVehicle { get; set; } = new Vehicle();
}
