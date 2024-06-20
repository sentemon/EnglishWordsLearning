namespace EnglishWordsLearning.Models;

public class WordViewModel
{
    public Guid Id { get; set; }
    public required string English { get; set; }
    public required string Russian { get; set; }
    public required string Transcription { get; set; }
    public required string Level { get; set; }
    public required string Class { get; set; }
}
