namespace Domain;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string PasswordHash { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? LastLogin { get; set; }
    
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiration { get; set; }

    // Navigational property
    public ICollection<UserRole> UserRoles { get; set; } =  new List<UserRole>();
}