using CarRental.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    public string newSsn = string.Empty, newFirstName = string.Empty, newLastName = string.Empty;
    public int LengthSsn = 5, minLengthName = 2;

    /* New vehicle */
    public bool inputErrorVehicle = false;
    public string newRegNo = string.Empty, newMake = string.Empty;
    public VehicleTypes newVehicleType;
    public double? newCostKm = null;
    public int? newCostDay = null, newOdometer = null;
    public int LengthRegNo = 6, minLengthMake = 2;

    /* Rent and return */
    public int rentCustomerId = 1; //Select Customer with id 1 when page is loaded
    public int? rentDrivenKm = null;
    public int minDrivenKm = 1, maxDrivenKm = 10000;
    public DateTime? returnDate = DateTime.Now; //Set to todays date when page is loaded
}
