using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ParkIt.Application.Services;
using ParkIt.Contracts.Request;
using ParkIt.Contracts.Response;
using ParkIt.Core.ValueObjects;

namespace ParkIt.Api.Controllers;

[Route("employees")]
public class EmployeesController : BaseController
{
    private readonly IEmployeeService _employeeService;

    public EmployeesController(IEmployeeService employeeService)
    {
        _employeeService = employeeService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<EmployeeResponse>>> Get()
        => Ok(await _employeeService.GetAllEmployeesAsync());

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<EmployeeWithVehiclesResponse>> Get(Guid id)
        => await _employeeService.GetEmployeeAsync(id);

    [Authorize(Policy = "is-employee")]
    [HttpGet("me")]
    public async Task<ActionResult<EmployeeWithVehiclesResponse>> GetFromToken()
    {
        var id = HttpContext.User.FindFirstValue("EmployeeId");
        return await _employeeService.GetEmployeeAsync(Guid.Parse(id));
        // var id = HttpContext.User.FindFirstValue("IdentityId");
        // return await _userService.GetCurrentEmployee(id);
    }

    [Authorize(Policy = "is-admin")]
    [HttpPost("handicap/{id:guid}")]
    public async Task<ActionResult> ChangeHandicapStatus(Guid id)
    {
        var result = await _employeeService.ToggleHandicapStatus(id);
        return Ok(result);
    }

    [Authorize(Policy = "is-employee")]
    [HttpGet("vehicles")]
    public async Task<ActionResult<IEnumerable<VehicleResponse>>> GetVehicles()
    {
        var employeeId = User.FindFirstValue("EmployeeId");
        var vehicles = await _employeeService.GetEmployeeVehiclesAsync(Guid.Parse(employeeId));
        return Ok(vehicles);
    }

    [Authorize(Policy = "is-employee")]
    [HttpPost("vehicles")]
    public async Task<ActionResult> AddVehicle(AddVehicleRequest request)
    {
        var id = HttpContext.User.FindFirstValue("EmployeeId");
        await _employeeService.AddEmployeeVehicle(request with { EmployeeId = Guid.Parse(id)});

        return NoContent();
    }
    
    [Authorize(Policy = "is-employee")]
    [HttpDelete("vehicles/{numberPlate}")]
    public async Task<ActionResult> RemoveVehicle(string numberPlate)
    {
        var id = HttpContext.User.FindFirstValue("EmployeeId");
        await _employeeService.RemoveEmployeeVehicle(new RemoveVehicleRequest(EmployeeId: Guid.Parse(id), NumberPlate: numberPlate));

        return NoContent();
    }
}