using System.Linq.Dynamic.Core;
using EnglishWordsLearning.Data;
using EnglishWordsLearning.Interfaces;
using EnglishWordsLearning.Models;
using EnglishWordsLearning.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EnglishWordsLearning.Controllers
{
    public class HistoryLogsController : Controller, IHistoryLogs
    {
        private readonly AppDbContext _appDbContext;
        private IHistoryLogs _historyLogsImplementation;

        public HistoryLogsController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public IActionResult HistoryLogsOfTests()
        {
            var historyLogs = _appDbContext.HistoryLogs;

            return View(historyLogs);
        }

        public async Task HistoryLogsOfTestsAdd(int totalQuestions, int correctAnswers, double resultInPercentage, string username, string level = "AllLevels")
        {
            HistoryLogs newHistoryLogs = new HistoryLogs
            {
                Id = Guid.NewGuid(),
                Date = DateTime.UtcNow,
                Level = LoadWordsHelper.Levels[level],
                TotalQuestions = totalQuestions,
                CorrectAnswers = correctAnswers,
                ResultInPercentage = resultInPercentage,
                Username = username
            };

            _appDbContext.HistoryLogs.Add(newHistoryLogs);
            await _appDbContext.SaveChangesAsync();
        }
    }
}