using EnglishWordsLearning.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace EnglishWordsLearning.Core.Interfaces;

public interface IHistoryLogsService
{
    DbSet<HistoryLogs> GetHistoryLogs();
    Task HistoryLogsOfTestsAdd(int totalQuestions, int correctAnswers, double resultInPercentage, string username, string level = "AllLevels");
}