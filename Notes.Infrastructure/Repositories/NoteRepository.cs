using Notes.Domain.Entities;
using Notes.Application.Interfaces;
using Notes.Infrastructure.Data;

namespace Notes.Infrastructure.Repositories;

public class NoteRepository : INoteRepository
{
    private readonly NotesDbContext _dbContext;

    public NoteRepository(NotesDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task AddAsync(Note note)
    {
        // guardar en la base de datos
        _dbContext.Notes.Add(note);
        await _dbContext.SaveChangesAsync();
    }
}