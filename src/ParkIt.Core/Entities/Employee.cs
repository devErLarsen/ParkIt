using System.Text.RegularExpressions;
using ParkIt.Core.Enums;
using ParkIt.Core.Exceptions;
using ParkIt.Core.ValueObjects;

namespace ParkIt.Core.Entities;

public class Employee
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string Email { get; private set; }
    public bool HandicapPrivilege { get; private set; }
    public DateTime CreatedAt { get; private set; }
    
    public IEnumerable<Vehicle> Vehicles => _vehicles;
    private HashSet<Vehicle> _vehicles = new();

    private static readonly Regex _emailPattern = new(@"^[a-zA-Z0-9]+(?:\.[a-zA-Z0-9]+)*@[a-zA-Z0-9]+(?:\.[a-zA-Z0-9]+)*$", RegexOptions.Compiled);

    public Employee(Guid id, string name, string email, DateTime createdAt)
    {
        Id = id;
        SetName(name);
        SetEmail(email);
        CreatedAt = createdAt;
    }

    public void AddVehicle(NumberPlate numberPlate, VehicleType type)
        => _vehicles.Add(new Vehicle(numberPlate, type));

    public void RemoveVehicle(NumberPlate numberPlate)
        => _vehicles.RemoveWhere(x => x.NumberPlate == numberPlate);

    public void UpdateEmployee(string name, string email)
    {
        SetName(name);
        SetEmail(email);
    }

    private void SetName(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || value.Length is > 30 or < 3)
            throw new InvalidNameException(value);
        Name = value;
    }

    private void SetEmail(string value)
    {
        if (!_emailPattern.IsMatch(value))
            throw new InvalidEmailException(value);
        Email = value;
    }

    public bool ToggleHandicapPrivilege()
    {
        HandicapPrivilege = !HandicapPrivilege;
        return HandicapPrivilege;
    }
}