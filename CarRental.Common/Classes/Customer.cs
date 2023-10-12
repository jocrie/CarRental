using CarRental.Common.Interfaces;

namespace CarRental.Common.Classes;

public class Customer : IPerson
{
    public int Id { get; init; }
    public int Ssn { get; init; }
    public string LastName { get; init; }
    public string FirstName { get; init; }

    public Customer(int id, int ssn, string lastName, string firstName) => (Id, Ssn, LastName, FirstName) = (id, ssn, lastName, firstName);
}
