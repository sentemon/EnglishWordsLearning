using EnglishWordsLearning.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace EnglishWordsLearning.Controllers;

public class TestController : Controller
{
    // private readonly string _jsonFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "data", "words.json");
    private readonly string _csvFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "data", "words", "words.csv");

    private async Task<List<WordViewModel>> LoadWordsAsync(string? level = null)
    {
        var words = new List<WordViewModel>();
        
        try
        {
            using (var reader = new StreamReader(_csvFilePath))
            {
                var csv = await reader.ReadToEndAsync();
                var lines = csv.Split("\n").Skip(1); // Skip header row

                foreach (var line in lines)
                {
                    if (string.IsNullOrWhiteSpace(line)) // Skip empty lines
                        continue;

                    var parts = line.Split(";");
                    if (parts.Length < 5) // Ensure there are enough parts
                        continue;

                    var word = new WordViewModel
                    {
                        English = parts[0],
                        Level = parts[1],
                        Transcription = parts[2],
                        Russian = parts[3],
                        Class = parts[4]
                    };

                    if (level == null || word.Level.Equals(level, StringComparison.OrdinalIgnoreCase))
                    {
                        words.Add(word);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ViewData["ValidateMessage"] = ex.ToString();
        }

        return words;
    }
    
    public async Task<IActionResult> Test(string level)
    {

        var words = await LoadWordsAsync(level);
        var random = new Random();
        var randomWord = words.MinBy(w => random.Next());

        ViewBag.SelectedLevel = level;
        
        return View(randomWord);
    }

    [HttpPost]
    public async Task<IActionResult> Test(string russian, string userTranslation, string level)
    {
        var words = await LoadWordsAsync();
        var word = words.FirstOrDefault(w => w.Russian == russian);
            
        int correctAnswers = HttpContext.Session.GetInt32("correctAnswers") ?? 0;
        int totalQuestions = HttpContext.Session.GetInt32("totalQuestions") ?? 0;
            
        totalQuestions++;

        // Check the user's translation
        if (word != null && word.English.Equals(userTranslation, StringComparison.OrdinalIgnoreCase))
        {
            correctAnswers++;
            ViewBag.Result = "Correct!";
        }
        else
        {
            ViewBag.Result = "Incorrect. The correct translation is: " + (word?.English ?? "Unknown");
        }

        // Save correct answers and total questions back to session
        HttpContext.Session.SetInt32("correctAnswers", correctAnswers);
        HttpContext.Session.SetInt32("totalQuestions", totalQuestions);

        // Select a new random word from the same level
        var random = new Random();
        var randomWord = words.MinBy(w => random.Next());

        // Pass the result and count to the view
        ViewBag.CorrectAnswers = correctAnswers;
        ViewBag.TotalQuestions = totalQuestions;
        
        ViewBag.SelectedLevel = level;

        return View(randomWord);
    }
}