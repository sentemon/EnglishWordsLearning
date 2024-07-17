namespace EnglishWordsLearning.Core.Models;

public class Word
{
    public Guid Id { get; set; }
    public string English { get; set; }
    public string Russian { get; set; }
    public string Transcription { get; set; }
    public string Level { get; set; }
    public string Class { get; set; }
}
