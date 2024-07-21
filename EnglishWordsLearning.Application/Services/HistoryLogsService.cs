using EnglishWordsLearning.Core.Interfaces;
using EnglishWordsLearning.Infrastructure.Data;
using EnglishWordsLearning.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace EnglishWordsLearning.Application.Services;

public class HistoryLogsService : IHistoryLogsService
{
    private readonly IHistoryLogsRepository _historyLogsRepository;

    public HistoryLogsService(IHistoryLogsRepository historyLogsRepository)
    {
        _historyLogsRepository = historyLogsRepository;
    }

    public DbSet<HistoryLogs> GetHistoryLogs()
    {
        var historyLogs = _historyLogsRepository.GetHistoryLogs();

        return historyLogs;
    }

    public async Task HistoryLogsOfTestsAdd(int totalQuestions, int correctAnswers, double resultInPercentage, string username, string level = "AllLevels")
    {
        await _historyLogsRepository.HistoryLogsOfTestsAdd(totalQuestions, correctAnswers, resultInPercentage, username,
            level);
    }
}