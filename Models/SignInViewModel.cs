namespace EnglishWordsLearning.Models;

public class SignInViewModel
{
    public required string Username { get; set; }
    public required string Password { get; set; }
    public bool KeepLoggedIn { get; set; }
}