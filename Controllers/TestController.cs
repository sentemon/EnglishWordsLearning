using EnglishWordsLearning.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace EnglishWordsLearning.Controllers;

public class TestController : Controller
{
    // private readonly string _jsonFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "data", "words.json");
    private readonly string _csvFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "data", "words", "words.csv");

    private readonly Dictionary<string, string> _levels = new()
    {
        { "AllLevels", "All Levels" },
        { "a1;a2", "Beginner" },
        { "b1;b2", "Intermediate" },
        { "c1", "Advanced" }
    };
    
    private async Task<List<WordViewModel>> LoadWordsAsync(string level = "AllLevels")
    {
        var words = new List<WordViewModel>();

        try
        {
            using (var reader = new StreamReader(_csvFilePath))
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
            ViewData["ValidateMessage"] = ex.ToString();
        }

        return words;
    }
    
    public async Task<IActionResult> CheckTranslation(string level = "AllLevels")
    {
        ViewBag.SelectedLevel = level;
    
        return View();
    }


    [HttpPost]
    public async Task<IActionResult> CheckTranslation(string russian, string userTranslation, string level)
    {
        var words = await LoadWordsAsync(level);
        var word = words.FirstOrDefault(w => w.Russian == russian);
        
        // Select a random word from the same level
        var random = new Random();
        var randomWord = words.MinBy(w => random.Next());
            
        int correctAnswers = HttpContext.Session.GetInt32("correctAnswers") ?? 0;
        int totalQuestions = HttpContext.Session.GetInt32("totalQuestions") ?? 0;

        if (word != null)
        {
            totalQuestions++;
            
            if (word.English == userTranslation)
            {
                correctAnswers++;
                ViewBag.Result = "Correct!";
            }
            else
            {
                ViewBag.Result = "Incorrect. The correct translation is: " + word.English;
            }
        }


        // Save correct answers and total questions back to session
        HttpContext.Session.SetInt32("correctAnswers", correctAnswers);
        HttpContext.Session.SetInt32("totalQuestions", totalQuestions);

        

        // Pass the result and count to the view
        ViewBag.CorrectAnswers = correctAnswers;
        ViewBag.TotalQuestions = totalQuestions;
        
        ViewBag.SelectedLevel = level;

        return View(randomWord);
    }
        
}