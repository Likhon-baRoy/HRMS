using System.ComponentModel.DataAnnotations;

namespace API.DTOs;

public class CreateEmployeeDto
{
    [Required]
    [MaxLength(50)]
    public string Name { get; set; }
    
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    
    public string Phone { get; set; }
    public string Department { get; set; }
    public string Position { get; set; }
    public string AccountNumber { get; set; }
    public string Status { get; set; }
}
