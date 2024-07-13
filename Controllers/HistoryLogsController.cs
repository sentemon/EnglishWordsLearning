using EnglishWordsLearning.Data;
using Microsoft.AspNetCore.Mvc;

namespace EnglishWordsLearning.Controllers
{
    public class HistoryLogsController : Controller
    {
        private readonly AppDbContext _appDbContext;

        public HistoryLogsController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public IActionResult HistoryLogsOfTests()
        {
            var historyLogs = _appDbContext.HistoryLogs;

            return View(historyLogs);
        }
    }
}