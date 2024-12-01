using System.ComponentModel.DataAnnotations;
using COPiTOS.Validators;

namespace COPiTOS.DTOs;

public class PersonDto
{
    public int Id { get; set; }
    
    public string? Title { get; set; }
    
    [Required(ErrorMessage = "Please set First Name")]
    public string? FirstName { get; set; }
    
    [Required(ErrorMessage = "Please set Last Name")]
    public string? LastName { get; set; }
    
    [Required(ErrorMessage = "Please set Birth Date")]
    [DataType(DataType.Date)]
    [DateInPastValidator(ErrorMessage = "Birth Date must be in the past")]
    public DateTime? BirthDate { get; set; }
    
    public string? Address { get; set; }
    
    [Required(ErrorMessage = "Please set Index")]
    [RegularExpression(@"^[0-9]{5,5}$", ErrorMessage = "Please check the Index format")]
    public string? Index { get; set; }
    
    public string? City { get; set; }
    
    [RegularExpression(@"^[a-zA-Z]{2,2}$", ErrorMessage = "Please check the State format")]
    public string? State { get; set; }
}