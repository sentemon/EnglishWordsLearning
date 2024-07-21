using EnglishWordsLearning.Core.Models;
using EnglishWordsLearning.Infrastructure.Data;
using EnglishWordsLearning.Core.Interfaces;


namespace EnglishWordsLearning.Infrastructure.Repositories;

public class AccountRepository : IAccountRepository
{
    private readonly AppDbContext _appDbContext;

    public AccountRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public List<User> LoadUsersFromDb()
    {
        var users = _appDbContext.Users;
            
        return users.ToList();
    }
    
    public User GetCurrentUser(string username)
    {
        var user = _appDbContext.Users.SingleOrDefault(u => u.Username == username);

        if (user == null)
        {
            throw new KeyNotFoundException("User not found.");
        }

        return user;
    }

    public void SaveUserToDb(User user)
    {
        _appDbContext.Users.Add(user);
        _appDbContext.SaveChanges();
    }
}