using EnglishWordsLearning.Models;
using Newtonsoft.Json;

namespace EnglishWordsLearning.Features
{
    
    public class LoadWords
    {
        public static readonly string JsonWordsFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "data", "words.json");
        
        
        public static async Task<List<WordViewModel>> LoadWordsAsync()
        {
            using (var reader = new StreamReader(JsonWordsFilePath))
            {
                var json = await reader.ReadToEndAsync();
                
                return JsonConvert.DeserializeObject<List<WordViewModel>>(json) ?? throw new InvalidOperationException();
            }
        }
    }
    
}
