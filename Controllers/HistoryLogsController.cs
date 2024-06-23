using Microsoft.AspNetCore.Mvc;

namespace EnglishWordsLearning.Controllers;

public class HistoryLogsController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}