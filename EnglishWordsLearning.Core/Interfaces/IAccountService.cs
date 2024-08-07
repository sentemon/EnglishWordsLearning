using EnglishWordsLearning.Core.Models;

namespace EnglishWordsLearning.Core.Interfaces;

public interface IAccountService
{
    public bool SignInValidateUser(string username, string password);

    public string HashPassword(string password);
    public bool CheckHashPasswords(string enterPassword, string userPassword);

    public string GetCurrentUsername();
    public User GetCurrentUser();

    public bool SignUpValidateUserName(string username);
    public bool SignUpValidateUserPassword(string password);
    public bool SignUpValidateUserEmail(string? email);
    public bool SignUpValidateUserFullName(string firstName, string lastName);
    
    public void SaveUserToDb(User user);
    public bool IsUsernameExists(string username);
}