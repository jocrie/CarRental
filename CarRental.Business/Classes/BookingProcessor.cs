using CarRental.Common.Classes;
using CarRental.Common.Enums;
using CarRental.Common.Interfaces;
using CarRental.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Business.Classes;

public class BookingProcessor
{
    private readonly IData _data;

    public BookingProcessor(IData data) => _data = data;

    public IEnumerable<Customer> GetCustomers()
    {
        return _data.GetPersons().OfType<Customer>();
    }

    public IEnumerable<IVehicle> GetVehicles(VehicleStatuses status = default)
    {
        return _data.GetVehicles(status);
    }

    public IEnumerable<IBooking> GetBookings()
    {
        //Logik för att beräkna kostnader med mera
        //var bookingsImported = _data.GetBookings();

        List<IBooking> bookingsProcessed = new List<IBooking>();

        foreach (var bI in _data.GetBookings()) 
        {
            if (bI is null)
                break;
            // Process booking if car is available otherwise skip booking
            if (bI.Vehicle.VehicleStatus.Equals(VehicleStatuses.Available))
            {
                bI.RentCar(bI);
                bookingsProcessed.Add(bI);
            }
            
            return bookingsProcessed.AsEnumerable();

            // Return car if all data provided and valid

            


        }
        return _data.GetBookings();
    }



}
