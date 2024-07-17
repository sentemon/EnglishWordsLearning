using Microsoft.AspNetCore.Mvc;
using EnglishWordsLearning.Infrastructure.Repositories;

namespace EnglishWordsLearning.Web.Controllers;

public class FlashCardsController : Controller
{
    
    public async Task<IActionResult> Index(string level = "AllLevels")
    {
        var words = await LoadWordsHelper.LoadCsvWordsAsync(level);
        
        var randomWord = LoadWordsHelper.GetRandomWord(words);
        
        ViewBag.DisplayedLevel = LoadWordsHelper.Levels[level];
        ViewBag.SelectedLevel = level;
        
        return View(randomWord);
    }
}