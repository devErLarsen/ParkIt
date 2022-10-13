using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ParkIt.Application.Services;
using ParkIt.Contracts.Request;
using ParkIt.Contracts.Response;

namespace ParkIt.Api.Controllers;

[Route("spots")]
// [AllowAnonymous]
public class ParkingSpotsController : BaseController
{
    private readonly IParkingSpotService _parkingSpotService;

    public ParkingSpotsController(IParkingSpotService parkingSpotService)
    {
        _parkingSpotService = parkingSpotService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ParkingSpotResponse>>> GetAllParkingSpotsAsync()
        => Ok(await _parkingSpotService.GetAllSpotsAsync());

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ParkingSpotResponse>> GetParkingSpotAsync(Guid id)
        => Ok(await _parkingSpotService.GetSpotAsync(id));

    [Authorize(Policy = "is-admin")]
    [HttpPost]
    public async Task<ActionResult> CreateParkingSpotAsync(CreateParkingSpotRequest request)
    {
        await _parkingSpotService.CreateSpotAsync(request);
        return NoContent();
    }
    
    [Authorize(Policy = "is-admin")]
    [HttpPost("bulk")]
    public async Task<ActionResult<IEnumerable<ParkingSpotResponse>>> CreateCarSpotsAsync(CreateParkingSpotRequest[] request)
    {
        var spots = await _parkingSpotService.CreateSpotsAsync(request);
        return Ok(spots);
    }

    // [Authorize(Policy = "is-admin")]
    // [HttpPost("bike")]
    // public async Task<ActionResult<IEnumerable<ParkingSpotResponse>>> CreateBikeSpotsAsync(CreateParkingSpotRequest[] request)
    // {
    //     var spots = await _parkingSpotService.CreateSpotsAsync(request);
    //     return Ok(spots);
    // }
    //
    // [Authorize(Policy = "is-admin")]
    // [HttpPost("handicap")]
    // public async Task<ActionResult<IEnumerable<ParkingSpotResponse>>> CreateHandicapSpotsAsync(CreateParkingSpotRequest[] request)
    // {
    //     var spots = await _parkingSpotService.CreateSpotsAsync(request);
    //     return Ok(spots);
    // }
    
    [Authorize(Policy = "is-admin")]
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> DeleteParkingSpotAsync(Guid id)
    {
        await _parkingSpotService.DeleteSpotAsync(id);
        return NoContent();
    }

    [Authorize(Policy = "is-admin")]
    [HttpDelete]
    public async Task<ActionResult> DeleteParkingSpotsAsync(Guid[] parkingSpotIds)
    {
        await _parkingSpotService.DeleteSpotsAsync(parkingSpotIds);

        return NoContent();
    }
    
}