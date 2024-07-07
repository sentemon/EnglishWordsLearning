using EnglishWordsLearning.Services;
using EnglishWordsLearning.Interfaces;
using EnglishWordsLearning.Models;
using Microsoft.AspNetCore.Mvc;

namespace EnglishWordsLearning.Controllers;

public class TestController : Controller
{
    private readonly IHistoryLogs _historyLogs;
    
    public TestController(IHistoryLogs historyLogs)
    {
        _historyLogs = historyLogs;
    }
    
    // private readonly string _jsonFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "data", "words.json");
    
    public async Task<IActionResult> CheckTranslation(string level = "AllLevels")
    {
        ViewBag.DisplayedLevel = LoadWordsHelper.Levels[level];
        ViewBag.SelectedLevel = level;
        
        var words = await LoadWordsHelper.LoadCsvWordsAsync(ViewData, level);
        var randomWord = LoadWordsHelper.GetRandomWord(words);
        
        return View(randomWord);
    }
    
    [HttpPost]
    public async Task<IActionResult> CheckTranslation(string russian, string userTranslation, string level)
    {
        var words = await LoadWordsHelper.LoadCsvWordsAsync(ViewData, level);
        var word = words.FirstOrDefault(w => w.Russian == russian);
            
        int correctAnswers = HttpContext.Session.GetInt32("correctAnswers") ?? 0;
        int totalQuestions = HttpContext.Session.GetInt32("totalQuestions") ?? 0;
    
        var randomWord = LoadWordsHelper.GetRandomWord(words);
        
        if (word != null)
        {
            randomWord = LoadWordsHelper.GetRandomWord(words);
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
        
        ViewBag.DisplayedLevel = LoadWordsHelper.Levels[level];
        ViewBag.SelectedLevel = level;
        
        return View(randomWord);
    }

    

    public IActionResult FinishTranslation()
    {
        if (User.Identity != null && User.Identity.IsAuthenticated)
        {
            int totalQuestions = Convert.ToInt32(HttpContext.Session.GetInt32("totalQuestions"));
            int correctAnswers = Convert.ToInt32(HttpContext.Session.GetInt32("correctAnswers"));
            double resultInPercentage = totalQuestions > 0 ? (double)correctAnswers / totalQuestions * 100 : 0.0;
            string? level = HttpContext.Session.GetString("SelectedLevel");
            
            _historyLogs.HistoryLogsOfTestsAdd(totalQuestions, correctAnswers, resultInPercentage, level);
        }
        
        HttpContext.Session.Remove("correctAnswers");
        HttpContext.Session.Remove("totalQuestions");
        
        return RedirectToAction("CheckTranslation");
    }
        
}