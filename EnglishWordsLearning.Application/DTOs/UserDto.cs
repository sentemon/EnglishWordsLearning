using System.ComponentModel.DataAnnotations;

namespace EnglishWordsLearning.Core.Models;

public class UserDto
{
    public string Username { get; set; }
    
    [Required]
    public string FirstName { get; set; }
    
    [Required]
    public string LastName { get; set; }
    
    public string? Email { get; set; }
}