using Notes.Domain.Entities;

namespace Notes.Application.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email);

    Task AddAsync(User user);

    //Task<bool> ExistsByEmailAsync(string email);
}