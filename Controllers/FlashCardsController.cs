using Microsoft.AspNetCore.Mvc;
using EnglishWordsLearning.Services;
using EnglishWordsLearning.Models;

namespace EnglishWordsLearning.Controllers;

public class FlashCardsController : Controller
{
    private static readonly Dictionary<string, string> Levels = new()
    {
        { "AllLevels", "All Levels" },
        { "a1;a2", "Beginner" },
        { "b1;b2", "Intermediate" },
        { "c1", "Advanced" }
    };

    public async Task<IActionResult> Index(string level = "AllLevels")
    {
        ViewBag.DisplayedLevel = Levels[level];
        ViewBag.SelectedLevel = level;
        
        var words = await LoadWordsHelper.LoadCsvWordsAsync(ViewData, level);
        var randomWord = LoadWordsHelper.GetRandomWord(words);
        
        int totalQuestions = HttpContext.Session.GetInt32("totalQuestions") ?? 0;
        HttpContext.Session.SetInt32("totalQuestions", totalQuestions);
        
        ViewBag.TotalQuestions = totalQuestions;
        totalQuestions++; // doesn't work
        
        return View(randomWord);
    }
}