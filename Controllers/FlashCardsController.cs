using Microsoft.AspNetCore.Mvc;
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
    
}