namespace Notes.Domain.Entities;

public class User
{
    public int Id { get; private set; }

    public string Email { get; private set; }

    public string PasswordHash { get; private set; }

    public string Role { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public User(
        string email,
        string passwordHash,
        string role = "User")
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email is required.");

        if (string.IsNullOrWhiteSpace(passwordHash))
            throw new ArgumentException("Password hash is required.");

        if (string.IsNullOrWhiteSpace(role))
            throw new ArgumentException("Role is required.");

        Email = email;
        PasswordHash = passwordHash;
        Role = role;
        CreatedAt = DateTime.UtcNow;
    }
}