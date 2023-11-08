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
            uiinput.NewCustomer.Id = _data.NextPersonId;
            uiinput.inputErrorCustomer = false;
            uiinput.addCustomerErrors = new();

            uiinput.NewCustomer.FirstName = uiinput.NewCustomer.FirstName.Capitalize();
            uiinput.NewCustomer.LastName = uiinput.NewCustomer.LastName.Capitalize();

            if (uiinput.NewCustomer.Ssn?.ToString().Length != uiinput.LengthSsn)
            {
                uiinput.addCustomerErrors.Add($"Unique SSN with {uiinput.LengthSsn} digits and no leading zeros");
                uiinput.inputErrorCustomer = true;
            }

            if (uiinput.NewCustomer.FirstName.Length < uiinput.minLengthName || !uiinput.NewCustomer.FirstName.IsLettersOnly() || uiinput.NewCustomer.LastName.Length < uiinput.minLengthName || !uiinput.NewCustomer.LastName.IsLettersOnly())
            {
                uiinput.addCustomerErrors.Add($"First and last name with at least {uiinput.minLengthName} letters");
                uiinput.inputErrorCustomer = true;
            }
            if (GetCustomers().Any(c => c.Ssn == uiinput.NewCustomer.Ssn))
            {
                uiinput.addCustomerErrors.Add($"SSN that does not exist in database yet");
                uiinput.inputErrorCustomer = true;
            }

            if (uiinput.inputErrorCustomer) throw new ArgumentException("Input error"); // Is displayed in UI

            _data.Add(uiinput.NewCustomer);
            uiinput.NewCustomer = new Customer();
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
            uiinput.NewVehicle.Id = _data.NextVehicleId;
            uiinput.inputErrorVehicle = false;
            uiinput.addVehicleErrors = new();

            if (uiinput.NewVehicle.RegNo is null || uiinput.NewVehicle.Make is null || uiinput.NewVehicle.CostDay is null || uiinput.NewVehicle.CostKm is null)
                throw new ArgumentNullException();

            uiinput.NewVehicle.RegNo = uiinput.NewVehicle.RegNo.ToUpper();
            uiinput.NewVehicle.Make = uiinput.NewVehicle.Make.Capitalize();

            if (uiinput.NewVehicle.RegNo.Length != uiinput.LengthRegNo)
            {
                uiinput.addVehicleErrors.Add($"Unique registration number with {uiinput.LengthRegNo} characters");
                uiinput.inputErrorVehicle = true;
            }

            if (GetVehicles().Any(v => v.RegNo == uiinput.NewVehicle.RegNo))
            {
                uiinput.addVehicleErrors.Add($"Registration number that does not exist in database yet");
                uiinput.inputErrorVehicle = true;
            }

            if (uiinput.NewVehicle.Make.Length < uiinput.minLengthMake || !uiinput.NewVehicle.Make.IsLettersOnly())
            {
                uiinput.addVehicleErrors.Add($"Make name with at least {uiinput.minLengthMake} letters. Only letters are allowed");
                uiinput.inputErrorVehicle = true;
            }
            if (uiinput.NewVehicle.CostKm < 0 || uiinput.NewVehicle.CostDay < 0 || uiinput.NewVehicle.Odometer < 0)
            {
                uiinput.addVehicleErrors.Add($"Positive Odometer value (int), positive Cost per km (double) and positive Cost per day (int) ");
                uiinput.inputErrorVehicle = true;
            }
            if (uiinput.inputErrorVehicle) throw new ArgumentException("Input error"); // Is displayed in UI

            uiinput.unforseenError = string.Empty;


            if (uiinput.NewVehicle is not null)
            {
                if (uiinput.NewVehicle.VehicleType == VehicleTypes.Motorcycle) 
                {
                    Motorcycle newMotorcycle = new Motorcycle(uiinput.NewVehicle);
                    _data.Add(newMotorcycle);
                }
                else if (uiinput.NewVehicle.VehicleType == VehicleTypes.Motorcycle && uiinput.NewVehicle is not null)
                {
                    Car newCar = new Car(uiinput.NewVehicle);
                    _data.Add(newCar);
                }
            }

            uiinput.NewVehicle = new Vehicle();

        }
        catch (ArgumentNullException) 
        {
            uiinput.inputErrorVehicle = true;
            uiinput.addVehicleErrors.Add("Provide all input fields");
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
