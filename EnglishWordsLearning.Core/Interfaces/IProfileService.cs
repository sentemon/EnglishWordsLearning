using EnglishWordsLearning.Core.Models;
using System.Threading.Tasks;

namespace EnglishWordsLearning.Core.Interfaces
{
    public interface IProfileService
    {
        public User? GetUserProfile(string username);
        public bool UpdateUserProfile(string username, User profile);
        // public bool ChangePassword(string username, ChangePassword model);
    }
}