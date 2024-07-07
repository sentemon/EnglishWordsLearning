using Microsoft.AspNetCore.Mvc;
using EnglishWordsLearning.Services;
using EnglishWordsLearning.Models;

namespace EnglishWordsLearning.Controllers;

public class FlashCardsController : Controller
{
    

    public async Task<IActionResult> Index(string level = "AllLevels")
    {
        var words = await LoadWordsHelper.LoadCsvWordsAsync(ViewData, level);
        
        var randomWord = LoadWordsHelper.GetRandomWord(words);
        
        int totalQuestions = HttpContext.Session.GetInt32("totalQuestions") ?? 0;
        HttpContext.Session.SetInt32("totalQuestions", totalQuestions);
        
        ViewBag.TotalQuestions = totalQuestions;
        totalQuestions++; // doesn't work
        
        ViewBag.DisplayedLevel = LoadWordsHelper.Levels[level];
        ViewBag.SelectedLevel = level;
        
        return View(randomWord);
    }
}