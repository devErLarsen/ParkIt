
using ParkIt.Contracts.Request;
using ParkIt.Contracts.Response;
using ParkIt.Core.Entities;

namespace ParkIt.Application.Services;

public interface IEmployeeService
{
    Task<IEnumerable<EmployeeResponse>> GetAllEmployeesAsync();
    Task<EmployeeWithVehiclesResponse> GetEmployeeAsync(Guid id);
    Task<IEnumerable<VehicleResponse>> GetEmployeeVehiclesAsync(Guid employeeId);
    Task<bool> ToggleHandicapStatus(Guid id);
    Task AddEmployeeVehicle(AddVehicleRequest request);
    Task RemoveEmployeeVehicle(RemoveVehicleRequest request);
}