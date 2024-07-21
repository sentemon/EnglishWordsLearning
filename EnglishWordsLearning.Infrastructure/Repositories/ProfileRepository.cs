using EnglishWordsLearning.Core.Interfaces;
using EnglishWordsLearning.Core.Models;
using EnglishWordsLearning.Infrastructure.Data;


namespace EnglishWordsLearning.Infrastructure.Repositories
{
    public class ProfileRepository : IProfileRepository
    {
        private readonly AppDbContext _appDbContext;

        public ProfileRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public User? GetUserProfile(string username)
        {
            return _appDbContext.Users.SingleOrDefault(u => u.Username == username);
        }

        public bool UpdateUserProfile(string username, User newProfile)
        {
            var existingProfile = GetUserProfile(username);

            if (existingProfile == null)
            {
                return false;
            }
            
            existingProfile.FirstName = newProfile.FirstName;
            existingProfile.LastName = newProfile.LastName;
            existingProfile.Email = newProfile.Email;
            
            _appDbContext.SaveChanges();
            
            return true;
        }

        // public bool ChangePassword(string username, ChangePassword model)
        // {
        //     var profile = GetUserProfile(username);
        //
        //     if (profile == null)
        //     {
        //         return false;
        //     }
        //
        //     return true;
        // }
    }
}