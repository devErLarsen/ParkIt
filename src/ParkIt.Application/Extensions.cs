using Microsoft.Extensions.DependencyInjection;
using ParkIt.Application.Services;
using ParkIt.Contracts.Response;
using ParkIt.Core.Entities;

namespace ParkIt.Application;

public static class Extensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        return services
            .AddScoped<IReservationService, ReservationService>()
            .AddScoped<IParkingSpotService, ParkingSpotService>()
            .AddScoped<IEmployeeService, EmployeeService>();
    }

    public static ParkingSpotResponse ToResponse(this ParkingSpot parkingSpot)
    {
        return new ParkingSpotResponse(
            parkingSpot.GetType().Name,
            parkingSpot.Id.ToString(),
            parkingSpot.SpotCode,
            parkingSpot.Reservations.Select(r => r.ToResponse()).ToList());
    }

    public static ReservationResponse ToResponse(this Reservation reservation)
    {
        return new ReservationResponse(
            reservation.Id,
            reservation.Employee.ToResponse(),
            reservation.Employee.Vehicles
                .SingleOrDefault(x => x.NumberPlate == reservation.NumberPlate).ToResponse(),
            reservation.ReservationPeriod.Start,
            reservation.ReservationPeriod.End);
    }

    public static EmployeeResponse ToResponse(this Employee employee)
        => new(employee.Id,
            employee.Name,
            employee.Email,
            employee.HandicapPrivilege);

    public static EmployeeWithVehiclesResponse ToResponseWithVehicles(this Employee employee)
        => new(employee.Id,
            employee.Name,
            employee.Email,
            employee.Vehicles.Select(v => v.ToResponse()).ToList());

    public static VehicleResponse ToResponse(this Vehicle vehicle)
        => new(vehicle.NumberPlate.Value, vehicle.Type.ToString());

}