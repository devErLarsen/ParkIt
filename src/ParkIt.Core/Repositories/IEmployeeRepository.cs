using ParkIt.Core.Entities;

namespace ParkIt.Core.Repositories;

public interface IEmployeeRepository
{
    Task<Employee> GetAsync(Guid employeeId);
    // Task<Employee> GetByIdentityIdAsync(string id);
    Task<Employee> GetByEmailAsync(string email);
    Task<IEnumerable<Employee>> GetAllAsync();
    Task<IEnumerable<Vehicle>> GetEmployeeVehicles(Guid id);
    Task AddAsync(Employee employee);
    Task UpdateAsync(Employee employee);
    Task DeleteAsync(Employee employee);
}