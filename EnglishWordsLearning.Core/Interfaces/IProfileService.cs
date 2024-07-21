using EnglishWordsLearning.Core.Models;
using System.Threading.Tasks;

namespace EnglishWordsLearning.Core.Interfaces
{
    public interface IProfileService
    {
        public Task<User?> GetUserProfile(string username);
        public Task<bool> UpdateUserProfile(string username, User profile);
        // public bool ChangePassword(string username, ChangePassword model);
    }
}