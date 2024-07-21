using EnglishWordsLearning.Core.Interfaces;
using EnglishWordsLearning.Infrastructure.Data;
using EnglishWordsLearning.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace EnglishWordsLearning.Infrastructure.Repositories;

public class HistoryLogsRepository : IHistoryLogsRepository
{
    private readonly AppDbContext _appDbContext;

    public HistoryLogsRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public DbSet<HistoryLogs> GetHistoryLogs()
    {
        var historyLogs = _appDbContext.HistoryLogs;

        return historyLogs;
    }

    public async Task HistoryLogsOfTestsAdd(int totalQuestions, int correctAnswers, double resultInPercentage, string username, string level = "AllLevels")
    {
        var newHistoryLogs = new HistoryLogs
        {
            Id = Guid.NewGuid(),
            Date = DateTime.UtcNow,
            Level = level,
            TotalQuestions = totalQuestions,
            CorrectAnswers = correctAnswers,
            ResultInPercentage = resultInPercentage,
            Username = username
        };

        _appDbContext.HistoryLogs.Add(newHistoryLogs);
        await _appDbContext.SaveChangesAsync();
    }
}