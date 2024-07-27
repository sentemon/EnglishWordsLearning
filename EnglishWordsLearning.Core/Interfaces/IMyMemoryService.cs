namespace EnglishWordsLearning.Core.Interfaces;

public interface IMyMemoryService
{
    public Task<string?> TranslateAsync(string toLanguage, string text, string fromLanguage = "en");
}