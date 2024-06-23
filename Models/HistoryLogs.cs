using EnglishWordsLearning.Controllers;

namespace EnglishWordsLearning.Models
{
    public class HistoryLogsViewModel
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public int CorrectAnswers { get; set; }
        public int TotalQuestions { get; set; }
        public float ResultInPercentage { get; set; }
    }
}

