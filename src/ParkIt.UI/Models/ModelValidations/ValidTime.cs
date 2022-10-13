using System.ComponentModel.DataAnnotations;

namespace ParkIt.UI.Models.ModelValidations;

public class ValidTime : ValidationAttribute
{
    public TimeSpan Time { get; set; }

    // protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    // {

    // }
}