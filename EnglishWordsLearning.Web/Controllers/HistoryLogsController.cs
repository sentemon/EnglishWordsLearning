using EnglishWordsLearning.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EnglishWordsLearning.Web.Controllers;

public class HistoryLogsController : Controller
{
    private readonly IHistoryLogs _historyLogs;

    public HistoryLogsController(IHistoryLogs historyLogs)
    {
        _historyLogs = historyLogs;
    }

    public IActionResult HistoryLogsOfTests()
    {
        var historyLogs = _historyLogs.GetHistoryLogs();

        return View(historyLogs);
    }
}