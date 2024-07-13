using Microsoft.AspNetCore.Mvc;
using EnglishWordsLearning.Services;

namespace EnglishWordsLearning.Controllers;

public class FlashCardsController : Controller
{
    
    public async Task<IActionResult> Index(string level = "AllLevels")
    {
        var words = await LoadWordsHelper.LoadCsvWordsAsync(ViewData, level);
        
        var randomWord = LoadWordsHelper.GetRandomWord(words);
        
        ViewBag.DisplayedLevel = LoadWordsHelper.Levels[level];
        ViewBag.SelectedLevel = level;
        
        return View(randomWord);
    }
}