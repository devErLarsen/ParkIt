// using ParkIt.Application.Contracts;

using ParkIt.Contracts.Request;
using ParkIt.Contracts.Response;
using ParkIt.Core.Entities;

namespace ParkIt.Application.Services;

public interface IParkingSpotService
{
    Task<IEnumerable<ParkingSpotResponse>> GetAllSpotsAsync();
    Task<ParkingSpotResponse> GetSpotAsync(Guid parkingSpotId);
    Task CreateSpotAsync(CreateParkingSpotRequest request);
    Task<IEnumerable<ParkingSpotResponse>> CreateSpotsAsync(IEnumerable<CreateParkingSpotRequest> request);
    Task DeleteSpotAsync(Guid parkingSpotId);
    Task DeleteSpotsAsync(Guid[] parkingSpotIds);
    Task UpdateSpotAsync(Guid parkingSpotId);
}