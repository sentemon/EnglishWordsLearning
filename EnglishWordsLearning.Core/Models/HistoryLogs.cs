using System.ComponentModel.DataAnnotations;

namespace EnglishWordsLearning.Core.Models;

public class HistoryLogs
{
    public Guid Id { get; set; }
    
    public DateTime Date { get; set; }
    
    public string? Level { get; set; }
    
    public int TotalQuestions { get; set; }
    
    public int CorrectAnswers { get; set; }
    
    public double ResultInPercentage { get; set; }
    
    public string? Username { get; set; }
}