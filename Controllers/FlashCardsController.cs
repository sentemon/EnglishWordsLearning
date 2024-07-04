using Microsoft.AspNetCore.Mvc;
using EnglishWordsLearning.Services;
using EnglishWordsLearning.Models;

namespace EnglishWordsLearning.Controllers;

public class FlashCardsController : Controller
{
    private readonly string _jsonFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "data", "words.json");
    
    // GET
    public IActionResult Index()
    {
        return View();
    }

    public async Task<IActionResult> ShowCards()
    {
        var words = await Services.LoadWordsHelper.LoadCsvWordsAsync(ViewData);
        throw new Exception();

    }
    
    
    
}