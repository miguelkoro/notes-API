using Notes.Application.Errors;
using Notes.Application.Interfaces;

namespace Notes.Application.Auth;

public class LoginUser
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;

    public LoginUser(IUserRepository userRepository, IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<LoginResult> ExecuteAsync(LoginRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Email))
        {
            return new LoginResult
            {
                Success = false,
                ErrorCode = ApplicationErrorCodes.InvalidCredentials
            };
        }

        if (string.IsNullOrWhiteSpace(request.Password))
        {
            return new LoginResult
            {
                Success = false,
                ErrorCode = ApplicationErrorCodes.InvalidCredentials
            };
        }

        var user = await _userRepository
            .GetByEmailAsync(request.Email);

        if (user == null)
        {
            return new LoginResult
            {
                Success = false,
                ErrorCode = ApplicationErrorCodes.InvalidCredentials
            };
        }

        var passwordIsValid = _passwordHasher.Verify(
            request.Password,
            user.PasswordHash);

        if (!passwordIsValid)
        {
            return new LoginResult
            {
                Success = false,
                ErrorCode = ApplicationErrorCodes.InvalidCredentials
            };
        }

        return new LoginResult
        {
            Success = true,
            Id = user.Id,
            Email = user.Email,
            Role = user.Role
        };
    }
}