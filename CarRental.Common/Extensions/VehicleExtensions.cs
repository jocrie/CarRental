namespace CarRental.Common.Extensions;

public static class VehicleExtensions
{
    public static int Duration(this DateOnly startDate, DateOnly? endDate)
    {
        if (endDate is null) return 0; 
        return (endDate.Value.DayNumber - startDate.DayNumber) + 1;
    }
}
