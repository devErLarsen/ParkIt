using ParkIt.Core.Entities;
using ParkIt.Core.Enums;

namespace ParkIt.Contracts.Response;

public record VehicleResponse(string NumberPlate, string VehicleType);