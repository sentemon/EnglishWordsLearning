using EnglishWordsLearning.Helper;
using EnglishWordsLearning.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace EnglishWordsLearning.Controllers;

public class TestController : Controller
{
    // private readonly string _jsonFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "data", "words.json");
    
    private readonly Dictionary<string, string> _levels = new()
    {
        { "AllLevels", "All Levels" },
        { "a1;a2", "Beginner" },
        { "b1;b2", "Intermediate" },
        { "c1", "Advanced" }
    };
    
    
    public async Task<IActionResult> CheckTranslation(string level = "AllLevels")
    {
        ViewBag.DisplayedLevel = _levels[level];
        ViewBag.SelectedLevel = level;
        
        var words = await LoadWordsHelper.LoadCsvWordsAsync(ViewData, level);
        var randomWord = GetRandomWord(words);
        
        return View(randomWord);
    }
    
    [HttpPost]
    public async Task<IActionResult> CheckTranslation(string russian, string userTranslation, string level)
    {
        var words = await LoadWordsHelper.LoadCsvWordsAsync(ViewData, level);
        var word = words.FirstOrDefault(w => w.Russian == russian);
            
        int correctAnswers = HttpContext.Session.GetInt32("correctAnswers") ?? 0;
        int totalQuestions = HttpContext.Session.GetInt32("totalQuestions") ?? 0;

        var randomWord = GetRandomWord(words);
        
        if (word != null)
        {
            randomWord = GetRandomWord(words);
            totalQuestions++;
            
            if (word.English.Equals(userTranslation, StringComparison.OrdinalIgnoreCase))
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
        
        ViewBag.DisplayedLevel = _levels[level];
        ViewBag.SelectedLevel = level;

        return View(randomWord);
    }

    private WordViewModel? GetRandomWord(List<WordViewModel> word)
    {
        var random = new Random();
        return word.MinBy(w => random.Next());
    }

    public IActionResult RestartTranslation()
    {
        HttpContext.Session.Remove("correctAnswers");
        HttpContext.Session.Remove("totalQuestions");
        
        return RedirectToAction("CheckTranslation");
    }
        
}