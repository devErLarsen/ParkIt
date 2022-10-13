using ParkIt.Core.Exceptions;

namespace ParkIt.Core.ValueObjects;

public record ReservationPeriod
{
    public DateTime Start { get; }
    public DateTime End { get; }
    // public TimeSpan Duration { get; }

    private ReservationPeriod()
    {
    }
    public ReservationPeriod(DateTime start, DateTime end)
    {
        var startOfDay = start.Date.AddHours(7);
        var endOfDay = start.Date.AddHours(18);
        var validPeriod = start < end
            && start >= startOfDay && start < end
            && end <= endOfDay;
        if (!validPeriod)
        {
            throw new InvalidReservationPeriodException();
        }
        Start = start;
        End = end;
    }

    public override string ToString()
    {
        return $"{Start} -> {End}";
    }
}