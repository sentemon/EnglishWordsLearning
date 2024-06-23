namespace EnglishWordsLearning.Interfaces
{
    public interface IHistoryLogs
    {
        Task AddHistoryLogAsync(int totalQuestions, int correctAnswers, double resultInPercentage);
    }
}

