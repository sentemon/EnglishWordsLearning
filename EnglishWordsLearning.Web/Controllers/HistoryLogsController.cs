using EnglishWordsLearning.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EnglishWordsLearning.Web.Controllers;

public class HistoryLogsController : Controller
{
    private readonly IHistoryLogsRepository _historyLogsRepository;

    public HistoryLogsController(IHistoryLogsRepository historyLogsRepository)
    {
        _historyLogsRepository = historyLogsRepository;
    }

    public IActionResult HistoryLogsOfTests()
    {
        var historyLogs = _historyLogsRepository.GetHistoryLogs();

        return View(historyLogs);
    }
}