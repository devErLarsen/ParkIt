using ParkIt.Core.Exceptions;

namespace ParkIt.Core.ValueObjects;

public record NumberPlate
{
    public string Value { get; }

    // private NumberPlate(){}
    public NumberPlate(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || value.Length is < 5 or > 9)
            throw new InvalidNumberPlateException(value);

        Value = value;
    }
}