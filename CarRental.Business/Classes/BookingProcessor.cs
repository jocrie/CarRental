using CarRental.Common.Classes;
using CarRental.Common.Enums;
using CarRental.Common.Extensions;
using CarRental.Common.Interfaces;
using CarRental.Data.Interfaces;

namespace CarRental.Business.Classes;

public class BookingProcessor
{
    private readonly IData _data;
    private readonly UIinput _input;

    public bool Processing { get; private set; } = false;
    public BookingProcessor(IData data, UIinput input)
    {
        _data = data;
        _input = input;
    }

    public string[] VehicleStatusNames => _data.VehicleStatusNames;
    public string[] VehicleTypeNames => _data.VehicleTypeNames;
    public VehicleTypes GetVehicleType(string name) => _data.GetVehicleType(name);

    public IEnumerable<Customer> GetCustomers() => _data.Get<IPerson>(null).Cast<Customer>();
    public Customer? GetCustomer(int customerId) => GetCustomers().SingleOrDefault(c => c.Id == customerId);
    public void AddCustomer()
    {
        try
        {
            _input.NewCustomer.Id = _data.NextPersonId;
            _input.inputErrorCustomer = false;
            _input.addCustomerErrors = new();

            _input.NewCustomer.FirstName = _input.NewCustomer.FirstName.Capitalize();
            _input.NewCustomer.LastName = _input.NewCustomer.LastName.Capitalize();

            if (_input.NewCustomer.Ssn?.ToString().Length != _input.LengthSsn)
            {
                _input.addCustomerErrors.Add($"Unique SSN with {_input.LengthSsn} digits and no leading zeros");
                _input.inputErrorCustomer = true;
            }

            if (_input.NewCustomer.FirstName.Length < _input.minLengthName || !_input.NewCustomer.FirstName.IsLettersOnly() || _input.NewCustomer.LastName.Length < _input.minLengthName || !_input.NewCustomer.LastName.IsLettersOnly())
            {
                _input.addCustomerErrors.Add($"First and last name with at least {_input.minLengthName} letters");
                _input.inputErrorCustomer = true;
            }

            if (GetCustomers().Any(c => c.Ssn == _input.NewCustomer.Ssn))
            {
                _input.addCustomerErrors.Add($"SSN that does not exist in database yet");
                _input.inputErrorCustomer = true;
            }

            if (_input.inputErrorCustomer) throw new ArgumentException("Input error"); // Is displayed in UI

            _data.Add(_input.NewCustomer);

            _input.NewCustomer = new Customer();
        }
        catch (ArgumentException) { }
        catch (Exception ex)
        {
            _input.unforseenError = $"Unforseen error: {ex.Message}";
        }
    }

    public IEnumerable<IVehicle> GetVehicles(VehicleStatuses status = default)
    {
        if (status != default) return _data.Get<IVehicle>(v => v.VehicleStatus == status);
        else return _data.Get<IVehicle>(null);
    }
    public IVehicle? GetVehicle(int vehicleId) => _data.Single<IVehicle>(v => v.Id == vehicleId);
    public void AddVehicle()
    {
        try
        {
            _input.NewVehicle.Id = _data.NextVehicleId;
            _input.inputErrorVehicle = false;
            _input.addVehicleErrors = new();

            if (_input.NewVehicle.RegNo is null || _input.NewVehicle.Make is null || _input.NewVehicle.CostDay is null || _input.NewVehicle.CostKm is null)
                throw new ArgumentNullException();

            _input.NewVehicle.RegNo = _input.NewVehicle.RegNo.ToUpper();
            _input.NewVehicle.Make = _input.NewVehicle.Make.Capitalize();

            if (_input.NewVehicle.RegNo.Length != _input.LengthRegNo)
            {
                _input.addVehicleErrors.Add($"Unique registration number with {_input.LengthRegNo} characters");
                _input.inputErrorVehicle = true;
            }

            if (GetVehicles().Any(v => v.RegNo == _input.NewVehicle.RegNo))
            {
                _input.addVehicleErrors.Add($"Registration number that does not exist in database yet");
                _input.inputErrorVehicle = true;
            }

            if (_input.NewVehicle.Make.Length < _input.minLengthMake || !_input.NewVehicle.Make.IsLettersOnly())
            {
                _input.addVehicleErrors.Add($"Make name with at least {_input.minLengthMake} letters. Only letters are allowed");
                _input.inputErrorVehicle = true;
            }
            if (_input.NewVehicle.CostKm < 0 || _input.NewVehicle.CostDay < 0 || _input.NewVehicle.Odometer < 0)
            {
                _input.addVehicleErrors.Add($"Positive Odometer value (int), positive Cost per km (double) and positive Cost per day (int) ");
                _input.inputErrorVehicle = true;
            }
            if (_input.inputErrorVehicle) throw new ArgumentException("Input error"); // Is displayed in UI

            _input.unforseenError = string.Empty;


            if (_input.NewVehicle is not null)
            {
                if (_input.NewVehicle.VehicleType == VehicleTypes.Motorcycle)
                {
                    IVehicle newMotorcycle = new Motorcycle(_input.NewVehicle);
                    _data.Add(newMotorcycle);
                }
                else
                {
                    IVehicle newCar = new Car(_input.NewVehicle);
                    _data.Add(newCar);
                }
            }

            _input.NewVehicle = new Vehicle();

        }
        catch (ArgumentNullException)
        {
            _input.inputErrorVehicle = true;
            _input.addVehicleErrors.Add("Provide all input fields");
        }
        catch (ArgumentException) { }
        catch (Exception ex)
        {
            _input.unforseenError = $"Unforseen error: {ex.Message}";
        }
    }

    public IEnumerable<IBooking> GetBookings() => _data.Get<IBooking>(null);

    public async Task<List<IBooking>> RentVehicle(int vehicleId, int customerId)
    {
        Processing = true;
        await Task.Delay(2000);
        var newBooking = _data.RentVehicle(vehicleId, customerId);

        if (newBooking != null)
            _data.Add(newBooking);

        Processing = false;
        return _data.Get<IBooking>(null).ToList();
    }

    public void ReturnVehicle(int vehicleId)
    {
        try
        {
            _input.rentReturnError = string.Empty;
            if (_input.rentDrivenKm < 1 || _input.rentDrivenKm is null) throw new ArgumentException("Distance reuired to be whole number and bigger than 0");
            if (_input.returnDate is null) throw new ArgumentException("Return date required");

            _data.ReturnVehicle(vehicleId, (int)_input.rentDrivenKm, DateOnly.FromDateTime((DateTime)_input.returnDate));

            _input.rentDrivenKm = null;
        }
        catch (ArgumentException ex)
        {
            _input.rentReturnError = ex.Message;
        }
        catch (Exception ex)
        {
            _input.unforseenError = $"Unforseen error: {ex.Message}";
        }
    }

    //FOR TESTING
    /*public void RemoveCar(int carIndexToRemove)
    {
        _data.RemoveAvehicle(carIndexToRemove);
    }*/

    }
