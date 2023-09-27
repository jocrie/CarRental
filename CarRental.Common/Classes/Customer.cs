using CarRental.Common.Interfaces;

namespace CarRental.Common.Classes;

public class Customer : IPerson
{
    // readonly List<Customer> _customers = new List<Customer>();
    public int Ssn { get; init; }
    public string LastName { get; init; }
    public string FirstName { get; init; }

    public Customer(int ssn, string lastName, string firstName) => (Ssn, LastName, FirstName) = (ssn, lastName, firstName);
}
