using System.ComponentModel.DataAnnotations;

namespace EnglishWordsLearning.Core.Models;

public class User
{
    public Guid Id { get; set; }
    
    [MaxLength(50)]
    [Required(ErrorMessage = "First Name is required")]
    public required string FirstName { get; set; }
    
    [MaxLength(50)]
    [Required(ErrorMessage = "Last Name is required")]
    public required string LastName { get; set; }

    [MaxLength(320)]
    public string? Email { get; set; }
    
    [MaxLength(30)]    
    [Required(ErrorMessage = "Username is required")]
    public required string Username { get; set; }
    
    [MaxLength(256)]
    [Required(ErrorMessage = "Password is required")]
    [DataType(DataType.Password)]
    public required string Password { get; set; }
    
    // language to translate
}