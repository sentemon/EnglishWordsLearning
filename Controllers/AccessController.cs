using Microsoft.AspNetCore.Mvc;

namespace EnglishWordsLearning.Controllers;

public class AccessController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}