using EnglishWordsLearning.Core.Interfaces;
using EnglishWordsLearning.Infrastructure.Data;
using EnglishWordsLearning.Core.Models;

namespace EnglishWordsLearning.Infrastructure.Repositories;

public class ProfileRepository : IProfileRepository
{
    private readonly AppDbContext _appDbContext;

    public ProfileRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public User? GetUser(string username)
    {
        var userProfile = _appDbContext.Users
            .Where(u => u.Username == username)
            .Select(u => new User
            {
                FirstName = u.FirstName,
                LastName = u.LastName,
                Email = u.Email,
                Username = u.Username,
                Password = u.Password
            })
            .FirstOrDefault();

        return userProfile;
    }
}