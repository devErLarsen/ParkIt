// using ParkIt.Application.Contracts;
using ParkIt.Application.Exceptions;
using ParkIt.Contracts.Request;
using ParkIt.Contracts.Response;
using ParkIt.Core.Entities;
using ParkIt.Core.Repositories;
using ParkIt.Core.Services;
using ParkIt.Core.ValueObjects;

namespace ParkIt.Application.Services;

public class ReservationService : IReservationService
{
    private readonly IParkingSpotRepository _parkingSpots;
    private readonly IEmployeeRepository _employees;
    private readonly IReserveService _reserve;

    public ReservationService(IParkingSpotRepository parkingSpots, IEmployeeRepository employees, IReserveService reserve)
    {
        _parkingSpots = parkingSpots;
        _employees = employees;
        _reserve = reserve;
    }

    public async Task<ReservationResponse> ReserveParkingSpot(ReserveParkingSpotRequest request)
    {
        var parkingSpot = await _parkingSpots.GetAsync(request.ParkingSpotId);
        if (parkingSpot is null)
            throw new ParkingSpotNotFoundException(request.ParkingSpotId.ToString());

        var employee = await _employees.GetAsync(request.EmployeeId);
        if (employee is null)
            throw new UserNotFoundException(request.EmployeeId.ToString());

        var reservationPeriod = new ReservationPeriod(request.From, request.To);

        var reservation = new Reservation(
            Guid.NewGuid(), 
            // request.EmployeeId, 
            employee,
            new NumberPlate(request.NumberPlate),
            reservationPeriod);
        // parkingSpot.ReserveParkingSpot(reservation);
        var allParkingSpots = await _parkingSpots.GetAllAsync();
        _reserve.ReserveParking(allParkingSpots, reservation, parkingSpot);
        await _parkingSpots.UpdateAsync(parkingSpot);

        return reservation.ToResponse();
    }

    public async Task ChangeNumberPlate(ChangeReservationNumberPlateRequest request)
    {
        var (parkingSpotId, reservationId, numberPlate) = request;
        var parkingSpot = await _parkingSpots.GetAsync(parkingSpotId);
        if (parkingSpot is null)
            throw new ParkingSpotNotFoundException(parkingSpotId.ToString());

        var reservation = parkingSpot.Reservations.SingleOrDefault(x => x.Id == reservationId);

        if (reservation is null)
            throw new ReservationNotFoundException(reservationId.ToString());

        reservation.ChangeNumberPlate(new NumberPlate(numberPlate));
        await _parkingSpots.UpdateAsync(parkingSpot);
    }

    public async Task DeleteReservation(DeleteReservationRequest request)
    {
        var (parkingSpotId, reservationId, employeeId) = request;
        var parkingSpot = await _parkingSpots.GetAsync(parkingSpotId);
        if (parkingSpot is null)
            throw new ParkingSpotNotFoundException(parkingSpotId.ToString());

        var reservation = parkingSpot.Reservations.SingleOrDefault(x => x.Id == reservationId);

        if (reservation is null)
            throw new ReservationNotFoundException(reservationId.ToString());

        parkingSpot.RemoveReservation(reservationId);
        await _parkingSpots.UpdateAsync(parkingSpot);
    }
}
