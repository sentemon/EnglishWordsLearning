using Microsoft.AspNetCore.Mvc;
using EnglishWordsLearning.Models;

namespace EnglishWordsLearning.Controllers;

public class FlashCardController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}