// using System.ComponentModel.DataAnnotations;
//
// namespace EnglishWordsLearning.Core.Models
// {
//     public class ChangePassword
//     {
//         [Required]
//         public string Username { get; set; }
//
//         [Required]
//         [DataType(DataType.Password)]
//         public string CurrentPassword { get; set; }
//
//         [Required]
//         [DataType(DataType.Password)]
//         [StringLength(100, MinimumLength = 6)]
//         public string NewPassword { get; set; }
//
//         [DataType(DataType.Password)]
//         [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
//         public string ConfirmPassword { get; set; }
//     }
// }