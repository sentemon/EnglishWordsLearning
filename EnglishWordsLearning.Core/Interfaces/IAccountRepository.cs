using EnglishWordsLearning.Core.Models;

namespace EnglishWordsLearning.Core.Interfaces;

public interface IAccountRepository
{
    List<User> LoadUsersFromDb();
    
    public User GetCurrentUser(string username);

    public void SaveUserToDb(User user);

}