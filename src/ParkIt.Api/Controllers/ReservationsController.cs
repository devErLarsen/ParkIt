using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ParkIt.Application.Services;
using ParkIt.Contracts.Request;
using ParkIt.Contracts.Response;

namespace ParkIt.Api.Controllers;

[Route("spots")]
public class ReservationsController : BaseController
{
    private readonly IReservationService _reservationService;

    public ReservationsController(IReservationService reservationService)
    {
        _reservationService = reservationService;
    }

    [Authorize(Policy = "is-employee")]
    [HttpPost("{parkingSpotId:guid}")]
    public async Task<ActionResult<ReservationResponse>> ReserveParkingSpot(Guid parkingSpotId, ReserveParkingSpotRequest request)
    {
        var reservation = await _reservationService.ReserveParkingSpot(request with
        {
            ParkingSpotId = parkingSpotId,
            EmployeeId = Guid.Parse(HttpContext.User.FindFirstValue("EmployeeId"))
        });
        return Created($"spots/{parkingSpotId}", reservation);
    }

    [Authorize]
    [HttpPut("{parkingSpotId:guid}/reservation/{reservationId:guid}")]
    public async Task<ActionResult> ChangeNumberPlate(Guid parkingSpotId, Guid reservationId, ChangeReservationNumberPlateRequest request)
    {
        await _reservationService.ChangeNumberPlate(request with
        {
            ParkingSpotId = parkingSpotId,
            ReservationId = reservationId
        });
        return NoContent();
    }

    [Authorize]
    [HttpDelete("{parkingSpotId:guid}/reservation/{reservationId:guid}")]
    public async Task<ActionResult> DeleteReservation(Guid parkingSpotId, Guid reservationId)
    {
        var employeeId = Guid.Parse(User.FindFirstValue("EmployeeId"));
        await _reservationService.DeleteReservation(new DeleteReservationRequest(parkingSpotId, reservationId, employeeId));
        return NoContent();
    }
}