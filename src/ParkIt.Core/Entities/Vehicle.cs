using ParkIt.Core.Enums;
using ParkIt.Core.ValueObjects;

namespace ParkIt.Core.Entities;

public record Vehicle(NumberPlate NumberPlate, VehicleType Type)
{
    public NumberPlate NumberPlate { get; private set; } = NumberPlate;
    public VehicleType Type { get; private set; } = Type;

    public static VehicleType ToVehicleType(string type) => Enum.Parse<VehicleType>(type);
}