// using ParkIt.Application.Contracts;

using ParkIt.Contracts.Request;
using ParkIt.Contracts.Response;
using ParkIt.Core.Entities;
using ParkIt.Core.Repositories;

namespace ParkIt.Application.Services;

public class ParkingSpotService : IParkingSpotService
{
    private readonly IParkingSpotRepository _parkingSpots;
    public ParkingSpotService(IParkingSpotRepository parkingSpots)
    {
        _parkingSpots = parkingSpots;
    }

    public async Task<IEnumerable<ParkingSpotResponse>> GetAllSpotsAsync()
    {
        var parkingSpots = await _parkingSpots.GetAllAsync();
        return parkingSpots.Select(x => x.ToResponse()).OrderBy(x => x.CodeName);
    }

    public async Task<ParkingSpotResponse> GetSpotAsync(Guid parkingSpotId)
    {
        var parkingSpot = await _parkingSpots.GetAsync(parkingSpotId);
        return parkingSpot.ToResponse();
    }

    public async Task CreateSpotAsync(CreateParkingSpotRequest request)
        => await _parkingSpots.AddAsync(CreateSpot(request));

    public async Task<IEnumerable<ParkingSpotResponse>> CreateSpotsAsync(IEnumerable<CreateParkingSpotRequest> request)
    {
        var spots = request.Select(CreateSpot).ToArray();
        await _parkingSpots.AddRangeAsync(spots);
        return spots.Select(p => p.ToResponse());
    }
    
    private ParkingSpot CreateSpot(CreateParkingSpotRequest request)
        => request.Type switch
        {
            nameof(CarSpot) => new CarSpot(Guid.NewGuid(), request.Name),
            nameof(BikeSpot) => new BikeSpot(Guid.NewGuid(), request.Name),
            nameof(HandicapSpot) => new HandicapSpot(Guid.NewGuid(), request.Name),
            _ => throw new Exception()
        };

    public async Task DeleteSpotAsync(Guid parkingSpotId)
    {
        var parkingSpot = await _parkingSpots.GetAsync(parkingSpotId);
        await _parkingSpots.DeleteAsync(parkingSpot);
    }

    public async Task DeleteSpotsAsync(Guid[] parkingSpotIds)
    {
        var parkingSpots = (await _parkingSpots.GetAllAsync())
            .Where(x => parkingSpotIds.Contains(x.Id)).ToArray();
        await _parkingSpots.DeleteRangeAsync(parkingSpots);
    }

    public async Task UpdateSpotAsync(Guid parkingSpotId)
    {
        var parkingSpot = await _parkingSpots.GetAsync(parkingSpotId);
        await _parkingSpots.UpdateAsync(parkingSpot);
    }
}