using EnglishWordsLearning.Core.Interfaces;
using EnglishWordsLearning.Core.Models;
using System.Threading.Tasks;

namespace EnglishWordsLearning.Application.Services
{
    public class ProfileService : IProfileService
    {
        private readonly IProfileRepository _profileRepository;

        public ProfileService(IProfileRepository profileRepository)
        {
            _profileRepository = profileRepository;
        }

        public User? GetUserProfile(string username)
        {
            return _profileRepository.GetUserProfile(username);
        }

        public bool UpdateUserProfile(string username, User profile)
        {
            return _profileRepository.UpdateUserProfile(username, profile);
        }

        // public bool ChangePassword(string username, ChangePassword model)
        // {
        //     return _profileRepository.ChangePassword(username, model);
        // }
    }
}