namespace CarRental.Common.Interfaces;

public interface IBooking
{
    public IVehicle Vehicle { get; }
    public IPerson Person { get; }
    public int? OdometerRented { get; set; }
    public int? OdometerReturned { get; set; }
    public DateOnly DateRented { get; init; }
    public DateOnly? DateReturned { get; set; }
    public int? DrivenKm { get; set; }
    
    //public double Cost { get; }

    public bool BookingClosed { get; }

    public void RentCar(IBooking booking);
}
