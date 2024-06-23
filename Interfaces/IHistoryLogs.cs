namespace EnglishWordsLearning.Interfaces
{
    public interface IHistoryLogs
    {
        Task HistoryLogsOfTestsAdd(int totalQuestions, int correctAnswers, double resultInPercentage, string level = "AllLevels");
    }
}

