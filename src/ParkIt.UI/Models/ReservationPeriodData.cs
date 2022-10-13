using System.ComponentModel.DataAnnotations;

namespace ParkIt.UI.Models;

public record ReservationPeriodData : IValidatableObject
{
    // [Required]
    public DateTime? Date { get; set; }
    // [Required]
    public TimeSpan? Start { get; set; }
    // [Required]
    public TimeSpan? End { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (End < Start)
            yield return new ValidationResult("End time must be before start time");
    }
}