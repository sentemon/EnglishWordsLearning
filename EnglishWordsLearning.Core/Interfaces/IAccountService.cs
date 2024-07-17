using EnglishWordsLearning.Core.Models;

namespace EnglishWordsLearning.Core.Interfaces;

public interface IAccountService
{
    public bool SignInValidateUser(string username, string password);

    public string HashPassword(string password);

    List<User> LoadUsersFromDb();

    public string GetCurrentUsername();

    public void SaveUserToDb(User user);

    public bool SignUpValidateUserName(string username);
    public bool SignUpValidateUserPassword(string password);
    public bool SignUpValidateUserEmail(string? email);
    public bool SignUpValidateUserFullName(string firstName, string lastName);
}