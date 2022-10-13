using ParkIt.Core.Entities;

namespace ParkIt.Contracts.Response;

public record EmployeeResponse(Guid Id, string Name, string Email, bool HasHandicapPrivileges)
{
    public bool HasHandicapPrivileges { get; set; } = HasHandicapPrivileges;
}
public record EmployeeWithVehiclesResponse(Guid Id, string Name, string Email, List<VehicleResponse> Vehicles);