using ParkIt.Contracts.Request;
using ParkIt.Contracts.Response;
using ParkIt.Core.Entities;
using ParkIt.Core.Enums;
using ParkIt.Core.Repositories;
using ParkIt.Core.ValueObjects;

namespace ParkIt.Application.Services;

public class EmployeeService : IEmployeeService
{
    private readonly IEmployeeRepository _employees;

    public EmployeeService(IEmployeeRepository employees)
    {
        _employees = employees;
    }

    public async Task<EmployeeWithVehiclesResponse> GetEmployeeAsync(Guid id)
        => (await _employees.GetAsync(id)).ToResponseWithVehicles();

    public async Task<IEnumerable<VehicleResponse>> GetEmployeeVehiclesAsync(Guid employeeId)
    {
        return (await _employees.GetEmployeeVehicles(employeeId)).Select(x => x.ToResponse());
    }

    public async Task<bool> ToggleHandicapStatus(Guid id)
    {
        var employee = await _employees.GetAsync(id);
        if (employee == null)
            throw new Exception();
        var value = employee.ToggleHandicapPrivilege();
        await _employees.UpdateAsync(employee);
        return value;
    }

    public async Task<IEnumerable<EmployeeResponse>> GetAllEmployeesAsync()
        => (await _employees.GetAllAsync()).Select(x => x.ToResponse());

    public async Task AddEmployeeVehicle(AddVehicleRequest request)
    {
        var employee = await _employees.GetAsync(request.EmployeeId);
        employee.AddVehicle(new NumberPlate(request.NumberPlate), Vehicle.ToVehicleType(request.Type));
        await _employees.UpdateAsync(employee);
    }

    public async Task RemoveEmployeeVehicle(RemoveVehicleRequest request)
    {
        var employee = await _employees.GetAsync(request.EmployeeId);
        employee.RemoveVehicle(new NumberPlate(request.NumberPlate));
        await _employees.UpdateAsync(employee);
    }

}
