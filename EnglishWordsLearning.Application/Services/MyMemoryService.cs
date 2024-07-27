using Newtonsoft.Json.Linq;
using EnglishWordsLearning.Core.Interfaces;

namespace EnglishWordsLearning.Application.Services;

public class MyMemoryService : IMyMemoryService
{
    private static readonly HttpClient Client = new();

    public async Task<string?> TranslateAsync(string toLanguage, string text, string fromLanguage = "en")
    {
        var url = $"https://api.mymemory.translated.net/get?q={Uri.EscapeDataString(text)}&langpair={fromLanguage}|{toLanguage}";
        
        var response = await Client.GetAsync(url);
        response.EnsureSuccessStatusCode();
        
        var responseBody = await response.Content.ReadAsStringAsync();
        
        var json = JObject.Parse(responseBody);
        var translatedText = json["responseData"]?["translatedText"]?.ToString();

        return translatedText;
    }
}