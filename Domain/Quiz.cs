namespace Domain;

// Aggregate Root
public class Quiz
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    
    public int? CategoryId { get; set; }
    public QuizCategory? Category { get; set; } 

    public int? LevelId { get; set; }
    public QuizLevel? Level { get; set; } 

    public int? UserId { get; set; }
    public User? User { get; set; }

    public string CodeToJoin { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public bool IsPublished { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    //Part of aggregate, Quiz has many Questions && Options
    public ICollection<Question> Questions { get; set; } = new List<Question>();

    
    public void Publish()
    {
        if (Questions.Count < 1)
        {
            throw new InvalidOperationException("You cannot publish a quiz without questions.");
        }
        IsPublished = true;
        UpdatedAt = DateTime.UtcNow;
    }

    
    public void GenerateQuizPin()
    {
        CodeToJoin = new Random().Next(100000, 999999).ToString("D4");
    }
}
