using EnglishWordsLearning.Models;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace EnglishWordsLearning.Services;

public static class LoadWordsHelper
{
    private static readonly string CsvWordsFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "data", "words", "words.csv");
        
    public static readonly Dictionary<string, string> Levels = new()
    {
        { "AllLevels", "All Levels" },
        { "a1;a2", "Beginner" },
        { "b1;b2", "Intermediate" },
        { "c1", "Advanced" }
    };

    public static async Task<List<Word>> LoadCsvWordsAsync(ViewDataDictionary viewData, string level = "AllLevels")
    {
        var words = new List<Word>();

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

                    var word = new Word
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


    public static Word? GetRandomWord(List<Word> words)
    {
        var random = new Random();
        return words.MinBy(w => random.Next());
    }
}