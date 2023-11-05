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

    public void AddCustomer(UIinput uiinput)
    {
        try
        {
            uiinput.inputErrorCustomer = false;
            uiinput.addCustomerErrors = new();

            if (uiinput.newSsn.Length != uiinput.LengthSsn || !uiinput.newSsn.IsNumber())
            {
                uiinput.addCustomerErrors.Add($"Unique SSN with {uiinput.LengthSsn} digits and no leading zeros");
                uiinput.inputErrorCustomer = true;
            }

            if (uiinput.newFirstName.Length < uiinput.minLengthName || !uiinput.newFirstName.IsLettersOnly() || uiinput.newLastName.Length < uiinput.minLengthName || !uiinput.newLastName.IsLettersOnly())
            {
                uiinput.addCustomerErrors.Add($"First and last name with at least {uiinput.minLengthName} letters");
                uiinput.inputErrorCustomer = true;
            }
            if (uiinput.newSsn.IsNumber() && GetCustomers().Any(c => c.Ssn == (int.Parse(uiinput.newSsn))))
            {
                uiinput.addCustomerErrors.Add($"SSN that does not exist in database yet");
                uiinput.inputErrorCustomer = true;
            }

            if (uiinput.inputErrorCustomer) throw new ArgumentException("Input error"); // Is displayed in UI

            var newCustomer = new Customer(_data.NextPersonId, int.Parse(uiinput.newSsn), uiinput.newLastName.Capitalize(), uiinput.newFirstName.Capitalize());
            _data.Add(newCustomer);
            uiinput.newSsn = uiinput.newFirstName = uiinput.newLastName = string.Empty;
        }
        catch (ArgumentException) { }
        catch (Exception ex)
        {
            uiinput.unforseenError = $"Unforseen error: {ex.Message}";
        }


    }

    public void AddVehicle(UIinput uiinput)
    {
        try
        {
            uiinput.inputErrorVehicle = false;
            uiinput.addVehicleErrors = new();

            if (uiinput.newRegNo.Length != uiinput.LengthRegNo)
            {
                uiinput.addVehicleErrors.Add($"Unique registration number with {uiinput.LengthRegNo} characters");
                uiinput.inputErrorVehicle = true;
            }

            if (GetVehicles().Any(v => v.RegNo == uiinput.newRegNo.ToUpper()))
            {
                uiinput.addVehicleErrors.Add($"Registration number that does not exist in database yet");
                uiinput.inputErrorVehicle = true;
            }

            if (uiinput.newMake.Length < uiinput.minLengthMake || !uiinput.newMake.IsLettersOnly())
            {
                uiinput.addVehicleErrors.Add($"Make name with at least {uiinput.minLengthMake} letters. Only letters are allowed");
                uiinput.inputErrorVehicle = true;
            }
            if (uiinput.newOdometer is null || uiinput.newCostKm is null || uiinput.newCostDay is null || uiinput.newOdometer < 0 || uiinput.newCostKm < 0 || uiinput.newCostDay < 0)
            {
                uiinput.addVehicleErrors.Add($"Positive Odometer value (int), positive Cost per km (double) and positive Cost per day (int) ");
                uiinput.inputErrorVehicle = true;
            }
            if (uiinput.inputErrorVehicle) throw new ArgumentException("Input error"); // Is displayed in UI

            uiinput.unforseenError = string.Empty;


            if (uiinput.newOdometer is null || uiinput.newCostKm is null || uiinput.newCostDay is null) throw new Exception();
            IVehicle newVehicle;

            uiinput.newRegNo = uiinput.newRegNo.ToUpper();
            uiinput.newMake = uiinput.newMake.Capitalize();

            if (uiinput.newVehicleType == VehicleTypes.Motorcycle) 
            {
                newVehicle = new Motorcycle(_data.NextVehicleId, uiinput.newRegNo, uiinput.newMake, (int)uiinput.newOdometer, uiinput.newVehicleType, (double)uiinput.newCostKm, (int)uiinput.newCostDay);
            }
            else
            {
                newVehicle = new Car(_data.NextVehicleId, uiinput.newRegNo, uiinput.newMake, (int)uiinput.newOdometer, uiinput.newVehicleType, (double)uiinput.newCostKm, (int)uiinput.newCostDay);
            }

            _data.Add(newVehicle);


            uiinput.newRegNo = uiinput.newMake = string.Empty;
            uiinput.newOdometer = uiinput.newCostDay = null;
            uiinput.newCostKm = null;

        }
        catch (ArgumentException) { }
        catch (Exception ex)
        {
            uiinput.unforseenError = $"Unforseen error: {ex.Message}";
        }
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

    public void ReturnVehicle(int vehicleId, UIinput uiinput)
    {
        try
        {
            uiinput.rentReturnError = string.Empty;
            if (uiinput.rentDrivenKm < 1 || uiinput.rentDrivenKm is null) throw new ArgumentException("Distance reuired to be whole number and bigger than 0");
            if (uiinput.returnDate is null) throw new ArgumentException("Return date required");


            _data.ReturnVehicle(vehicleId, (int)uiinput.rentDrivenKm, DateOnly.FromDateTime((DateTime)uiinput.returnDate));

            uiinput.rentDrivenKm = null;

        }
        catch (ArgumentException ex)
        {
            uiinput.rentReturnError = ex.Message;
        }
        catch (Exception ex)
        {
            uiinput.unforseenError = $"Unforseen error: {ex.Message}";
        }


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
