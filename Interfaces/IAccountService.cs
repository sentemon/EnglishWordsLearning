using EnglishWordsLearning.Models;

namespace EnglishWordsLearning.Interfaces;

public interface IAccountService
{
    public bool SignInValidateUser(string username, string password);

    public bool SignUpValidateUser(string username, string password);

    public string HashPassword(string password);

    List<User> LoadUsersFromDb();

    public string GetCurrentUsername();

    public void SaveUserToDb(User user);
}