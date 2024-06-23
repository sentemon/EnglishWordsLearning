using EnglishWordsLearning.Data;
using EnglishWordsLearning.Interfaces;
using EnglishWordsLearning.Models;
using static EnglishWordsLearning.Controllers.TestController;
using Microsoft.AspNetCore.Mvc;

namespace EnglishWordsLearning.Controllers
{
    public class HistoryLogsController : Controller, IHistoryLogs
    {
        private readonly AppDbContext _appDbContext;

        public HistoryLogsController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public IActionResult HistoryLogsOfTests()
        {
            var historyLogs = _appDbContext.HistoryLogs.ToList() ?? null;
            
            if (historyLogs != null)
            {
                return View(historyLogs);
            }

            return View();
        }

        public async Task HistoryLogsOfTestsAdd(int totalQuestions, int correctAnswers, double resultInPercentage, string level = "AllLevels")
        {
            HistoryLogs newHistoryLogs = new HistoryLogs
            {
                Id = Guid.NewGuid(),
                Date = DateTime.UtcNow,
                Level = Levels[level],
                TotalQuestions = totalQuestions,
                CorrectAnswers = correctAnswers,
                ResultInPercentage = resultInPercentage
            };

            _appDbContext.HistoryLogs.Add(newHistoryLogs);
            await _appDbContext.SaveChangesAsync();
        }
    }
}