using EnglishWordsLearning.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EnglishWordsLearning.Web.Controllers;

public class HistoryLogsController : Controller
{
    private readonly IHistoryLogsService _historyLogsService;

    public HistoryLogsController(IHistoryLogsService historyLogsService)
    {
        _historyLogsService = historyLogsService;
    }

    public IActionResult HistoryLogsOfTests()
    {
        var historyLogs = _historyLogsService.GetHistoryLogs();

        return View(historyLogs);
    }
}