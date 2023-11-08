using CarRental.Common.Interfaces;

namespace CarRental.Common.Classes;

public class Customer : IPerson
{
    public int Id { get; set; }
    public int? Ssn { get; set; } = null;
    public string LastName { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    
    public string CustomerOutput => $"{LastName} {FirstName} ({Ssn})";

    public Customer(int id, int ssn, string lastName, string firstName) => (Id, Ssn, LastName, FirstName) = (id, ssn, lastName, firstName);

    public Customer()
    {
    }
}
