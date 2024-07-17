using EnglishWordsLearning.Core.Models;

namespace EnglishWordsLearning.Core.Interfaces;

public interface IProfileRepository
{
    public User? GetUser(string username);
}