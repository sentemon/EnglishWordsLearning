using System.ComponentModel.DataAnnotations;

namespace EnglishWordsLearning.Models
{
    public class SignUpViewModel
    {
        [Display(Name = "Email")]
        public required string? Email { get; set; }
        
        [Required]
        [Display(Name = "First Name")]
        public required string FirstName { get; set; }
        
        [Required]
        [Display(Name = "Last Name")]
        public required string LastName { get; set; }
        
        [Required]
        [Display(Name = "Username")]
        public required string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public required string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public required string ConfirmPassword { get; set; }

        [Required]
        [Display(Name = "Accept Terms and Conditions")]
        public bool AcceptTerms { get; set; }
    }
}