using EnglishWordsLearning.Data;
using EnglishWordsLearning.Interfaces;
using EnglishWordsLearning.Models;

namespace EnglishWordsLearning.Services;

public class HistoryLogsService : IHistoryLogs
{
    private readonly AppDbContext _appDbContext;

    public HistoryLogsService(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
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