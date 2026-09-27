using Microsoft.EntityFrameworkCore;
using Notes.Application.Interfaces;
using Notes.Domain.Entities;
using Notes.Infrastructure.Data;

namespace Notes.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly NotesDbContext _context;

    public UserRepository(NotesDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task AddAsync(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
    }
}