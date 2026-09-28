
using Notes.Application.Interfaces;
using Notes.Domain.Entities;
using Notes.Application.Errors;
namespace Notes.Application.Auth;

public class RegisterUser
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;

    public RegisterUser(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<RegisterUserResult> ExecuteAsync(RegisterRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Email))
            throw new ApplicationException(ApplicationErrorCodes.InvalidEmail);

        if (string.IsNullOrWhiteSpace(request.Password))
            throw new ApplicationException(ApplicationErrorCodes.InvalidPassword);

        if (request.Password.Length < 8 || request.Password.Length > 100)
            throw new ApplicationException(ApplicationErrorCodes.InvalidPassword);

        var existingUser = await _userRepository
            .GetByEmailAsync(request.Email);

        if (existingUser != null)
            throw new ApplicationException(ApplicationErrorCodes.EmailAlreadyExists);

        var passwordHash = _passwordHasher.Hash(request.Password);

        var user = new User(
            request.Email,
            passwordHash);

        await _userRepository.AddAsync(user);

        // Devolvemos un objeto RegisterUserResult con la información del usuario registrado, sin incluir la contraseña.
        var result = new RegisterUserResult
        {
            Id = user.Id,
            Email = user.Email,
            Role = user.Role,
            CreatedAt = user.CreatedAt
        };

        return result;
    }
}