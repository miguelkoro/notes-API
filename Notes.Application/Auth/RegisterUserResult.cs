namespace Notes.Application.Auth;

public class RegisterUserResult
{
    public bool Success { get; init; } 

    public string? ErrorCode { get; init; } 

    public int Id { get; init; }

    public string Email { get; init; } = string.Empty;

    public string Role { get; init; } = string.Empty;

    public DateTime CreatedAt { get; init; }
}