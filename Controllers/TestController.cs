using Microsoft.AspNetCore.Mvc;

namespace EnglishWordsLearning.Controllers;

public class TestController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}