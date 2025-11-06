namespace Domain;

public class QuizAccessLog
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public User User { get; set; } = null!;
        
    public int QuizId { get; set; }
    public Quiz Quiz { get; set; } = null!;
        
    public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;
}