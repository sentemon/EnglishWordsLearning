using EnglishWordsLearning.Models;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;

namespace EnglishWordsLearning.Services
{
    public static class LoadWordsHelper
    {
        public static readonly string JsonWordsFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "data", "words.json");
        
        private static readonly string CsvWordsFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "data", "words", "words.csv");
        
        public static async Task<List<WordViewModel>> LoadJsonWordsAsync()
        {
            using (var reader = new StreamReader(JsonWordsFilePath))
            {
                var json = await reader.ReadToEndAsync();
                
                return JsonConvert.DeserializeObject<List<WordViewModel>>(json) ?? throw new InvalidOperationException();
            }
        }
        
        public static async Task<List<WordViewModel>> LoadCsvWordsAsync(ViewDataDictionary viewData, string level = "AllLevels")
        {
            var words = new List<WordViewModel>();

            try
            {
                using (var reader = new StreamReader(CsvWordsFilePath))
                {
                    var csv = await reader.ReadToEndAsync();
                    var lines = csv.Split("\n").Skip(1);

                    foreach (var line in lines)
                    {
                        if (string.IsNullOrWhiteSpace(line))
                            continue;

                        var parts = line.Split(";");

                        var word = new WordViewModel
                        {
                            English = parts[0],
                            Level = parts[1],
                            Transcription = parts[2],
                            Russian = parts[3],
                            Class = parts[4]
                        };

                        if (word.Level[0] == level[0] || level == "AllLevels")
                        {
                            words.Add(word);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                viewData["ValidateMessage"] = ex.Message;
            }

            return words;
        }
    }
}