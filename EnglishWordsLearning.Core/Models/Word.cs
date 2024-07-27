namespace EnglishWordsLearning.Core.Models;

public class Word
{
    public Guid Id { get; set; }
    public required string English { get; set; }
    //public string Russian { get; set; } // remove
    public string? Translation { get; set; }
    public required string Transcription { get; set; }
    public required string Level { get; set; }
    public required string Class { get; set; }
}
