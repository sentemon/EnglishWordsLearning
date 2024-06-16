using System.ComponentModel.DataAnnotations;

namespace EnglishWordsLearning.Models
{
    public class SignUpViewModel
    {
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