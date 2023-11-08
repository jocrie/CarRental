namespace CarRental.Common.Interfaces;

public interface IPerson
{
    public int Id { get; set; }
    public int? Ssn { get; set; }
    public string LastName { get; set; }
    public string FirstName { get; set; }
}
