using System.ComponentModel.DataAnnotations;

namespace EnglishWordsLearning.Models
{
    public class User
    {
        public Guid Id { get; set; }
        
        [Required(ErrorMessage = "Username is required")]
        public required string Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public required string Password { get; set; }
    }
}