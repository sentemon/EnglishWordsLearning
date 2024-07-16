namespace EnglishWordsLearning.Interfaces;

public interface IHistoryLogs
{
    Task HistoryLogsOfTestsAdd(int totalQuestions, int correctAnswers, double resultInPercentage, string username, string level = "AllLevels");
}