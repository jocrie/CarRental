namespace CarRental.Common.Interfaces;

public interface IPerson
{
    public int Ssn { get; init; }
    public string LastName { get; init; }
    public string FirstName { get; init; }
}
