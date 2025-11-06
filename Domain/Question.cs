namespace Domain;

// Part of aggregate
public class Question
{
    public int Id { get; set; }
    
    public int QuizId { get; set; }
    public Quiz Quiz { get; set; } = null!; 

    public string Text { get; set; } = string.Empty;
    public int Duration { get; set; } = 30;
    
    public ICollection<Option> Options { get; set; } = new List<Option>();

    public bool HasCorrectOption()
    {
        return Options.Any(o => o.IsCorrect);
    }
}
