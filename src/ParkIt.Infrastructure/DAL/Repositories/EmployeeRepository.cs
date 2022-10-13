using Microsoft.EntityFrameworkCore;
using ParkIt.Core.Entities;
using ParkIt.Core.Repositories;

namespace ParkIt.Infrastructure.DAL.Repositories;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly ParkItDbContext _context;
    private readonly DbSet<Employee> _employees;

    public EmployeeRepository(ParkItDbContext context)
    {
        _context = context;
        _employees = _context.Employees;
    }

    public async Task<IEnumerable<Employee>> GetAllAsync()
        => await _employees
            // .Include(x => x.Vehicles)
            .ToListAsync();

    public async Task<Employee> GetAsync(Guid id)
        => await _employees
            .Include(x => x.Vehicles)
            .FirstOrDefaultAsync(x => x.Id == id);

    // public async Task<Employee> GetByIdentityIdAsync(string id)
    //     => throw new NotImplementedException();

    public async Task<Employee> GetByEmailAsync(string email)
        => await _employees
            // .Include(x => x.Reservations)
            .FirstOrDefaultAsync(x => x.Email == email);
    
    public async Task<IEnumerable<Vehicle>> GetEmployeeVehicles(Guid employeeId)
    {
        var employee = await _employees
            .Include(x => x.Vehicles)
            .FirstOrDefaultAsync(x => x.Id == employeeId);
        
        return employee?.Vehicles;
    }

    public async Task AddAsync(Employee entity)
    {
        await _employees.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Employee entity)
    {
        _employees.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Employee entity)
    {
        _employees.Remove(entity);
        await _context.SaveChangesAsync();
    }
}