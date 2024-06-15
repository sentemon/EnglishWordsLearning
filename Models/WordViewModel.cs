namespace EnglishWordsLearning.Models;

public class WordViewModel
{
    public int Id { get; set; }
    public required string English { get; set; }
    public required string Russian { get; set; }
    public required string Level { get; set; }
}
