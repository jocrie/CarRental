namespace CarRental.Common.Interfaces;

public interface IPerson
{
    public int Id { get; init; }
    public int Ssn { get; init; }
    public string LastName { get; init; }
    public string FirstName { get; init; }
}
